#define NETCORE
using System;
using System.IO.Ports;
using System.Text;
using OpenThreadDotNet;
using OpenThreadDotNet.Networking.Sockets;
using OpenThreadDotNet.Networking.Lowpan;
using System.Collections;

namespace Samples
{
    class FormLowpanNetworkAndUDPListener
    {
        private static NcpInterface ncpInterface;

        private static ushort port = 1234;
        private static string networkname = "OpenThreadCore";
        private static string masterkey = "00112233445566778899aabbccddeeff";
        private static byte channel = 11;
        private static ushort panid = 1000;

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
            
            SerialStream serialStream = new SerialStream(args[0]);            
            ncpInterface = new NcpInterface();          

            try
            {
                ncpInterface.Open(serialStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                return;
            }

            string tempString;

            Console.WriteLine("Enter new values or press enter to keep default values.");
            Console.WriteLine();

            Console.Write("Networkname: {0} ? ", networkname);
            tempString = Console.ReadLine();
            if (tempString != string.Empty && tempString != networkname)
            {
                networkname = tempString;
            }
         
            Console.Write("Channel:  {0} ? ", channel.ToString());
            tempString = Console.ReadLine();
            if (tempString != string.Empty && Convert.ToByte(tempString) != channel)
            {
                channel = Convert.ToByte(tempString);
            }
           
            Console.Write("Masterkey: {0} ? ", masterkey);
            tempString = Console.ReadLine();           
            if (tempString != string.Empty && masterkey != tempString)
            {
                masterkey = tempString;
            }

            Console.Write("Panid: {0} ? ", panid);
            tempString = Console.ReadLine();
            if (tempString != string.Empty && Convert.ToUInt16(tempString) != panid)
            {
                panid = Convert.ToUInt16(tempString);
            }
        
            Console.Write("Listener port: {0} ? ", port);
            tempString = Console.ReadLine();
            if (tempString != string.Empty && Convert.ToUInt16(tempString) != port)
            {
                port = Convert.ToUInt16(tempString);
            }

            ncpInterface.OnLastStatusHandler += OnLastStatus;

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

        private static void OnLastStatus(LastStatus lastStatus)
        {
            if (lastStatus.ToString().ToLower() != "ok")
            {
                Console.WriteLine(lastStatus.ToString());
            }
        }
    }
}
