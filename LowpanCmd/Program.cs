using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using OpenThreadDotNet;
using OpenThreadDotNet.Networking;
using OpenThreadDotNet.Networking.IPv6;
using OpenThreadDotNet.Networking.Lowpan;
using OpenThreadDotNet.Networking.Sockets;
using OpenThreadDotNet.Spinel;

namespace LowpanCmd
{
    class Program
    {       
        private static NcpInterface ncpInterface;
        private static bool isUdpOpen = false;
        private static Thread threadUdpListening;
        private static bool isUdpListenerRunning = true;
       
        private static void OnLastStatus(LastStatus lastStatus)
        {
            if (lastStatus.ToString().ToLower() != "ok")
            {
                Console.WriteLine(lastStatus.ToString());
            }          
        }

        private static void OnStateChanged()
        {
            Console.WriteLine();
            Console.WriteLine(ncpInterface.State);
        }
          
        static void Main(string[] args)
        {          
            if (args.Length != 1)
            {
                string[] ports = SerialPort.GetPortNames();              
                Console.WriteLine("COM port parameter not provided.");
                Console.WriteLine("Available serial ports: ");
                foreach (var port in ports)
                {
                    Console.WriteLine(port);
                }
                Console.ReadKey();
                return;
            }
        
            StreamUART uartStream = new StreamUART(args[0]);

            ncpInterface = new NcpInterface();
            ncpInterface.OnLastStatusHandler += OnLastStatus;
            ncpInterface.OnLowpanStateChanged += OnStateChanged;
                             
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

          //  NetworkingInterface.SetupInterface(ncpInterface);
      
            while (true)
            {
                Console.Write(">");
                string line = Console.ReadLine().Trim().ToLower();

                if (line == "quit" || line == "exit") 
                {
                    isUdpListenerRunning = false;

                    if (threadUdpListening != null)
                    {
                        if (threadUdpListening.IsAlive)
                        {
                            threadUdpListening.Interrupt();
                        }

                        threadUdpListening= null;
                    }
                    
                    return;
                }
                
                string cmdName = line.Split(' ').First();
                string[] cmdArgs = line.Split(' ').Skip(1).ToArray();

                switch (cmdName)
                {
                    case "help":
                        PrintHelp(cmdArgs);
                        break;

                    case "reset":
                        ncpInterface.Reset();
                        break;

                    case "form":
                        DoForm(cmdArgs);
                        break;

                    case "attach":
                        DoJoin(cmdArgs,false);
                        break;

                    case "join":
                        DoJoin(cmdArgs,true);
                        break;

                    case "ping":
                        DoPing(cmdArgs);
                        break;

                    case "status":
                        Console.WriteLine(ncpInterface.LastStatus.ToString());
                        break;

                    case "protocol":
                        Console.WriteLine(ncpInterface.ProtocolVersion);
                        break;

                    case "version":
                        Console.WriteLine(ncpInterface.NcpVersion);
                        break;

                    case "interface":
                        Console.WriteLine(ncpInterface.InterfaceType.ToString());
                        break;

                    case "vendor":
                        Console.WriteLine(ncpInterface.Vendor);
                        break;

                    case "connected":
                        Console.WriteLine(ncpInterface.Connected.ToString());
                        break;

                    case "caps":
                        DoCaps();
                        break;

                    case "networkname":
                        DoNetworkName(cmdArgs);
                        break;

                    case "scan":
                        DoScan(cmdArgs);                                               
                        break;
              
                    case "bufferinfo":
                        DoBufferInfo();
                        break;

                    case "channel":
                        DoChannel(cmdArgs);
                        break;

                    case "ipaddr":
                        DoIPpaddr(cmdArgs);
                        break;

                    case "ifconfig":
                        DoIfConfig(cmdArgs);
                        break;

                    case "thread":
                        DoThread(cmdArgs);                      
                        break;

                    case "panid":
                        DoPanId(cmdArgs);
                        break;

                    case "extpanid":
                        DoXpanId(cmdArgs);
                        break;

                    case "state":
                        DoState(cmdArgs);
                        break;

                    case "masterkey":
                        DoMasterkey(cmdArgs);
                        break;
                    case "partition":
                        Console.WriteLine(ncpInterface.PartitionId.ToString());
                        break;
                    case "extaddr":
                        Console.WriteLine(ncpInterface.ExtendedAddress.ToString());
                        break;
                    case "macaddr":
                        Console.WriteLine(ncpInterface.HardwareAddress.ToString());
                        break;
                    case "udp":
                        DoUdp(cmdArgs);
                        break;


                }
            }
        }

        private static void DoUdp(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }
            else if (args[0].ToLower() == "open")
            {
                if (isUdpOpen)
                {
                    Console.WriteLine("Udp already opened");                    
                }
                else
                {
                    isUdpOpen = true;
                }
            }
            else if (args[0].ToLower() == "close")
            {
                if (isUdpOpen)
                {
                    //stop listener threads 
                }
                else
                {
                    isUdpOpen = false;
                }
            }
            else if (args[0].ToLower() == "bind")
            {
                //because ipv6 string parsing algorithm is complicated, for now and just in console application using .Net class for it
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(args[1]);

                IPv6Address ipv6Address = new IPv6Address(ip.GetAddressBytes());                             
                ushort port = Convert.ToUInt16( args[2]);
           
                threadUdpListening = new Thread(()=>UDPListenerThread(ipv6Address,port));
                threadUdpListening.Start();
            }
            else if (args[0].ToLower() == "send" && args.Length==4)
            {
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(args[1]);
                IPv6Address ipv6Address = new IPv6Address(ip.GetAddressBytes());

                ushort port = Convert.ToUInt16(args[2]);
                
                byte[] data = Encoding.UTF8.GetBytes(args[3]);

                UdpSocket udpClient = new UdpSocket();
                udpClient.Connect(ipv6Address, port);
                udpClient.Send(data, data.Length);
                udpClient.Close();
            }
        }
            
        private static void UDPListenerThread(IPv6Address ipAddress, ushort port)
        {
            UdpSocket receiver = new UdpSocket();
            receiver.Bind(ipAddress, port);
            IPv6EndPoint remoteIp = null;

            isUdpListenerRunning = true;

            while (isUdpListenerRunning)
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

            receiver.Close();
            receiver = null;
        }

        private static void DoForm(string[] args)
        {
            if (args.Length < 2) return;

            string networkname;
            byte channel;
            string masterkey=string.Empty;
            ushort panid=0xFFFF;

            networkname = args[0];

            if (Utilities.IsNumeric(args[1]))
            {
                channel = Convert.ToByte(args[1]);
            }
            else
            {
                return;
            }

            if(args.Length==3)
            {
                masterkey = args[2];
            }
            else if (args.Length >= 4)
            {
                masterkey = args[2];
                panid = Convert.ToUInt16(args[3]);
            }
          
            ncpInterface.Form(networkname, channel, masterkey, panid);
        }

        private static void DoJoin(string[] args, bool requireExistingPeers = false)
        {
            if (args.Length != 5) return;

            string networkname;
            byte channel;
            string masterkey = string.Empty;
            string xpanid = string.Empty;
            ushort panid = 0xFFFF;

            networkname = args[0];

            if (Utilities.IsNumeric(args[1]))
            {
                channel = Convert.ToByte(args[1]);
            }
            else
            {
                return;
            }

            masterkey = args[2];
            xpanid= args[3];
            panid = Convert.ToUInt16(args[4]);

            if (requireExistingPeers)
            {
                ncpInterface.Join(networkname, channel, masterkey, xpanid,  panid);
            }
            else
            {
                ncpInterface.Attach(networkname, channel, masterkey, xpanid, panid);
            }          
        }

        private static void DoPing(string[] args)
        {
            if (args.Length == 1)
            {
                IPv6Address iPAddress = new IPv6Address( System.Net.IPAddress.Parse(args[0]).GetAddressBytes());                
                short replyTime= Icmpv6.SendEchoRequest(iPAddress);

                if (replyTime > -1)
                {
                    Console.WriteLine("8 bytes from {0}: icmp_seq=0 hlim=0 time={1}ms", iPAddress.ToString(), replyTime);
                }               
            }            
        }
            
        private static void DoScan(string[] args)
        {
            if (args.Length == 0)
            {
              
                ncpInterface.ScanMask = ncpInterface.SupportedChannels;
                var scanResult = ncpInterface.ScanBeacon();

                Console.WriteLine("| J | Network Name     | Extended PAN     | PAN  | MAC Address      | Ch | dBm | LQI |");
                Console.WriteLine("+---+------------------+------------------+------+------------------+----+-----+-----+");

                if (scanResult != null)
                {
                   foreach (var beacon in scanResult)
                    {
                        Console.Write("| " + beacon.IsJoiningPermitted.ToString().Substring(0,1).PadRight(2, ' '));
                        Console.Write("| " + beacon.NetworkName.PadRight(17, ' '));
                        Console.Write("| " + BitConverter.ToString((byte[])beacon.XpanId).Replace("-", string.Empty).PadRight(17, ' '));
                        Console.Write("| " + beacon.PanId.ToString().PadRight(5, ' '));
                        Console.Write("| " + beacon.HardwareAddress.ToString().PadRight(17, ' '));
                        Console.Write("| " + beacon.Channel.ToString().PadRight(3, ' '));
                        Console.Write("| " + beacon.Rssi.ToString().PadRight(4, ' '));
                        Console.Write("| " + beacon.LQI.ToString().PadRight(4, ' '));
                        Console.Write("|");

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
            }
            else if (args[0].ToLower() == "energy")
            {
                var scanResult = ncpInterface.ScanEnergy();

                if (scanResult != null)
                {
                    foreach (var channelInfo in scanResult)
                    {
                        Console.WriteLine("Channel : {0}, RSSI : {1}", channelInfo.Channel, channelInfo.Rssi);
                    }

                    Console.WriteLine();
                }

            }
            else if (Utilities.IsNumeric(args[0]))
            {
                Console.WriteLine("| J | Network Name     | Extended PAN     | PAN  | MAC Address      | Ch | dBm | LQI |");
                Console.WriteLine("+---+------------------+------------------+------+------------------+----+-----+-----+");

                ncpInterface.ScanMask = new byte[] { Convert.ToByte(args[0]) };
                var scanResult = ncpInterface.ScanBeacon();

                if (scanResult != null)
                {
                    foreach (var beacon in scanResult)
                    {
                        Console.Write("| ?".PadRight(4, ' '));
                        Console.Write("| " + beacon.NetworkName.PadRight(17, ' '));
                        Console.Write("| " + BitConverter.ToString((byte[])beacon.XpanId).Replace("-", string.Empty).PadRight(17, ' '));
                        Console.Write("| " + beacon.PanId.ToString().PadRight(5, ' '));
                        Console.Write("| " + beacon.HardwareAddress.ToString().PadRight(17, ' '));
                        Console.Write("| " + beacon.Channel.ToString().PadRight(3, ' '));
                        Console.Write("| " + beacon.Rssi.ToString().PadRight(4, ' '));
                        Console.Write("| " + beacon.LQI.ToString().PadRight(4, ' '));
                        Console.Write("|");

                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }
            }         
        }

        private static void DoBufferInfo()
        {
            LowpanCounters lowpanCounters = ncpInterface.GetCounters();

            var bufferInfo = lowpanCounters.BufferCountersInfo;

            Console.WriteLine();
            Console.WriteLine(string.Format("TotalBuffers: {0}", bufferInfo.TotalBuffers));           
            Console.WriteLine(string.Format("FreeBuffers: {0}", bufferInfo.FreeBuffers));
            Console.WriteLine(string.Format("LowpanSendMessages: {0}", bufferInfo.LowpanSendMessages));
            Console.WriteLine(string.Format("LowpanSendBuffers: {0}", bufferInfo.LowpanSendBuffers));
            Console.WriteLine(string.Format("LowpanReassemblyMessages: {0}", bufferInfo.LowpanReassemblyMessages));
            Console.WriteLine(string.Format("LowpanReassemblyBuffers: {0}", bufferInfo.LowpanReassemblyBuffers));
            Console.WriteLine(string.Format("Ip6Messages: {0}", bufferInfo.Ip6Messages));
            Console.WriteLine(string.Format("Ip6Buffers: {0}", bufferInfo.Ip6Buffers));
            Console.WriteLine(string.Format("MplMessages: {0}", bufferInfo.MplMessages));
            Console.WriteLine(string.Format("MplBuffers: {0}", bufferInfo.MplBuffers));
            Console.WriteLine(string.Format("MleMessages: {0}", bufferInfo.MleMessages));
            Console.WriteLine(string.Format("MleBuffers: {0}", bufferInfo.MleBuffers));
            Console.WriteLine(string.Format("ArpMessages: {0}", bufferInfo.ArpMessages));
            Console.WriteLine(string.Format("ArpBuffers: {0}", bufferInfo.ArpBuffers));
            Console.WriteLine(string.Format("CoapMessages: {0}", bufferInfo.CoapMessages));
            Console.WriteLine(string.Format("CoapBuffers: {0}", bufferInfo.CoapBuffers));
            Console.WriteLine();
        }

        private static void DoCaps()
        {
            Capabilities[] caps = ncpInterface.Capabilities;

            foreach (var capability in caps)
            {               
                Console.WriteLine(capability.ToString());
            }
        }

        private static void DoNetworkName(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(ncpInterface.LowpanIdentity.NetworkName);
            }
            else
            {
                ncpInterface.LowpanIdentity.NetworkName = args[0];
            }
        }

        private static void DoChannel(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(ncpInterface.LowpanIdentity.Channel);
            }
            else if (args[0].ToLower() == "list")
            {
                foreach (var channel in ncpInterface.SupportedChannels)
                {
                    Console.Write(channel.ToString() + ' ');
                }

                Console.WriteLine();
            }
            else if (Utilities.IsNumeric(args[0]))
            {
                ncpInterface.LowpanIdentity.Channel = Convert.ToByte(args[0]);
            }
        }

        private static void DoIPpaddr(string[] args)
        {
            if (args.Length == 0)
            {
                IPv6Address[] ipaddresses = ncpInterface.IPAddresses;

                if (ipaddresses == null)
                {
                    return;
                }

                foreach (IPv6Address ip in ipaddresses)
                {
                    Console.WriteLine(ip.ToString());
                }
            }
            else if (args[0].ToLower() == "list")
            {
                            
            }
            else if (Utilities.IsNumeric(args[0]))
            {
             
            }
        }

        private static void DoIfConfig(string[] args)
        {
            if (args.Length == 0)
            {
                bool ifState = ncpInterface.NetworkInterfaceState;

                if (ifState == true)
                {
                    Console.WriteLine("up");
                }
                else if (ifState == false)
                {
                    Console.WriteLine("down");
                }
                else
                {
                    Console.WriteLine("error.");
                }

            }
            else if (args[0].ToLower() == "up")
            {
                ncpInterface.NetworkInterfaceUp();
            }
            else if (args[0].ToLower() == "down")
            {
                ncpInterface.NetworkInterfaceDown();
            }
        }

        private static void DoThread(string[] args)
        {

            if (args.Length == 0)
            {

                bool threadState = ncpInterface.ThreadStackState;

                if (threadState == true)
                {
                    Console.WriteLine("up");
                }
                else if (threadState == false)
                {
                    Console.WriteLine("down");
                }

            }
            else if (args[0].ToLower() == "start")
            {
                ncpInterface.ThreadUp();
                Thread.Sleep(3000);
               // Console.WriteLine();
            }
            else if (args[0].ToLower() == "stop")
            {
                ncpInterface.ThreadDown();
                Thread.Sleep(3000);
            }
        }

        private static void DoPanId(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("{0:X4}", ncpInterface.LowpanIdentity.Panid);
            }
            else
            {
                ncpInterface.LowpanIdentity.Panid = Convert.ToUInt16(args[0]);
            }
        }

        private static void DoXpanId(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(BitConverter.ToString((byte[])ncpInterface.LowpanIdentity.Xpanid).Replace("-", string.Empty));
            }
            else
            {
                byte[] xpanid = Utilities.HexToBytes(args[0]);
                if (xpanid.Length != 8)
                {
                    return;
                }

                ncpInterface.LowpanIdentity.Xpanid = xpanid;
            }
        }

        private static void DoState(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(ncpInterface.State.ToString());
            }
            else if (args[0].ToLower() == "detached")
            {             
                ncpInterface.State = State.Detached;
            }
            else if (args[0].ToLower() == "child")
            {
                ncpInterface.State = State.Child;
            }
            else if (args[0].ToLower() == "router")
            {
                ncpInterface.State = State.Router;
            }
            else if (args[0].ToLower() == "leader")
            {
                ncpInterface.State = State.Leader;
            }
            else
            {
                Console.WriteLine("error.");
            }
        }

        private static void DoMasterkey(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(BitConverter.ToString((byte[])ncpInterface.LowpanCredential.MasterKey).Replace("-", string.Empty));
            }
            else
            {
                byte[] masterkey = Utilities.HexToBytes(args[0]);
                if (masterkey.Length != 16)
                {
                    return;
                }

                ncpInterface.LowpanCredential.MasterKey = masterkey;
            }
        }

        private static void PrintHelp(string[] args)
        {
            //to do, add protocol
            if (args.Length == 0)
            {
                string helpstring = @"
            
available commands(type help < name > for more information):
============================================================
status          channel         bufferinfo          extaddr
reset           extpanid        help                ifconfig
ipaddr          networkname     scan                panid
state            thread          ping                version              
masterkey       udp             quit                exit              
protocol        interface       vendor              connected
caps            partition       macaddr             form
attach          join
";
                Console.WriteLine(helpstring);
            }
            else
            {
                string helpcommand = args[0];
                string helptext = "";
                switch (helpcommand)
                {
                    case "version":
                        helptext = @"version
                      
Print the build version information.

> version
OPENTHREAD / gf4f2f04; Jul  1 2016 17:00:09
Done";

                        break;
                    case "networkname":
                        helptext = @" networkname
       
Get the Thread Network Name.

> networkname
OpenThread
Done

networkname<name>

Set the Thread Network Name.

> networkname OpenThread
Done";
                        break;


                }

                Console.WriteLine(helptext);
            }

        }
    }
}
