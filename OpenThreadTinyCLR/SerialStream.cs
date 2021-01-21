using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Pins;
using OpenThreadDotNet;
using System;
using System.Collections;
using System.Text;
using System.Threading;

namespace OpenThreadTinyCLR
{
    public class SerialStream : IStream
    {
        public event SerialDataReceivedEventHandler SerialDataReceived;
        private UartController serialDevice;        
        private byte[] rxBuffer = new byte[1];
        private string portName;

        public SerialStream() { }

        public SerialStream(string portName)
        {
            this.portName = portName;
        }

        public bool IsDataAvailable
        {
            get
            {
                return serialDevice.BytesToRead > 0 ? true : false;
            }
        }
          
        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            serialDevice = UartController.FromName(portName);

            var uartSetting = new UartSetting()
            {
                BaudRate = 115200,
                DataBits = 8,
                Parity = UartParity.None,
                StopBits = UartStopBitCount.One,
                Handshaking = UartHandshake.None,
            };

            serialDevice.SetActiveSettings(uartSetting);
            serialDevice.DataReceived += DataReceived;
            
            try
            {
                serialDevice.Enable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte[] Read()
        {
            throw new NotImplementedException();
        }

        public byte ReadByte()
        {
            serialDevice.Read(rxBuffer, 0, 1);
            return rxBuffer[0];
        }

        public void Write(byte[] data)
        {
            serialDevice.Write(data, 0, data.Length);
        }

        private void DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            SerialDataReceived();         
        }
    }
}
