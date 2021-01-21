using OpenThreadDotNet.Networking.IPv6;
using System;
using System.Threading;


namespace OpenThreadDotNet.Networking.Sockets
{
    public enum SelectMode
    {
        SelectRead = 0,
        SelectWrite = 1,
        SelectError = 2
    }

    public class UdpSocket : IDisposable
    {
        internal class ReceivedPacketBuffer
        {
            internal readonly int UDPPacketSize = 1232;// 
            public byte[] Buffer;
            public Int32 BytesReceived;
            public bool IsEmpty;
            public object LockObject;
        }

        protected const Int32 SELECT_MODE_READ = 0;
        protected const Int32 SELECT_MODE_WRITE = 1;
        protected const Int32 SELECT_MODE_ERROR = 2;

        protected bool _isDisposed = false;

        protected const UInt16 IPPortAny = 0x0000;
        protected readonly IPv6Address IPv6Any = IPv6Address.IPv6Any;

        public IPv6Address sourceIpAddress = IPv6Address.IPv6Any;
        public ushort sourcePort = IPPortAny;

        internal IPv6Address destinationIpAddress = IPv6Address.IPv6Any;      
        internal ushort destinationPort = IPPortAny;

        private  ReceivedPacketBuffer receivedPacketBuffer = new ReceivedPacketBuffer();
        private AutoResetEvent receivedPacketBufferFilledEvent = new AutoResetEvent(false);

        private bool _sourceIpAddressAndPortAssigned = false;

        protected int ReceiveTimeout { get; set; }
        protected bool Active { get; set; }
       

        public UdpSocket()
        {
            InitializeReceivedPacketBuffer(receivedPacketBuffer);
        }

        public void Bind(IPv6Address ipAddress, ushort ipPort)
        {
            if (_sourceIpAddressAndPortAssigned)
                throw new SocketsException("Socket is connected.");
             
            _sourceIpAddressAndPortAssigned = true;

            // if ipAddress is IP_ADDRESS_ANY, then change it to to our actual ipAddress.
            if (ipAddress.Equals (IPv6Any))
            {
                sourceIpAddress = NetworkingInterface.IPAddress;
            }
            else
            {
                sourceIpAddress = ipAddress;
            }

            if (ipPort == IPPortAny)
            {
                sourcePort = NetworkingInterface.GetEphemeralPort();
            }
            else
            {
                sourcePort = ipPort;
            }
                

            // verify that this source IP address is correct
            if (sourceIpAddress != NetworkingInterface.IPAddress)
                throw new SocketsException("Source address is not correct.");

            NetworkingInterface.CreateSocket(this);
            ReceiveTimeout = 5000;
        }
       
        public void Connect(IPv6Address ipAddress, UInt16 ipPort)
        {
            if (!_sourceIpAddressAndPortAssigned)
                Bind(IPv6Any, IPPortAny);

            // UDP is connectionless, so the Connect function sets the default destination IP address and port values.
            destinationIpAddress = ipAddress;
            destinationPort = ipPort;
        }

        public void Send(byte[] buffer, int length)
        {
            // make sure that a default destination IPEndpoint has been configured.
            if ((destinationIpAddress == IPv6Any) || (destinationPort == IPPortAny))
                throw new SocketsException("Socket is not connected.");

            UdpDatagram udpDatagram = new UdpDatagram();

            udpDatagram.DestinationPort = destinationPort;
            udpDatagram.SourcePort = sourcePort;
            udpDatagram.AddPayload(buffer);            
            udpDatagram.Checksum = 0;


            IPv6Packet packetUDP = new IPv6Packet();
            packetUDP.SourceAddress = sourceIpAddress;
            packetUDP.DestinationAddress = destinationIpAddress;
            packetUDP.NextHeader =IPv6Protocol.Udp;
            packetUDP.Payload = udpDatagram;
            packetUDP.PayloadLength = udpDatagram.Length;

          
            IPv6PseudoHeader ipv6PseudoHeader = new IPv6PseudoHeader(packetUDP.SourceAddress, packetUDP.DestinationAddress, packetUDP.PayloadLength, (byte)packetUDP.NextHeader);
            ushort checkSum = ipv6PseudoHeader.GetCheckSum();
            checkSum = NetUtilities.ComputeChecksum(checkSum, udpDatagram.ToBytes(), true);

            udpDatagram.Checksum = checkSum;
          
            NetworkingInterface.Send(packetUDP.ToBytes());
        }

        public void PacketHandler(IPv6Packet ipv6Packet)
        {
            UdpDatagram udpDatagram = (UdpDatagram)ipv6Packet.Payload;
            destinationIpAddress = ipv6Packet.SourceAddress;
            destinationPort = udpDatagram.SourcePort;

            /* if we do not have enough room for the incoming frame, discard it */
            if (receivedPacketBuffer.IsEmpty == false)
                return;

            lock (receivedPacketBuffer.LockObject)
            {
                int bytesReceived = udpDatagram.Payload.Length;

                Array.Copy(udpDatagram.Payload, receivedPacketBuffer.Buffer, bytesReceived);
                receivedPacketBuffer.IsEmpty = false;
                receivedPacketBuffer.BytesReceived = bytesReceived;

                receivedPacketBufferFilledEvent.Set();
            }
        }

        public bool Poll(int microSeconds, SelectMode mode)
        {
            switch (mode)
            {
                case SelectMode.SelectRead:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - if Listen has been called and a connection is pending
                     *   - if data is available for reading
                     *   - if the connection has been closed, reset, or terminated
                     * otherwise return false */
                    {
                        /* TODO: check if listen has been called and a connection is pending */
                        //return true;

                        if (receivedPacketBuffer.BytesReceived > 0)
                            return true;

                        /* TODO: check if connection has been closed, reset, or terminated */
                        //return true;

                        /* TODO: only check _isDisposed if the connection hasn't been closed/reset/terminated; those other cases should return TRUE */
                        if (_isDisposed) return false;

                        // in all other circumstances, return false.
                        return false;
                    }
                case SelectMode.SelectWrite:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - processing a Connect and the connection has succeeded
                     *   - if data can be sent
                     * otherwise return false */
                    {
                        if (_isDisposed) return false;
                        
                        if ((destinationIpAddress != IPv6Any) && (destinationPort != 0))
                            return true;
                        else
                            return false;
                    }
                case SelectMode.SelectError:
                    /* [source: MSDN documentation]
                     * return true when:
                     *   - if processing a Connect that does not block--and the connection has failed
                     *   - if OutOfBandInline is not set and out-of-band data is available 
                     * otherwise return false */
                    {
                        if (_isDisposed) return false;

                        return false;
                    }
                default:
                    {
                        // the following line should never be executed
                        return false;
                    }
            }
        }

        public byte[] ReceiveFrom(IPv6EndPoint remoteEPt)
        {           
            throw new NotImplementedException();
        }
            
        public byte[] Receive(ref IPv6EndPoint remoteEP)
        {
            if (!receivedPacketBufferFilledEvent.WaitOne(ReceiveTimeout, false))
            {
                throw new SocketsException("UDP recieve data timeout.");
            }

            byte[] newBuffer = new byte[receivedPacketBuffer.BytesReceived];

            lock (receivedPacketBuffer.LockObject)
            {                                             
                Array.Copy(receivedPacketBuffer.Buffer, 0, newBuffer, 0, receivedPacketBuffer.BytesReceived);
                // now empty our datagram buffer
                InitializeReceivedPacketBuffer(receivedPacketBuffer);               
            }

            IPv6EndPoint endPoint = new IPv6EndPoint(destinationIpAddress, destinationPort);

            remoteEP = endPoint;

            return newBuffer;
        }

        internal void InitializeReceivedPacketBuffer(ReceivedPacketBuffer buffer)
        {         
            if (buffer.Buffer == null)
                buffer.Buffer = new byte[buffer.UDPPacketSize];
            buffer.BytesReceived = 0;

            buffer.IsEmpty = true;
            if (buffer.LockObject == null)
                buffer.LockObject = new object();
        }

        public void Close()
        {
            _isDisposed = true;

            NetworkingInterface.CloseSocket(this);
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
