using GHIElectronics.TinyCLR.Pins;
using OpenThreadDotNet;
using OpenThreadDotNet.Networking.Sockets;
using OpenThreadTinyCLR;
using System;
using System.Diagnostics;
using System.Text;

namespace SamplesNanoTinyCLR
{
    class Program
    {
        private static NcpInterface ncpInterface = new NcpInterface();
        private static ushort port = 1234;
        private static string networkname = "OpenThreadNano";
        private static string masterkey = "00112233445566778899aabbccddeeff";
        private static byte channel = 11;
        private static ushort panid = 1000;

        static void Main()
        {
            SerialStream serialStream = new SerialStream(SC20100.UartPort.Uart4);

            try
            {
                ncpInterface.Form(networkname, channel, masterkey, panid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);            
                return;
            }

            UdpSocket receiver = new UdpSocket();
            receiver.Bind(IPv6Address.IPv6Any, port);
            IPv6EndPoint remoteIp = null;

            while (true)
            {
                if (receiver.Poll(-1, SelectMode.SelectRead))
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    string message = Encoding.UTF8.GetString(data, 0, data.Length);
                    Debug.WriteLine("\n");
                    Debug.WriteLine(message.Length + " bytes from " + remoteIp.Address + " " + remoteIp.Port + " " + message);
                    Debug.WriteLine(">");
                }
            }
        }
    }
}
