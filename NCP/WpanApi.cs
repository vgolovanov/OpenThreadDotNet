using System;
using System.Collections;
using System.Threading;
using OpenThreadDotNet.Spinel;

namespace OpenThreadDotNet
{
    public class WpanApi
    {
        private const byte SpinelHeaderFlag = 0x80;
        private IStream stream;
        private Hdlc hdlcInterface;
        private SpinelEncoder mEncoder = new SpinelEncoder();
        private Queue waitingQueue = new Queue();
        private bool isSyncFrameExpecting = false;
        private AutoResetEvent receivedPacketWaitHandle = new AutoResetEvent(false);

        static object locker = new object();

        public event FrameReceivedEventHandler FrameDataReceived;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpanApi"/> class.
        /// </summary>
        /// <param name="stream"></param>
        public WpanApi(IStream stream)
        {
            this.stream = stream;
            this.hdlcInterface = new Hdlc(this.stream);
            this.stream.SerialDataReceived += new DataReceivedEventHandler(StreamDataReceived);
        }

        /// <summary>
        ///
        /// </summary>
        public void Open()
        {
            stream.Open();
        }

        public void DoReset()
        {
            Transact(SpinelCommands.CMD_RESET);
        }

        public int DoLastStatus()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_LAST_STATUS);

            try
            {
                return Converter.ToInt32(frameData.Response);
            }
            catch
            {
                throw new SpinelProtocolExceptions("Interface type format violation");
            }
        }

        public uint[] DoProtocolVersion()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_PROTOCOL_VERSION);

            try
            {               
                return (uint[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Protocol version format violation");
            }
        }

        public string DoNCPVersion()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_NCP_VERSION);

            try
            {
                return frameData.Response.ToString();
            }
            catch
            {
                throw new SpinelProtocolExceptions("Protocol ncp version format violation");
            }
        }

        public string DoVendor()
        {
            FrameData frameData= PropertyGetValue(SpinelProperties.PROP_VENDOR_ID);

            try
            {
                return frameData.Response.ToString();
            }
            catch
            {
                throw new SpinelProtocolExceptions("Vendor id format violation");
            }
        }


        public uint DoInterfaceType()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_INTERFACE_TYPE);

            try
            {
                return (uint) frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Interface type format violation");
            }
        }

        public Capabilities[] DoCaps()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_CAPS);

            try
            {
                Capabilities[] caps = (Capabilities[])frameData.Response;
                return caps;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Caps format violation");
            }
        }

        public string DoNetworkName()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_NETWORK_NAME);

            try
            {
                return frameData.Response.ToString();
            }
            catch
            {
                throw new SpinelProtocolExceptions("Network name format violation");
            }
        }

        public bool DoNetworkName(string networkName)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_NETWORK_NAME, networkName, "U");

            if (frameData != null && frameData.Response.ToString() == networkName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte DoState()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_ROLE );

            try
            {
                return (byte)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Role id format violation");
            }
        }

        public bool DoState(byte role)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_ROLE , role, "C");

            if (frameData != null && Converter.ToByte(frameData.Response) == role)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte DoChannel()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_PHY_CHAN);

            try
            {
                return Converter.ToByte(frameData.Response);
            }
            catch
            {
                throw new SpinelProtocolExceptions("Channel number format violation");
            }
        }

        public bool DoChannel(byte channel)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.PROP_PHY_CHAN, channel, "C");

            if (frameData != null && Converter.ToByte( frameData.Response) == channel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] DoChannels()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_PHY_CHAN_SUPPORTED);

            try
            {
                return (byte[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Supported channels format violation");
            }
        }

        public byte[] DoChannelsMask()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_MAC_SCAN_MASK);

            try
            {
                return (byte[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Channels mask format violation");
            }
        }

        public bool DoChannelsMask(byte[] channels)
        {        
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_MAC_SCAN_MASK, channels, "D");

            if (frameData != null && Utilities.ByteArrayCompare((byte[])frameData.Response, channels))                
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ushort DoPanId()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_MAC_15_4_PANID);

            try
            {
                return Converter.ToUInt16(frameData.Response);
            }
            catch
            {
                throw new SpinelProtocolExceptions("Pan id format violation");
            }
        }

        public bool DoPanId(ushort panId)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_MAC_15_4_PANID, panId, "S");

            if (frameData != null && Converter.ToUInt16(frameData.Response) == panId)
            {
                return true;
            }
            else
            {
                return false;
            }          
        }

        public byte[] DoXpanId()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_XPANID);

            try
            {
                return (byte[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("XPan id format violation");
            }
        }

        public bool DoXpanId(byte[] xpanId)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_XPANID, xpanId, "D");

            if (frameData != null && Utilities.ByteArrayCompare((byte[])frameData.Response , xpanId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public SpinelIPv6Address[] DoIPAddresses()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_IPV6_ADDRESS_TABLE);

            try
            {
                return (SpinelIPv6Address[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("IP addesss format violation");
            }
        }

        public SpinelIPv6Address DoIPLinkLocal64()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_IPV6_LL_ADDR);

            try
            {
                return (SpinelIPv6Address)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("IP addesss format violation");
            }
        }

        public SpinelEUI64 DoExtendedAddress()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_MAC_15_4_LADDR);

            try
            {
                return (SpinelEUI64)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("IP addesss format violation");
            }
        }
     
        public SpinelEUI64 DoPhysicalAddress()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.PROP_HWADDR);

            try
            {
                return (SpinelEUI64)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("IP addesss format violation");
            }
        }



        public SpinelIPv6Address DoIPMeshLocal64()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_IPV6_ML_ADDR);

            try
            {
                return (SpinelIPv6Address)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("IP addesss format violation");
            }
        }

        public bool DoInterfaceConfig()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_IF_UP);
            try
            {
                return (bool)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("XPan id format violation");
            }
        }

        public bool DoInterfaceConfig(bool interfaceState)
        {            
            FrameData frameData;

            if (interfaceState)
            {
                frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_IF_UP, 1, "b");
            }
            else
            {
                frameData =  PropertySetValue(SpinelProperties.SPINEL_PROP_NET_IF_UP, 0, "b");
            }

            if (frameData != null && Converter.ToBoolean(frameData.Response) == interfaceState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoThread()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_STACK_UP );
            try
            {
                return (bool)frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Stack up format violation");
            }
        }

        public bool DoThread(bool threadState)
        {
            FrameData frameData;

            if (threadState)
            {
                frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_STACK_UP , 1, "b");
            }
            else
            {
                frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_STACK_UP , 0, "b");
            }

            if (frameData != null && Converter.ToBoolean(frameData.Response) == threadState)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public byte[] DoMasterkey()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_MASTER_KEY);

            try
            {
                return (byte[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("XPan id format violation");
            }
        }

        public bool DoMasterkey(byte[] masterKey)
        {
            FrameData frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_MASTER_KEY, masterKey, "D");

            if (frameData != null && Utilities.ByteArrayCompare((byte[])frameData.Response , masterKey))
            {               
                return true;
            }
            else
            {
                return false;
            }
        }

        public uint DoPartitionId()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_NET_PARTITION_ID);
            try
            {
                return Converter.ToUInt32( frameData.Response);
            }
            catch
            {
                throw new SpinelProtocolExceptions("Partition id format violation");
            }
        }

        public void DoScan(byte ScanState)
        {                       
            PropertySetValue(SpinelProperties.SPINEL_PROP_MAC_SCAN_STATE, ScanState, "C");                      
        }
       
        public bool DoProperty_NET_REQUIRE_JOIN_EXISTING(bool State)
        {
            FrameData frameData;

            if (State)
            {
                frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_REQUIRE_JOIN_EXISTING, 1, "b");
            }
            else
            {
                frameData = PropertySetValue(SpinelProperties.SPINEL_PROP_NET_REQUIRE_JOIN_EXISTING, 0, "b");
            }

            if (frameData != null && Converter.ToBoolean(frameData.Response) == State)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DoSendData(byte[] frame, bool waitResponse=true)
        {
            byte[] dataCombined = mEncoder.EncodeDataWithLength(frame);

           PropertySetValue(SpinelProperties.PROP_STREAM_NET, dataCombined, "dD", 129, waitResponse);                
        }


        public void DoCountersReset()
        {
            PropertySetValue(SpinelProperties.SPINEL_PROP_CNTR_RESET, 1 , "C");
        }

        public ushort[] DoCountersMessageBuffer()
        {
            FrameData frameData = PropertyGetValue(SpinelProperties.SPINEL_PROP_MSG_BUFFER_COUNTERS);

            try
            {
                return (ushort[])frameData.Response;
            }
            catch
            {
                throw new SpinelProtocolExceptions("Buffer counters format violation");
            }
        }

        public void Transact(int commandId, byte[] payload, byte tID = SpinelCommands.HEADER_DEFAULT)
        {
            byte[] packet = EncodePacket(commandId,tID,payload);
            StreamTx(packet);
        }

        public void Transact(int commandId, byte tID = SpinelCommands.HEADER_DEFAULT)
        {
            Transact(commandId, null, tID);
        }

        public byte[] EncodePacket(int commandId, byte tid = SpinelCommands.HEADER_DEFAULT, params byte[] payload)
        {
            byte[] tidBytes = new byte[1] { tid };
            byte[] commandBytes = mEncoder.EncodeValue(commandId);
            byte[] packet = new byte[commandBytes.Length + tidBytes.Length + (payload == null?0:payload.Length) ];

            if (payload != null)
            {
                packet = Utilities.CombineArrays(tidBytes, commandBytes, payload);
            }
            else
            {
                packet = Utilities.CombineArrays(tidBytes, commandBytes);
            }

            return packet;
        }

        private void StreamTx(byte[] packet)
        {
            hdlcInterface.Write(packet);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StreamRX(int timeout = 0)
        {
            DateTime start = DateTime.UtcNow;
            
            bool dataPooled = false;

            while (true)
            {
                TimeSpan elapsed = DateTime.UtcNow - start;

                if (timeout != 0)
                {
                    if (elapsed.Seconds > timeout)
                    {
                        break;
                    }
                }

                if (stream.IsDataAvailable)
                {
                    byte[] frameDecoded = hdlcInterface.Read();
                    ParseRX(frameDecoded);
                    dataPooled = true;
                }

                if (!stream.IsDataAvailable && dataPooled)
                {
                    break;
                }
            }
        }

        private void ParseRX(byte[] frameIn)
        {

            SpinelDecoder mDecoder = new SpinelDecoder();
            object ncpResponse=null;
            mDecoder.Init(frameIn);

            byte header = mDecoder.FrameHeader;

            if ((SpinelHeaderFlag & header) != SpinelHeaderFlag)
            {
                throw new SpinelFormatException("Header parsing error.");
            }

            uint command = mDecoder.FrameCommand;
            uint properyId = mDecoder.FramePropertyId;

            if (properyId == SpinelProperties.SPINEL_PROP_THREAD_CHILD_TABLE)
            {
                if (command == SpinelCommands.RSP_PROP_VALUE_INSERTED || command == SpinelCommands.RSP_PROP_VALUE_REMOVED)
                {
                    return;
                }
            }

            object tempObj = null;

            switch (properyId)
            {
                case SpinelProperties.PROP_NCP_VERSION:
                    ncpResponse = mDecoder.ReadUtf8();
                    break;

                case SpinelProperties.PROP_LAST_STATUS:
                    ncpResponse = mDecoder.ReadUintPacked();
                    break;

                case SpinelProperties.PROP_INTERFACE_TYPE:
                    ncpResponse = mDecoder.ReadUintPacked();
                    break;

                case SpinelProperties.PROP_VENDOR_ID:
                    ncpResponse = mDecoder.ReadUintPacked();
                    break;

                case SpinelProperties.SPINEL_PROP_NET_NETWORK_NAME:

                    ncpResponse = mDecoder.ReadUtf8();
                    break;

                case SpinelProperties.SPINEL_PROP_MAC_SCAN_STATE:
                    ncpResponse = mDecoder.ReadUint8();
                    break;

                case SpinelProperties.SPINEL_PROP_MAC_SCAN_MASK:                   
                    tempObj = mDecoder.ReadFields("A(C)");

                    if (tempObj != null)
                    {
                        ArrayList channels = (ArrayList)tempObj;
                        ncpResponse = (byte[])channels.ToArray(typeof(byte));
                    }

                    break;

                case SpinelProperties.SPINEL_PROP_MAC_SCAN_PERIOD:
                    ncpResponse = mDecoder.ReadUint16();
                    break;

                case SpinelProperties.SPINEL_PROP_MAC_SCAN_BEACON:
                    ncpResponse = mDecoder.ReadFields("Cct(ESSC)t(iCUdd)");
                    break;

                case SpinelProperties.SPINEL_PROP_MAC_ENERGY_SCAN_RESULT:
                    ncpResponse = mDecoder.ReadFields("Cc");
                    break;
                    
                case SpinelProperties.PROP_PROTOCOL_VERSION:

                    tempObj = mDecoder.ReadFields("ii");

                    if (tempObj != null)
                    {
                        ArrayList protocol = (ArrayList)tempObj;
                        ncpResponse = (uint[])protocol.ToArray(typeof(uint));
                    }

                    break;

                case SpinelProperties.PROP_CAPS:

                    tempObj = mDecoder.ReadFields("A(i)");

                    if (tempObj != null)
                    {
                        ArrayList caps = (ArrayList)tempObj;
                        Capabilities[] capsArray = new Capabilities[caps.Count];
                        int index = 0;

                        foreach (var capsValue in caps)
                        {
                            capsArray[index] = (Capabilities)Converter.ToUInt32(capsValue);
                            index++;
                        }

                        ncpResponse = capsArray;
                    }

                    break;

                case SpinelProperties.SPINEL_PROP_MSG_BUFFER_COUNTERS:
                  
                    tempObj = mDecoder.ReadFields("SSSSSSSSSSSSSSSS");
                    
                    if (tempObj != null)
                    {
                        ArrayList counters = (ArrayList)tempObj;
                        ncpResponse = (ushort[])counters.ToArray(typeof(ushort));
                    }

                    break;

                case SpinelProperties.PROP_PHY_CHAN:
                    ncpResponse = mDecoder.ReadUint8();
                    break;

                case SpinelProperties.PROP_PHY_CHAN_SUPPORTED:
                    tempObj = mDecoder.ReadFields("A(C)");

                    if (tempObj != null)
                    {
                        ArrayList channels = (ArrayList)tempObj;
                        ncpResponse = (byte[])channels.ToArray(typeof(byte));
                    }

                    break;

                case SpinelProperties.SPINEL_PROP_IPV6_ADDRESS_TABLE:

                    tempObj = mDecoder.ReadFields("A(t(6CLL))");
                    ArrayList ipAddresses = new ArrayList();

                    if (tempObj != null)
                    {
                        ArrayList addressArray = tempObj as ArrayList;

                        foreach (ArrayList addrInfo in addressArray)
                        {
                            object[] ipProps = addrInfo.ToArray();
                            SpinelIPv6Address ipaddr = ipProps[0] as SpinelIPv6Address;                           
                            ipAddresses.Add(ipaddr);
                        }
                    }

                    if (ipAddresses.Count > 0)
                    {
                        ncpResponse = ipAddresses.ToArray(typeof(SpinelIPv6Address));
                    }

                    break;

                case SpinelProperties.SPINEL_PROP_NET_IF_UP:
                    ncpResponse = mDecoder.ReadBool();
                    break;

                case SpinelProperties.SPINEL_PROP_NET_STACK_UP :
                    ncpResponse = mDecoder.ReadBool();
                    break;

                case SpinelProperties.SPINEL_PROP_NET_REQUIRE_JOIN_EXISTING:
                    ncpResponse = mDecoder.ReadBool();
                    break;
                    
                case SpinelProperties.SPINEL_PROP_MAC_15_4_PANID:
                    ncpResponse = mDecoder.ReadUint16();
                    break;

                case SpinelProperties.SPINEL_PROP_NET_XPANID:
                    ncpResponse = mDecoder.ReadData();
                    break;

                case SpinelProperties.SPINEL_PROP_NET_ROLE :
                    ncpResponse = mDecoder.ReadUint8();
                    break;
                case SpinelProperties.SPINEL_PROP_NET_MASTER_KEY:
                    ncpResponse = mDecoder.ReadData();
                    break;
                case SpinelProperties.PROP_STREAM_NET:                    
                    tempObj = mDecoder.ReadFields("dD");
                    if (tempObj != null)
                    {
                        ArrayList responseArray = tempObj as ArrayList;
                        ncpResponse = responseArray[0];
                    }                        
                    break;            

                case SpinelProperties.SPINEL_PROP_IPV6_LL_ADDR:
                    SpinelIPv6Address ipaddrLL = mDecoder.ReadIp6Address();                    
                    ncpResponse = ipaddrLL;
                    break;

                case SpinelProperties.SPINEL_PROP_IPV6_ML_ADDR:
                    SpinelIPv6Address ipaddrML = mDecoder.ReadIp6Address();                   
                    ncpResponse = ipaddrML;
                    break;

                case SpinelProperties.SPINEL_PROP_MAC_15_4_LADDR:
                    SpinelEUI64 spinelEUI64 = mDecoder.ReadEui64();
                    ncpResponse = spinelEUI64;
                    break;

                case SpinelProperties.PROP_HWADDR:
                    SpinelEUI64 hwaddr = mDecoder.ReadEui64();
                    ncpResponse = hwaddr;
                    break;
                   
                    //case SpinelProperties.SPINEL_PROP_IPV6_ML_PREFIX:
                    //    ncpResponse = mDecoder.ReadFields("6C");
                    //    break;
            }

            FrameData frameData = new FrameData(mDecoder.FramePropertyId, mDecoder.FrameHeader, mDecoder.GetFrameLoad(),  ncpResponse);

            waitingQueue.Enqueue(frameData);
        }

        private void StreamDataReceived()
        {
                    
            StreamRX();

            receivedPacketWaitHandle.Set();

            if (isSyncFrameExpecting)
            {
                return;
            }

            while (waitingQueue.Count != 0)
            {
                FrameData frameData = waitingQueue.Dequeue() as FrameData;

                FrameDataReceived(frameData);
            }
        }

        private object PropertyChangeValue(int commandId, int propertyId, byte[] propertyValue, string propertyFormat = "B", byte tid= SpinelCommands.HEADER_DEFAULT, bool waitResponse=true)
        {
            FrameData responseFrame=null;
            byte[] payload = mEncoder.EncodeValue(propertyId);

            if (propertyFormat != null)
            {
                payload= Utilities.CombineArrays(payload, propertyValue);
            }

            int uid = Utilities.GetUID(propertyId, tid);

            Console.WriteLine("Transact");
           
            lock (locker)
            {
                Transact(commandId, payload, tid);

                if (!waitResponse) return null;

                isSyncFrameExpecting = true;
            }
                   
            receivedPacketWaitHandle.Reset();

            if (!receivedPacketWaitHandle.WaitOne(115000, false))            
            {
                throw new SpinelProtocolExceptions(string.Format("Timeout for sync packet {0}.", commandId));
            }
         
            if (waitingQueue.Count > 0)
            {
                while (waitingQueue.Count != 0)
                {
                    FrameData frameData = waitingQueue.Dequeue() as FrameData;

                    if (frameData.UID == uid)
                    {
                        responseFrame = frameData;                      
                        isSyncFrameExpecting = false;
                    }
                    else
                    {
                        FrameDataReceived(frameData);
                    }
                }
            }
            else
            {
                throw new SpinelProtocolExceptions(string.Format("No response packet for command {0}.", commandId));
            }

            return responseFrame;
        }

        private FrameData PropertyGetValue(int propertyId, byte tid = SpinelCommands.HEADER_DEFAULT)
        {
            return PropertyChangeValue(SpinelCommands.CMD_PROP_VALUE_GET, propertyId, null, null, tid) as FrameData;
        }

        private FrameData PropertySetValue(int propertyId, ushort propertyValue, string propertyFormat = "B", byte tid = SpinelCommands.HEADER_DEFAULT)
        {
            byte[] propertyValueArray = mEncoder.EncodeValue(propertyValue, propertyFormat);

            return PropertySetValue(propertyId, propertyValueArray, propertyFormat, tid);
        }

        private FrameData PropertySetValue(int propertyId, byte propertyValue, string propertyFormat = "B", byte tid = SpinelCommands.HEADER_DEFAULT)
        {
            byte[] propertyValueArray = mEncoder.EncodeValue(propertyValue, propertyFormat);

            return PropertySetValue(propertyId, propertyValueArray, propertyFormat, tid);
        }

        private FrameData PropertySetValue(int propertyId, string propertyValue, string propertyFormat = "B", byte tid = SpinelCommands.HEADER_DEFAULT)
        {
            byte[] propertyValueArray = mEncoder.EncodeValue(propertyValue, propertyFormat);

            return PropertySetValue(propertyId, propertyValueArray, propertyFormat, tid);
        }

        private FrameData PropertySetValue(int propertyId, byte[] propertyValue, string propertyFormat = "B", byte tid = SpinelCommands.HEADER_DEFAULT, bool waitResponse = true)
        {
            return PropertyChangeValue(SpinelCommands.CMD_PROP_VALUE_SET, propertyId, propertyValue, propertyFormat, tid, waitResponse) as FrameData;
        }       
    }
}
