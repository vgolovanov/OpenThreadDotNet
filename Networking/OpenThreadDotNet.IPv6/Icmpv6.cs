using System;
using System.Threading;
using OpenThreadDotNet.Networking.Sockets;

namespace OpenThreadDotNet.Networking.IPv6
{
    public static class Icmpv6
    {     
        private static DateTime pingStart;     
        private static bool isResponseTimeout;

        private static AutoResetEvent pingTimeout = new AutoResetEvent(false);
        private static short replyTime;
       
        public static short SendEchoRequest(IPv6Address destionationIPAddress)
        {            
            IPv6PseudoHeader pv6PseudoHeader = new IPv6PseudoHeader(NetworkingInterface.IPAddress, destionationIPAddress, 0x10, 58);
            ushort checkSum = pv6PseudoHeader.GetCheckSum();

            byte[] packet = new byte[16];
            packet[0] = 0x80;
            packet[1] = 0x00;
            packet[2] = 0x00;//Checksum
            packet[3] = 0x00;//Checksum

            packet[4] = 0x00;
            packet[5] = 0x01;
            packet[6] = 0x00;
            packet[7] = 0x0a;

            packet[8] = 0x02;
            packet[9] = 0xcd;
            packet[10] = 0x21;
            packet[11] = 0xf2;
            packet[12] = 0x00;
            packet[13] = 0x00;
            packet[14] = 0x00;
            packet[15] = 0x00;

            checkSum = NetUtilities.ComputeChecksum(checkSum, packet, true);

            Icmpv6Packet icmpv6Packet = new Icmpv6Packet();
            int packetIndex = 0;
            icmpv6Packet.FromBytes(packet, ref packetIndex);
            icmpv6Packet.Checksum = checkSum;

            IPv6Packet packetEchoRequest = new IPv6Packet();

            packetEchoRequest.DestinationAddress = destionationIPAddress;
            packetEchoRequest.SourceAddress = NetworkingInterface.IPAddress;
            packetEchoRequest.PayloadLength = 0x10;
            packetEchoRequest.NextHeader = IPv6Protocol.ICMPv6;
            packetEchoRequest.Payload = icmpv6Packet;

            pingStart = DateTime.Now;           
            isResponseTimeout = false;

            replyTime = -1;

            NetworkingInterface.SendAndWait(packetEchoRequest.ToBytes());         
            pingTimeout.WaitOne(5000);

            if (replyTime == -1) 
            {
                isResponseTimeout = true;               
            }
            
            return replyTime;
        }
      
        public static void PacketHandler(IPv6Packet ipv6Packet)
        {
            Icmpv6Packet icmpv6Handler = (Icmpv6Packet)ipv6Packet.Payload;

            if (icmpv6Handler.Icmpv6MessageType == Icmpv6MessageType.EchoRequest)
            {
                IPv6Packet packetEchoReply = new IPv6Packet();
                packetEchoReply.SourceAddress = ipv6Packet.DestinationAddress;
                packetEchoReply.DestinationAddress = ipv6Packet.SourceAddress;
                packetEchoReply.NextHeader = ipv6Packet.NextHeader;
                packetEchoReply.Flow = ipv6Packet.Flow;
                packetEchoReply.HopLimit = ipv6Packet.HopLimit;
                packetEchoReply.PayloadLength = ipv6Packet.PayloadLength;
                packetEchoReply.TrafficClass = ipv6Packet.TrafficClass;
                packetEchoReply.Version = ipv6Packet.Version;


                icmpv6Handler.Icmpv6MessageType = Icmpv6MessageType.EchoReply;
                Icmpv6EchoReply icmpv6EchoReply = new Icmpv6EchoReply((Icmpv6EchoRequest)icmpv6Handler.IcmpMessage);
                icmpv6Handler.IcmpMessage = icmpv6EchoReply;

                IPv6PseudoHeader ipv6PseudoHeader = new IPv6PseudoHeader(packetEchoReply.SourceAddress, packetEchoReply.DestinationAddress, packetEchoReply.PayloadLength, (byte)packetEchoReply.NextHeader);
                ushort checkSum = ipv6PseudoHeader.GetCheckSum();

                byte[] icmpData = icmpv6Handler.ToBytes();
                icmpData[2] = 0;
                icmpData[3] = 0;

                checkSum = NetUtilities.ComputeChecksum(checkSum, icmpData, true);

                icmpv6Handler.Checksum = checkSum;

                packetEchoReply.Payload = icmpv6Handler;

                NetworkingInterface.Send(packetEchoReply.ToBytes());
            }
            else if (icmpv6Handler.Icmpv6MessageType == Icmpv6MessageType.EchoReply)
            {
                if (isResponseTimeout)
                {                   
                    return;
                }
                
                TimeSpan elapsed = DateTime.Now - pingStart;
                replyTime = (short)elapsed.TotalMilliseconds;              
                pingTimeout.Set();
            }
        }
    }
}
