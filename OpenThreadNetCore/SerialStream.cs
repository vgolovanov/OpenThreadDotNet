using System;
using System.IO.Ports;

namespace OpenThreadDotNet
{
    public class SerialStream : IStream
    {
        private SerialPort serialPort;
        private string portName;

        public event SerialDataReceivedEventHandler SerialDataReceived;

        public SerialStream() { }

        public SerialStream(string portName)
        {
            this.portName = portName;
        }

        public bool IsDataAvailable
        {
            get { return serialPort.BytesToRead > 0 ? true:false; }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            serialPort = new SerialPort(portName, 115200, Parity.None, 8, StopBits.One);
            serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceived);
            try
            {
                serialPort.Open();
                serialPort.DiscardInBuffer();
                serialPort.DiscardOutBuffer();               
            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }

        public byte[] Read()
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] data)
        {
            serialPort.Write(data, 0, data.Length);
        }

        void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialDataReceived();
        }

        public byte ReadByte()
        {
            return Convert.ToByte(serialPort.ReadByte());
        }
    }
}
