using System;
using System.Collections;
using System.Threading;
using OpenThreadDotNet.Networking;
using OpenThreadDotNet.Networking.Lowpan;
using OpenThreadDotNet.Networking.Sockets;
using OpenThreadDotNet.Spinel;

namespace OpenThreadDotNet
{
    public class NcpInterface : ILowpanInterface
    {
        private WpanApi wpanApi;

        private string name;
        private string ncpVersion;
        private string protocolVersion;
        private InterfaceType interfaceType;
        private string vendor;
        private Capabilities[] capabilities;
        private byte[] supportedChannels;
        private byte[] scanMask;
        private bool threadStackState;
        private bool networkInterfaceState;
        private State state = Networking.Lowpan.State.Detached;

        private HardwareAddress extendedAddress;
        private HardwareAddress hardwareAddress;
        
        private IPv6Address[] ipAddresses;
        private IPv6Address[] ipMulticastAddresses;
        private IPv6Address ipLinkLocal;
        private IPv6Address ipMeshLocal;

        private uint partitionId = 0;

        private ArrayList scanMacResult = new ArrayList();
        private ArrayList scanEnergyResult = new ArrayList();

        private AutoResetEvent scanThread = new AutoResetEvent(false);

        public event LowpanLastStatusHandler OnLastStatusHandler;
        public event LowpanRoleChanged OnLowpanStateChanged;           
        public event PacketReceivedEventHandler OnPacketReceived;
        public event LowpanIpChanged OnIpChanged;
        
        public string Name
        {
            get { return ncpVersion.Split('/')[0]; }
        }

        public string NcpVersion
        {
            get { return ncpVersion; }
        }

        public string ProtocolVersion
        {
            get { return protocolVersion; }
        }

        public InterfaceType InterfaceType
        {
            get { return interfaceType; }
        }

        public string Vendor
        {
            get { return vendor; }
        }

        public Capabilities[] Capabilities
        {
            get { return capabilities; }
        }

        public byte[] SupportedChannels
        {
            get { return supportedChannels; }
        }

        public byte[] ScanMask
        {
            get { return supportedChannels; }
            set
            {
                if (wpanApi.DoChannelsMask((byte[])value))
                {
                    scanMask = value;
                }
            }
        }

        public HardwareAddress HardwareAddress
        {
            get { return hardwareAddress; }
        }

        public HardwareAddress ExtendedAddress
        {
            get { return extendedAddress; }
        }

        public bool ThreadStackState
        {
            get { return threadStackState; }
        }

        public bool NetworkInterfaceState
        {
            get { return networkInterfaceState; }
        }
       
        public PowerState PowerState => throw new NotImplementedException();

        public State State
        {
            get { return state; }
            set
            {
                if (value != State)
                {
                    if (wpanApi.DoState((byte)value))
                    {
                        state = value;
                    }
                }
            }
        }

        public LowpanIdentity LowpanIdentity { get; set; }

        public LowpanCredential LowpanCredential { get; set; }

        public IPv6Address[] IPAddresses
        {
            get { return ipAddresses; }
        }

        public IPv6Address[] IPMulticastAddresses
        {
            get { return ipMulticastAddresses; }
        }

        public IPv6Address IPLinkLocal
        {
            get { return ipLinkLocal; }
        }

        public IPv6Address IPMeshLocal
        {
            get { return ipMeshLocal; }
        }

        public LastStatus LastStatus => (LastStatus)wpanApi.DoLastStatus();

        public uint PartitionId
        {
            get { return partitionId; }
        }

        public bool Connected 
        {
            get { return state == Networking.Lowpan.State.Detached ? false : true ; }
        }
    
        public bool Commissioned => throw new NotImplementedException();

        public NcpInterface() 
        {           
        }

        public void Open(IStream stream)
        {
            wpanApi = new WpanApi(stream);
            wpanApi.FrameDataReceived += new FrameReceivedEventHandler(FrameDataReceived);
            wpanApi.Open();

            ReadInitialValues();
            NetworkingInterface.SetupInterface(this);
        }

        private void ReadInitialValues()
        {
            LowpanIdentity = new LowpanIdentity(wpanApi);
            LowpanCredential = new LowpanCredential(wpanApi);

            ncpVersion = wpanApi.DoNCPVersion();
            protocolVersion = wpanApi.DoProtocolVersion();
            interfaceType = (InterfaceType)wpanApi.DoInterfaceType();
            vendor = wpanApi.DoVendor();
            capabilities = wpanApi.DoCaps();
            supportedChannels = wpanApi.DoChannels();
            scanMask = wpanApi.DoChannelsMask();

            networkInterfaceState = wpanApi.DoInterfaceConfig();
            threadStackState = wpanApi.DoThread();
            state = (State)wpanApi.DoState();
            extendedAddress = new HardwareAddress(wpanApi.DoExtendedAddress().bytes);
            hardwareAddress = new HardwareAddress(wpanApi.DoPhysicalAddress().bytes);
            ipAddresses = NetUtilities.SpinelIPtoSystemIP(wpanApi.DoIPAddresses());

            ipLinkLocal = new IPv6Address(wpanApi.DoIPLinkLocal64().bytes);
            ipMeshLocal = new IPv6Address(wpanApi.DoIPMeshLocal64().bytes);
        }

        public bool NetworkInterfaceUp()
        {
            if (wpanApi.DoInterfaceConfig(true))
            {
                networkInterfaceState = true;
            }

            return networkInterfaceState;
        }

        public bool NetworkInterfaceDown()
        {
            if (wpanApi.DoInterfaceConfig(false))
            {
                networkInterfaceState = false;
            }

            return networkInterfaceState;
        }

        public bool ThreadUp()
        {
            if (wpanApi.DoThread(true))
            {
                threadStackState = true;
            }

            return threadStackState;
        }

        public bool ThreadDown()
        {
            if (wpanApi.DoThread(false))
            {
                threadStackState = false;
            }

            return threadStackState;
        }
                              
        public LowpanChannelInfo[] ScanEnergy()
        {
            wpanApi.DoScan(2);
            scanThread.WaitOne(10000);
           
            if (scanEnergyResult.Count > 0)
            {
                LowpanChannelInfo[] scanEnergyArray = (LowpanChannelInfo[])scanEnergyResult.ToArray(typeof(LowpanChannelInfo));
                scanEnergyResult.Clear();
                return scanEnergyArray;
            }
            else
            {
                return null;
            }
        }
        
        public LowpanBeaconInfo[] ScanBeacon()
        {
            wpanApi.DoScan(1);
            scanThread.WaitOne(10000);
        
            if (scanMacResult.Count > 0)        
            {            
                LowpanBeaconInfo[] scanMacArray = (LowpanBeaconInfo[])scanMacResult.ToArray(typeof(LowpanBeaconInfo));            
                scanMacResult.Clear();
                return scanMacArray;
            }
            else
            {
                return null;
            }    
        }

        public void EnableLowPower()
        {
            throw new NotImplementedException();
        }

        public void Form(string networkName, byte channel, string masterkey = null, ushort panid = 0xFFFF)
        {           
            if (networkName == string.Empty) throw new ArgumentException("Networkname cannot be null or empty");

            if (channel < 11 || channel > 26) throw new ArgumentException("Channel number should be in between 11 and 26");

            var scanResult = ScanBeacon();

            bool netExisted = false;

            foreach (var beacon in scanResult)
            {
                if (beacon.NetworkName == networkName && beacon.Channel == channel)
                {
                    netExisted = true;
                    break;
                }
            }

            if (netExisted) throw new ArgumentException("Networkname with provided identity already exists.");

            Leave();

            LowpanIdentity.Channel = channel;
            LowpanIdentity.NetworkName = networkName;
            

            if (masterkey != string.Empty)
            {
                this.LowpanCredential.MasterKey = Utilities.HexToBytes(masterkey);
            }

            if (panid != 0xFFFF)
            {
                this.LowpanIdentity.Panid = panid;
            }

            if (!NetworkInterfaceUp()) throw new InvalidOperationException("Interface up exception");          
            if (!ThreadUp()) throw new InvalidOperationException("Thread start exception");

            this.State = State.Leader;
        }
                 
        public void Join(string networkName, byte channel, string masterkey, string xpanid, ushort panid)
        {
            Attach(networkName, channel, masterkey, xpanid, panid, true);
        }

        public void Attach(string networkName, byte channel, string masterkey, string xpanid, ushort panid, bool requireExistingPeers=false)
        {
            if (networkName == string.Empty) throw new ArgumentException("Networkname cannot be null or empty");

            if (channel < 11 || channel > 26) throw new ArgumentException("Channel number should be in between 11 and 26");

            if (masterkey == string.Empty) throw new ArgumentException("Masterkey cannot be null or empty");

            if (xpanid == string.Empty) throw new ArgumentException("Xpanid cannot be null or empty");

            if (panid == 0xFFFF) throw new ArgumentException("Panid value cannot be 0xFFFF");

            Leave();

            this.LowpanCredential.MasterKey = Utilities.HexToBytes(masterkey);

            this.LowpanIdentity.Channel = channel;
            this.LowpanIdentity.NetworkName = networkName;
            this.LowpanIdentity.Panid = panid;
            this.LowpanIdentity.Xpanid = Utilities.HexToBytes(xpanid);

            if (!NetworkInterfaceUp()) throw new InvalidOperationException("Interface up exception");

            if (requireExistingPeers)
            {
                wpanApi.DoProperty_NET_REQUIRE_JOIN_EXISTING(true);
            }
            else
            {
                wpanApi.DoProperty_NET_REQUIRE_JOIN_EXISTING(false);
            }

            if (!ThreadUp()) throw new InvalidOperationException("Thread stack start exception");
        }

        public void Leave()
        {
            ThreadDown();
            NetworkInterfaceDown();
        }

        public void OnHostWake()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            wpanApi.DoReset();
        }
 
        public void SendAndWait(byte[] frame)
        {
            wpanApi.DoSendData(frame);
        }

        public void Send(byte[] frame)
        {
            wpanApi.DoSendData(frame, false);
        }      

        public LowpanCounters GetCounters()
        {
            LowpanCounters lowpanCounters = new LowpanCounters(wpanApi);
            return lowpanCounters;
        }

        private void FrameDataReceived(FrameData frameData)
        {
            uint properyId = frameData.PropertyId;

            if (properyId == SpinelProperties.PROP_LAST_STATUS)
            {
                LastStatus lastStatus = (LastStatus)Convert.ToInt32(frameData.Response);
                OnLastStatusHandler(lastStatus);
                return;
            }
            else if (properyId == SpinelProperties.PROP_STREAM_NET)
            {
                byte[] ipv6frame = (byte[])frameData.Response;

                if (OnPacketReceived != null)
                {
                    OnPacketReceived(this, ipv6frame);
                }

                return;
            }
            else if (properyId == SpinelProperties.SPINEL_PROP_MAC_SCAN_STATE)
            {
                byte scanState = Convert.ToByte(frameData.Response);

                if (scanState == 0)
                {
                    scanThread.Set();
                }
            }
            else if (properyId == SpinelProperties.SPINEL_PROP_MAC_SCAN_BEACON)
            {
                ArrayList scanInfo = (ArrayList)frameData.Response;

                LowpanBeaconInfo lowpanBeaconInfo = new LowpanBeaconInfo();

                lowpanBeaconInfo.Channel = (byte)scanInfo[0];
                lowpanBeaconInfo.Rssi = (sbyte)scanInfo[1];

                ArrayList tempObj = scanInfo[2] as ArrayList;
                SpinelEUI64 mac = (SpinelEUI64)tempObj[0];

                lowpanBeaconInfo.HardwareAddress = new HardwareAddress(mac.bytes);
                lowpanBeaconInfo.ShortAddress = (ushort)tempObj[1];
                lowpanBeaconInfo.PanId = (ushort)tempObj[2];
                lowpanBeaconInfo.LQI = (byte)tempObj[3];

                tempObj = scanInfo[3] as ArrayList;

                lowpanBeaconInfo.Protocol = (uint)tempObj[0];
                lowpanBeaconInfo.Flags = (byte)tempObj[1];
                lowpanBeaconInfo.NetworkName = (string)tempObj[2];
                lowpanBeaconInfo.XpanId = (byte[])tempObj[3];

                scanMacResult.Add(lowpanBeaconInfo);


                return;
            }
            else if (properyId == SpinelProperties.SPINEL_PROP_MAC_ENERGY_SCAN_RESULT)
            {
                ArrayList energyScan = (ArrayList)frameData.Response;

                LowpanChannelInfo lowpanChannelInfo = new LowpanChannelInfo();

                lowpanChannelInfo.Channel = (byte)energyScan[0];
                lowpanChannelInfo.Rssi = (sbyte)energyScan[1];
                scanEnergyResult.Add(lowpanChannelInfo);

                return;
            }

            if (frameData.TID == 0x80)
            {
                switch (properyId)
                {
                    case SpinelProperties.SPINEL_PROP_NET_ROLE :
                        State newRole = (State)Convert.ToByte(frameData.Response);
                        if (state != newRole)
                        {
                            state = newRole;
                            OnLowpanStateChanged();
                        }
                        break;

                    case SpinelProperties.SPINEL_PROP_IPV6_LL_ADDR:

                        if (frameData.Response == null)
                        {
                            ipLinkLocal = null;
                            return;
                        }

                        SpinelIPv6Address ipaddrLL = (SpinelIPv6Address)frameData.Response;
                        ipLinkLocal = new IPv6Address(ipaddrLL.bytes);

                        if (OnIpChanged != null)
                        {
                            OnIpChanged();
                        }

                        break;

                    case SpinelProperties.SPINEL_PROP_IPV6_ML_ADDR:

                        if (frameData.Response == null)
                        {
                            ipMeshLocal = null;
                            return;
                        }

                        SpinelIPv6Address ipaddrML = (SpinelIPv6Address)frameData.Response;
                        ipMeshLocal = new IPv6Address(ipaddrML.bytes);
                        break;

                    case SpinelProperties.SPINEL_PROP_IPV6_ADDRESS_TABLE:
                        ipAddresses = NetUtilities.SpinelIPtoSystemIP((SpinelIPv6Address[])frameData.Response);
                        break;

                    case SpinelProperties.SPINEL_PROP_IPV6_MULTICAST_ADDRESS_TABLE:
                        ipMulticastAddresses = NetUtilities.SpinelIPtoSystemIP((SpinelIPv6Address[])frameData.Response);
                        break;

                    //case SpinelProperties.PROP_NET_SAVED:
                    //case SpinelProperties.PROP_NET_IF_UP:
                    //    break;
                    //case SpinelProperties.PROP_NET_STACK_UP:
                    //    break;

                    //case SpinelProperties.PROP_NET_NETWORK_NAME:
                    //case SpinelProperties.PROP_NET_XPANID:
                    //case SpinelProperties.PROP_NET_MASTER_KEY:
                    //case SpinelProperties.PROP_NET_KEY_SEQUENCE_COUNTER:
                    //case SpinelProperties.PROP_NET_PARTITION_ID:
                    //case SpinelProperties.PROP_NET_KEY_SWITCH_GUARDTIME:
                    //    break;

                    //case SpinelProperties.SPINEL_PROP_THREAD_LEADER_ADDR:
                    //case SpinelProperties.SPINEL_PROP_THREAD_PARENT:
                    //case SpinelProperties.SPINEL_PROP_THREAD_CHILD_TABLE:
                    //case SpinelProperties.SPINEL_PROP_THREAD_LEADER_RID:
                    //case SpinelProperties.SPINEL_PROP_THREAD_LEADER_WEIGHT:
                    //case SpinelProperties.SPINEL_PROP_THREAD_LOCAL_LEADER_WEIGHT:
                    //case SpinelProperties.SPINEL_PROP_THREAD_NETWORK_DATA:
                    //case SpinelProperties.SPINEL_PROP_THREAD_NETWORK_DATA_VERSION:
                    //case SpinelProperties.SPINEL_PROP_THREAD_STABLE_NETWORK_DATA:
                    //case SpinelProperties.SPINEL_PROP_THREAD_STABLE_NETWORK_DATA_VERSION:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ASSISTING_PORTS:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ALLOW_LOCAL_NET_DATA_CHANGE:
                    //case SpinelProperties.SPINEL_PROP_THREAD_MODE:
                    //    break;
                    //case SpinelProperties.SPINEL_PROP_THREAD_ON_MESH_NETS:
                    //    break;
                    //case SpinelProperties.SPINEL_PROP_THREAD_OFF_MESH_ROUTES:
                    //    break;



                    //case SpinelProperties.SPINEL_PROP_IPV6_ML_PREFIX:
                    //    break;

                    //case SpinelProperties.SPINEL_PROP_IPV6_ROUTE_TABLE:
                    //case SpinelProperties.SPINEL_PROP_IPV6_ICMP_PING_OFFLOAD:
                    //case SpinelProperties.SPINEL_PROP_IPV6_ICMP_PING_OFFLOAD_MODE:
                    //    break;



                    //case SpinelProperties.SPINEL_PROP_THREAD_CHILD_TIMEOUT:
                    //case SpinelProperties.SPINEL_PROP_THREAD_RLOC16:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ROUTER_UPGRADE_THRESHOLD:
                    //case SpinelProperties.SPINEL_PROP_THREAD_CONTEXT_REUSE_DELAY:
                    //case SpinelProperties.SPINEL_PROP_THREAD_NETWORK_ID_TIMEOUT:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ACTIVE_ROUTER_IDS:
                    //case SpinelProperties.SPINEL_PROP_THREAD_RLOC16_DEBUG_PASSTHRU:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ROUTER_ROLE_ENABLED:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ROUTER_DOWNGRADE_THRESHOLD:
                    //case SpinelProperties.SPINEL_PROP_THREAD_ROUTER_SELECTION_JITTER:
                    //case SpinelProperties.SPINEL_PROP_THREAD_PREFERRED_ROUTER_ID:
                    //case SpinelProperties.SPINEL_PROP_THREAD_CHILD_COUNT_MAX:
                    //    break;
                    //case SpinelProperties.SPINEL_PROP_THREAD_NEIGHBOR_TABLE:
                    //    break;
                    //case SpinelProperties.SPINEL_PROP_THREAD_LEADER_NETWORK_DATA:
                    //    break;

                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_NEW_CHANNEL:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_DELAY:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_SUPPORTED_CHANNELS:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_FAVORED_CHANNELS:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_CHANNEL_SELECT:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_AUTO_SELECT_ENABLED:
                    //case SpinelProperties.SPINEL_PROP_CHANNEL_MANAGER_AUTO_SELECT_INTERVAL:
                    //case SpinelProperties.SPINEL_PROP_THREAD_NETWORK_TIME:
                    //case SpinelProperties.SPINEL_PROP_TIME_SYNC_PERIOD:
                    //case SpinelProperties.SPINEL_PROP_TIME_SYNC_XTAL_THRESHOLD:
                    //case SpinelProperties.SPINEL_PROP_CHILD_SUPERVISION_INTERVAL:
                    //case SpinelProperties.SPINEL_PROP_CHILD_SUPERVISION_CHECK_TIMEOUT:
                    //case SpinelProperties.SPINEL_PROP_RCP_VERSION:
                    //case SpinelProperties.SPINEL_PROP_SLAAC_ENABLED:
                    //    break;
                    //case SpinelProperties.SPINEL_PROP_PARENT_RESPONSE_INFO:
                    //    break;
                    //case SpinelProperties.PROP_LAST_STATUS:

                    //    LastStatus lastStatus = (LastStatus)Convert.ToInt32(frameData.Response);
                    //    OnLastStatusHandler(lastStatus);
                    //    break;
                    default:
                        break;

                }
            }
        }
    }
}
