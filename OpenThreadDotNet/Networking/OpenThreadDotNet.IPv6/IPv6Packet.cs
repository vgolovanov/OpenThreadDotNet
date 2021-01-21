using OpenThreadDotNet.Networking.Sockets;
using System;

namespace OpenThreadDotNet.Networking.IPv6
{
    /// <summary>
    /// Indicates the next level IPv6 protocol used in the pyaload of the IPv6 datagram.
    /// </summary>
    public enum IPv6Protocol : byte
    {
        /// <summary>
        /// User Datagram Protocol RFC 768   
        /// </summary>
        Udp = 0x11,
        /// <summary>
        /// ICMP for IPv6 RFC 2460   
        /// </summary>
        ICMPv6 = 0x3A,
    }

    public class IPv6Packet
    {
        private const int Ipv6HeaderLength = 40;

        #region Header
        /// <summary>
        /// Gets or sets and sets the IP version. This value should be 6.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// Gets or sets and sets the traffic class for the header. 
        /// </summary>
        public byte TrafficClass { get; set; }

        /// <summary>
        /// Gets or sets and sets the flow value for the packet. Byte order conversion
        /// is required for this field.
        /// </summary>
        public uint Flow { get; set; }

        /// <summary>
        /// Gets or sets and sets the payload length for the IPv6 packet. Note for IPv6, the
        /// payload length counts only the payload and not the IPv6 header (since
        /// the IPv6 header is a fixed length). Byte order conversion is required.
        /// </summary>
        public ushort PayloadLength { get; set; }

        /// <summary>
        /// Gets or sets the protocol value of the header encapsulated by the IPv6 header.
        /// </summary>
        public IPv6Protocol NextHeader { get; set; }

        /// <summary>
        /// Gets or sets time-to-live (TTL) of the IPv6 header.
        /// </summary>
        public byte HopLimit { get; set; }

        /// <summary>
        /// Gets or sets iPv6 source address in the IPv6 header.
        /// </summary>
        public IPv6Address SourceAddress { get; set; }

        /// <summary>
        /// Gets or sets iPv6 destination address in the IPv6 header.
        /// </summary>
        public IPv6Address DestinationAddress { get; set; }
        #endregion
        
        public IPPayload Payload;

        private int packetIndex = 0;

        public IPv6Packet()
        {
            uint tempVal1, tempVal2, tempVal3 ;

            tempVal1 = 0x60;          
            tempVal1 = (tempVal1 >> 4) & 0xF;
            Version = (byte)tempVal1;

            tempVal1 = 0x60;
            tempVal2 = 0;
            tempVal1 = (tempVal1 & 0xF) >> 4;
            TrafficClass = (byte)(tempVal1 | (uint)((tempVal2 >> 4) & 0xF));

            tempVal1 = 0;
            tempVal2 = 0;
            tempVal3 = 0;

            tempVal2 = (tempVal2 & 0xF) << 16;
             
            tempVal1 = tempVal1 << 8;
            Flow = tempVal2 | tempVal1 | tempVal3;

            PayloadLength = 0;
            NextHeader = 0;
            HopLimit = 0x40;
        }

        public bool FromBytes(byte[] ipv6Packet)
        {

            byte[] addressBytes = new byte[16];
            uint tempVal = 0, tempVal2 = 0;

            // Ensure byte array is large enough to contain an IPv6 header
            if (ipv6Packet.Length < Ipv6HeaderLength)
            {
                return false;
            }

            tempVal = ipv6Packet[0];
            tempVal = (tempVal >> 4) & 0xF;
            Version = (byte)tempVal;

            tempVal = ipv6Packet[0];
            tempVal = (tempVal & 0xF) >> 4;
            TrafficClass = (byte)(tempVal | (uint)((ipv6Packet[1] >> 4) & 0xF));

            tempVal2 = ipv6Packet[1];
            tempVal2 = (tempVal2 & 0xF) << 16;
            tempVal = ipv6Packet[2];
            tempVal = tempVal << 8;
            Flow = tempVal2 | tempVal | ipv6Packet[3];
            PayloadLength = NetUtilities.ToLittleEndian(BitConverter.ToUInt16(ipv6Packet, 4));
            NextHeader = (IPv6Protocol)ipv6Packet[6];
            HopLimit = ipv6Packet[7];

            Array.Copy(ipv6Packet, 8, addressBytes, 0, 16);
            SourceAddress = new IPv6Address(addressBytes);

            Array.Copy(ipv6Packet, 24, addressBytes, 0, 16);
            DestinationAddress = new IPv6Address(addressBytes);

            packetIndex += Ipv6HeaderLength;

            IPv6PseudoHeader ipv6PseudoHeader = new IPv6PseudoHeader(SourceAddress, DestinationAddress, PayloadLength, (byte)NextHeader);
            ushort checkSum = ipv6PseudoHeader.GetCheckSum();

            if (ipv6Packet.Length > packetIndex)
            {
                switch (NextHeader)
                {
                    case IPv6Protocol.ICMPv6:

                        Icmpv6Packet icmpv6packet = new Icmpv6Packet();
                        if ((icmpv6packet.FromBytes(ipv6Packet, ref packetIndex)) == false) return false;                 
                        Payload = icmpv6packet;
                        
                        checkSum = NetUtilities.ComputeChecksum(checkSum, Payload.ToBytes(), true);

                        if (checkSum != 0)
                        {
                            return false;
                        }

                        break;

                    case IPv6Protocol.Udp:
                        
                        UdpDatagram udpDatagram = new UdpDatagram();                    
                        if ((udpDatagram.FromBytes(ipv6Packet, ref packetIndex)) == false) return false;
                        Payload = udpDatagram;

                        checkSum = NetUtilities.ComputeChecksum(checkSum, Payload.ToBytes(), true);

                        if (checkSum != 0)
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        public byte[] ToBytes()
        {
            byte[] byteValue;
            byte[] payload = Payload.ToBytes();
            byte[] ipv6Packet = new byte[Ipv6HeaderLength+ payload.Length];
            int offset = 0;

            ipv6Packet[offset++] = (byte)((Version << 4) | ((TrafficClass >> 4) & 0xF));
            ipv6Packet[offset++] = (byte)((uint)((TrafficClass << 4) & 0xF0) | (uint)((Flow >> 16) & 0xF));
            ipv6Packet[offset++] = (byte)((Flow >> 8) & 0xFF);
            ipv6Packet[offset++] = (byte)(Flow & 0xFF);


            byteValue = NetUtilities.FromLittleEndian(PayloadLength);
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;

            ipv6Packet[offset++] = (byte)NextHeader;
            ipv6Packet[offset++] = (byte)HopLimit;

            byteValue = SourceAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;

            byteValue = DestinationAddress.GetAddressBytes();
            Array.Copy(byteValue, 0, ipv6Packet, offset, byteValue.Length);
            offset += byteValue.Length;
          
            Array.Copy(payload, 0, ipv6Packet, offset, payload.Length);

            return ipv6Packet;          
        }              
    }
}
