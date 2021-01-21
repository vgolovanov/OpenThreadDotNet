using System;

namespace OpenThreadDotNet.Networking.IPv6
{
    /// <summary>
    ///
    /// </summary>
    public class UdpDatagram : IPPayload
    {
        private const byte UdpHeaderLength = 8;
        /// <summary>
        /// Gets and sets the destination port. Performs the necessary byte order conversion.
        /// </summary>
        public ushort SourcePort { get; set; }
       
        /// <summary>
        /// Gets and sets the destination port. Performs the necessary byte order conversion.
        /// </summary>
        public ushort DestinationPort { get; set; }
        /// <summary>
        /// Gets and sets the UDP payload length. This is the length of the payload
        /// plus the size of the UDP header itself.
        /// </summary>
        public ushort Length { get; set; }       
        /// <summary>
        /// Gets and sets the checksum value. It performs the necessary byte order conversion.
        /// </summary>
        public ushort Checksum { get; set; }

        public byte[] Payload { get; set; }

        public void AddPayload(byte[] payload)
        {
            Payload = new byte[payload.Length];           
            Array.Copy(payload, Payload, payload.Length);
            Length = (ushort)(Payload.Length + UdpHeaderLength);
        }

        public bool FromBytes(byte[] ipv6Packet, ref int frameIndex)
        {
            // Verify buffer is large enough to contain an ICMPv6 header
            if (ipv6Packet.Length < UdpHeaderLength)
            {
                return false;
            }

            SourcePort = NetUtilities.ToLittleEndian(BitConverter.ToUInt16(ipv6Packet, frameIndex + 0));
            DestinationPort = NetUtilities.ToLittleEndian(BitConverter.ToUInt16(ipv6Packet, frameIndex + 2));
            Length = NetUtilities.ToLittleEndian(BitConverter.ToUInt16(ipv6Packet, frameIndex + 4));
            Checksum = NetUtilities.ToLittleEndian(BitConverter.ToUInt16(ipv6Packet, frameIndex + 6));

            Payload = new byte[Length - UdpHeaderLength];

            Array.Copy(ipv6Packet, frameIndex + UdpHeaderLength, Payload, 0, Length - UdpHeaderLength);

            frameIndex += UdpHeaderLength;

            return true;
        }

        public byte[] ToBytes()
        {
            byte[] udpHeader = new byte[UdpHeaderLength];

            Array.Copy(NetUtilities.FromLittleEndian(SourcePort), 0, udpHeader, 0, 2);
            Array.Copy(NetUtilities.FromLittleEndian(DestinationPort), 0, udpHeader, 2, 2);
            Array.Copy(NetUtilities.FromLittleEndian(Length), 0, udpHeader, 4, 2);
            Array.Copy(NetUtilities.FromLittleEndian(Checksum), 0, udpHeader, 6, 2);

            if (Payload != null)
            {                              
                return Utilities.CombineArrays(udpHeader, Payload);
            }
            else
            {
                return udpHeader;
            }
        }        
    }
}
