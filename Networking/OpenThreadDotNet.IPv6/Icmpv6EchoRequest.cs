using System;

namespace OpenThreadDotNet.Networking.IPv6
{
    /// <summary>
    /// Class representing the ICMPv6 echo request header. Since the ICMPv6 protocol is
    /// used for a variety of different functions other than "ping", this header is
    /// broken out from the base ICMPv6 header that is common across all of its functions
    /// (such as Multicast Listener Discovery, Neighbor Discovery, etc.).
    /// </summary>
    public class Icmpv6EchoRequest : Icmpv6Message
    {
        private static int Icmpv6EchoRequestLength = 4;

        /// <summary>
        /// Gets and sets the ID configuredOneShotParams. Also performs the necessary byte order conversion.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets and sets the echo sequence configuredOneShotParams. Also performs the necessary byte order conversion.
        /// </summary>
        public ushort Sequence { get; set; }

        public byte[] Icmpv6EchoPayload { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Icmpv6EchoRequest"/> class.
        /// Simple constructor for the ICMPv6 echo request header
        /// </summary>
        /// 
        public Icmpv6EchoRequest()
            : base()
        {
            Id = 0;
            Sequence = 0;
        }

        public bool FromBytes(byte[] buffer, ref int offset)
        {
            // Verify buffer is large enough
            if (buffer.Length < Icmpv6EchoRequest.Icmpv6EchoRequestLength)
            {
                return false;
            }

            // Properties are stored in network byte order so just grab the bytes
            //    from the buffer
            Id = System.BitConverter.ToUInt16(buffer, offset + 0);
            Sequence = System.BitConverter.ToUInt16(buffer, offset + 2);

            offset += Icmpv6EchoRequest.Icmpv6EchoRequestLength;

            int payloadLength = buffer.Length - offset;

            Icmpv6EchoPayload = new byte[payloadLength];

            Array.Copy(buffer, offset, Icmpv6EchoPayload, 0, payloadLength);

            var bytes = BitConverter.GetBytes(Sequence);
          
            return true;
        }

        public byte[] ToBytes()
        {
            byte[] value = new byte[Icmpv6EchoRequestLength + Icmpv6EchoPayload.Length];

            Array.Copy(BitConverter.GetBytes(Id), 0, value, 0, 2);
            Array.Copy(BitConverter.GetBytes(Sequence), 0, value, 2, 2);
            Array.Copy(Icmpv6EchoPayload, 0, value, 4, Icmpv6EchoPayload.Length);

            return value;
        }
    }
}
