using System;
using System.IO.Ports;
using System.Text;
using OpenThreadDotNet;
using OpenThreadDotNet.Networking;
using OpenThreadDotNet.Networking.Sockets;

namespace Samples
{
    class FormLowpanNetworkAndUDPListener
    {
        private static NcpInterface ncpInterface;

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                string[] ports = SerialPort.GetPortNames();
                Console.WriteLine("COM port parameter not provided.");
                Console.WriteLine("Available serial ports: ");
                foreach (var serialPort in ports)
                {
                    Console.WriteLine(serialPort);
                }
                Console.ReadKey();
                return;
            }

            StreamUART uartStream = new StreamUART(args[0]);

            ncpInterface = new NcpInterface();          

            try
            {
                ncpInterface.Open(uartStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

           // NetworkingInterface.SetupInterface(ncpInterface);

            Console.Write("Networkname:");
            string networkname = Console.ReadLine();

            Console.Write("Channel:");
            byte channel = Convert.ToByte(Console.ReadLine());

            Console.Write("Masterkey:");
            string masterkey = Console.ReadLine();

            Console.Write("Panid:");
            ushort panid = Convert.ToUInt16(Console.ReadLine());

            Console.Write("Listener port:");
            ushort port = Convert.ToUInt16(Console.ReadLine());

            try
            {
                ncpInterface.Form(networkname, channel, masterkey, panid);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
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
                    string message = Encoding.ASCII.GetString(data);
                    Console.WriteLine("\n");
                    Console.WriteLine("{0} bytes from {1} {2} {3}", message.Length, remoteIp.Address, remoteIp.Port, message);
                    Console.WriteLine(">");
                }
            }

        }
    }
}
