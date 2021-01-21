
using OpenThreadDotNet;
using System;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace OpenThreadDotNet
{
    public class SerialStream : IStream
    {
        private SerialDevice serialDevice;
        private string portName;
        private DataReader inputDataReader;
        private DataWriter outputDataWriter;
        private bool headingByte;
        private object syncLockRead = new object();
        private object syncLockWrite = new object();

        public bool IsDataAvailable
        {
            get
            {
                return serialDevice.BytesToRead > 0 ? true : false;
            }
        }

        public event SerialDataReceivedEventHandler SerialDataReceived;

        public SerialStream(string portName)
        {
            this.portName = portName;
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {            
            serialDevice = SerialDevice.FromId(portName);
            serialDevice.BaudRate = 115200;
            serialDevice.Parity = SerialParity.None;
            serialDevice.StopBits = SerialStopBitCount.One;
            serialDevice.Handshake = SerialHandshake.None;
            serialDevice.DataBits = 8;

            inputDataReader = new DataReader(serialDevice.InputStream);
            outputDataWriter = new DataWriter(serialDevice.OutputStream);
            serialDevice.WatchChar = (char)0x7E;
            serialDevice.DataReceived += SerialDevice_DataReceived;

            headingByte = true;
        }

        public byte[] Read()
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            lock (syncLockRead)
            {
                var bytesRead = inputDataReader.Load(1);
                return inputDataReader.ReadByte();               
            }
        }

        public void Write(byte[] data)
        {
            lock (syncLockWrite)
            {
                outputDataWriter.WriteBytes(data);
                var bytes = outputDataWriter.Store();
            }
        }

        private void SerialDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.WatchChar)
            {
                if (headingByte)
                {
                    headingByte = false;
                    return;
                }

                headingByte = true;
                SerialDataReceived();
            }
        }
    }
}
