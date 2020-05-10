using System;
using OpenThreadDotNet.Networking.Sockets;


namespace OpenThreadDotNet.Networking.IPv6
{
    public class IPv6PseudoHeader
    {      
        private byte[] pseudoHeader;

        public IPv6PseudoHeader(byte[] ipv6Packet)
        {
            pseudoHeader = new byte[40]; // Size of IPv6 header
            Array.Copy(ipv6Packet, 8, pseudoHeader, 0, 16); // source ip address            
            Array.Copy(ipv6Packet, 24, pseudoHeader, 16, 16); // destination ip address
            Array.Copy(ipv6Packet, 4, pseudoHeader, 32, 2); // header length

            pseudoHeader[36] = 0;  // Reserved 3 zeros
            pseudoHeader[37] = 0;
            pseudoHeader[38] = 0;

            pseudoHeader[39] = ipv6Packet[6]; //NextHeader
        }

        public IPv6PseudoHeader(IPv6Address sourceAddress, IPv6Address destinationAddress, ushort headerLength, byte nextHeader)
        {
            pseudoHeader = new byte[40]; // Size of IPv6 header
            Array.Copy(sourceAddress.GetAddressBytes(), 0, pseudoHeader, 0, 16);
            Array.Copy(destinationAddress.GetAddressBytes(), 0, pseudoHeader, 16, 16);
            Array.Copy(NetUtilities.FromLittleEndian(headerLength), 0, pseudoHeader, 32, 2);

            pseudoHeader[36] = 0;  // Reserved 3 zeros
            pseudoHeader[37] = 0;
            pseudoHeader[38] = 0;

            pseudoHeader[39] = nextHeader; //NextHeader
        }
           
        public ushort GetCheckSum()
        {          
            if (pseudoHeader != null)
            {
                return NetUtilities.ComputeChecksum(0, pseudoHeader, false);
            }

            throw new InvalidOperationException("Empty packet array.");
        }
    }
}
