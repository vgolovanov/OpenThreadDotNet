using static OpenThreadDotNet.WpanApi;

namespace OpenThreadDotNet
{
    /// <summary>
    /// Interface for serial data stream.
    /// </summary>
    public interface IStream
    {
        /// <summary>
        /// Public event for recieving serial data.
        /// </summary>
        event DataReceivedEventHandler SerialDataReceived;

        /// <summary>
        /// Gets a value indicating whether true if the data in buffer is available for reading.
        /// </summary>
        bool IsDataAvailable
        {
            get;
        }

        /// <summary>
        /// Open the stream.
        /// </summary>
        void Open();

        /// <summary>
        /// Read an array of byte integers from the stream.
        /// </summary>
        /// <returns>Byte array.</returns>
        byte[] Read();

        /// <summary>
        /// Read a byte from the stream.
        /// </summary>
        /// <returns>Byte.</returns>
        byte ReadByte();

        /// <summary>
        /// Write the given packed data to the stream.
        /// </summary>
        /// <param name="data."></param>
        void Write(byte[] data);

        /// <summary>
        /// Close the stream cleanly as needed.
        /// </summary>
        void Close();
    }
}
