using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThreadDotNet.Networking.IPv6
{
    public class Icmpv6EchoReply : Icmpv6Message
    {
        private static int Icmpv6EchoReplyLength = 4;

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
        /// Initializes a new instance of the <see cref="Icmpv6EchoReply"/> class.
        /// Simple constructor for the ICMPv6 echo request header
        /// </summary>
        /// 

        public Icmpv6EchoReply(Icmpv6EchoRequest icmpv6EchoRequest)
        {
            Id = icmpv6EchoRequest.Id;
            Sequence = icmpv6EchoRequest.Sequence;
            Icmpv6EchoPayload = icmpv6EchoRequest.Icmpv6EchoPayload;
        }

        public bool FromBytes(byte[] buffer, ref int offset)
        {
            throw new NotImplementedException();
        }
    
        public byte[] ToBytes()
        {
            byte[] value = new byte[Icmpv6EchoReplyLength + Icmpv6EchoPayload.Length];

            Array.Copy(BitConverter.GetBytes(Id), 0, value, 0, 2);
            Array.Copy(BitConverter.GetBytes(Sequence), 0, value, 2, 2);
            Array.Copy(Icmpv6EchoPayload, 0, value, 4, Icmpv6EchoPayload.Length);

            return value;
        }
    }
}
