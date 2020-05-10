using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThreadDotNet.Networking.IPv6
{
    /// <summary>
    /// The interface of a protocol layer immediately above IPv6.
    /// Examples are transport protocols such as TCP and UDP, control
    /// protocols such as ICMP, routing protocols such as OSPF,
    /// and internet or lower-layer protocols being "tunneled"
    /// over(i.e., encapsulated in) IPv6 such as IPX, AppleTalk, or IPv6 itself.
    /// </summary>
    public interface IPPayload
    {
        /// <summary>
        /// Writes the layer to the byte array.
        /// </summary>
        /// <returns>byte array.</returns>
        byte[] ToBytes();

        /// <summary>
        ///  Builds a packet from byte array.
        /// </summary>
        /// <param name="buffer">The buffer to write the layer to.</param>
        /// <param name="offset">The offset in the buffer to start writing the layer at.</param>
        /// <returns>ILayer.</returns>
        bool FromBytes(byte[] buffer, ref int offset);

        void AddPayload(byte[] payload);
    }
}
