using OpenThreadDotNet.Networking.IPv6;
using OpenThreadDotNet.Networking.Lowpan;
using OpenThreadDotNet.Networking.Sockets;
using System.Collections;

namespace OpenThreadDotNet.Networking
{
    public static class NetworkingInterface
    {
        private static ushort currenEphemeraltPort = 0xC000;
        private static ILowpanInterface lowpanInterface;
        private static Hashtable udpClients = new Hashtable();
        internal const byte MaxSimultaneousSockets = 8;

        public static IPv6Address IPAddress { get; set; }        
        public static UdpSocket[] listeners = new UdpSocket[MaxSimultaneousSockets];
        
        public static void SetupInterface(ILowpanInterface Interface)
        {
            lowpanInterface = Interface;
            IPAddress = lowpanInterface.IPLinkLocal;
            lowpanInterface.OnPacketReceived += IPv6PacketHandler;
            lowpanInterface.OnIpChanged += OnIpChanged;
        }

        public static void Send(byte[] data)
        {
            lowpanInterface.Send(data);
        }

        public static void SendAndWait(byte[] data)
        {
            lowpanInterface.SendAndWait(data);
        }
                    
        private static void OnIpChanged()
        {
            IPAddress = lowpanInterface.IPLinkLocal;
        }
            
        private static void IPv6PacketHandler(object sender, byte[] frame)
        {
            IPv6Packet ipv6Packet = new IPv6Packet();
            ipv6Packet.FromBytes(frame);

            if (ipv6Packet.NextHeader == IPv6Protocol.ICMPv6)
            {               
                Icmpv6.PacketHandler(ipv6Packet);               
            }
            else if (ipv6Packet.NextHeader == IPv6Protocol.Udp)
            {
                var udpDatagram = ipv6Packet.Payload as UdpDatagram;

                var udpClient = udpClients[udpDatagram.DestinationPort] as UdpSocket;
                
                if (udpClient == null) return;
             
                udpClient.PacketHandler(ipv6Packet);
            }
        }

        internal static void CreateSocket(UdpSocket udpClient)
        {
            var client = udpClients[udpClient.sourcePort] as UdpSocket;
            
            if(client!=null && client.sourceIpAddress == udpClient.sourceIpAddress){

                throw new SocketsException("Port and ip adress already in use.");                
            }

            udpClients.Add(udpClient.sourcePort, udpClient);
        }

        internal static void CloseSocket(UdpSocket udpClient)
        {
            var client = udpClients[udpClient.sourcePort] as UdpSocket;

            client.Dispose();

            udpClients.Remove(client.sourcePort);
        }

        internal static ushort GetEphemeralPort()
        {
            currenEphemeraltPort++;
            return currenEphemeraltPort;
        }
    }
}
