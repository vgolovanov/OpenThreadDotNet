using System;

namespace OpenThreadDotNet.Networking.IPv6
{
    /// <summary>
    /// The different ICMP message types.
    /// </summary>
    public enum Icmpv6MessageType : byte
    {
        DestinationUnreachable = 0x01,

        EchoRequest = 0x80,

        EchoReply = 0x81,
    }

    public class Icmpv6Packet : IPPayload
    {
        private const int Icmpv6HeaderLength = 4;                    // ICMPv6 header length        
        
        /// <summary>
        /// Gets or sets the ICMPv6 message type.
        /// </summary>
        public Icmpv6MessageType Icmpv6MessageType { get; set; }

        /// <summary>
        /// Gets or sets the ICMPv6 code type.
        /// </summary>
        public byte Code { get; set; }

        /// <summary>
        /// Gets or sets the ICMPv6 checksum value. This value is computed over the ICMPv6 header, payload,
        /// and the IPv6 header as well.
        /// </summary>
        public ushort Checksum { get; set; }

        public Icmpv6Message IcmpMessage { get; set; }

        public void AddPayload(byte[] payload)
        {
            throw new NotImplementedException();
        }

        public bool FromBytes(byte[] ipv6Packet, ref int offset)
        {
            // Verify buffer is large enough to contain an ICMPv6 header
            if (ipv6Packet.Length < Icmpv6HeaderLength)
            {
                return false;
            }

            Icmpv6MessageType = (Icmpv6MessageType)ipv6Packet[offset];
            Code = ipv6Packet[offset + 1];          
            Checksum = NetUtilities.ToLittleEndian( BitConverter.ToUInt16(ipv6Packet, offset + 2));

            offset += Icmpv6HeaderLength;

            if (Icmpv6MessageType == Icmpv6MessageType.EchoRequest)
            {
                IcmpMessage = new Icmpv6EchoRequest();
                IcmpMessage.FromBytes(ipv6Packet, ref offset);
            }

            return true;
        }

        public byte[] ToBytes()
        {
            byte[] icmpv6message=null;

            if (IcmpMessage != null)
            {
                icmpv6message = IcmpMessage.ToBytes();
            }
                   
            byte[] icmpv6packet = new byte[Icmpv6HeaderLength];
            

            icmpv6packet[0] = (byte)Icmpv6MessageType;
            icmpv6packet[1] = Code;

            Array.Copy(NetUtilities.FromLittleEndian(Checksum), 0, icmpv6packet, 2, 2);

            if (icmpv6message != null)
            {                
                return Utilities.CombineArrays(icmpv6packet, icmpv6message);
            }
            else
            {
                return icmpv6packet;
            }        
        }    
    }
}
