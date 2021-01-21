namespace OpenThreadDotNet.Networking.IPv6
{
    public interface Icmpv6Message
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

    }
}
