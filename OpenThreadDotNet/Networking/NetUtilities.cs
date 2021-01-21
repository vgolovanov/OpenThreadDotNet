using OpenThreadDotNet.Networking.Sockets;
using OpenThreadDotNet.Spinel;
using System;
using System.Collections;

namespace OpenThreadDotNet.Networking
{
    public static class NetUtilities
    {
        /// <summary>
        /// This is a simple method for computing the 16-bit one's complement
        /// checksum of a byte buffer. The byte buffer will be padded with
        /// a zero byte if an uneven number.
        /// </summary>
        /// <param name="payLoad">Byte array to compute checksum over</param>
        /// <returns></returns>
        public static ushort ComputeChecksum(uint sum, byte[] payLoad, bool inverseSum = true)
        {
            uint xsum = sum;
            ushort shortval = 0,
                    hiword = 0,
                    loword = 0;

            // Sum up the 16-bits
            for (int i = 0; i < payLoad.Length / 2; i++)
            {
                hiword = (ushort)(((ushort)payLoad[i * 2]) << 8);
                loword = (ushort)payLoad[(i * 2) + 1];

                shortval = (ushort)(hiword | loword);

                xsum = xsum + (uint)shortval;
            }
            // Pad if necessary
            if ((payLoad.Length % 2) != 0)
            {
                xsum += (uint)payLoad[payLoad.Length - 1];
            }

            xsum = ((xsum >> 16) + (xsum & 0xFFFF));
            xsum = (xsum + (xsum >> 16));

            if (inverseSum == false)
            {
                shortval = (ushort)xsum;
            }
            else
            {
                shortval = (ushort)(~xsum);
            }
            
            return shortval;
        }

        public static ushort ToLittleEndian(byte[] data)
        {
            if (BitConverter.IsLittleEndian)
            {
#if NETCORE
                Array.Reverse(data);               
#else
               ArrayExtensions.Reverse(data);
#endif
            }

            return BitConverter.ToUInt16(data, 0);
        }

        public static ushort ToLittleEndian(ushort data)
        {
            if (BitConverter.IsLittleEndian)
            {
                return (ushort)((((int)data & 0xFF) << 8) | (int)((data >> 8) & 0xFF));
            }

            return data;
        }

        public static byte[] FromLittleEndian(ushort data)
        {
            byte[] value = BitConverter.GetBytes(data);

            if (BitConverter.IsLittleEndian)
            {
#if NETCORE
                Array.Reverse(value);
#else               
                ArrayExtensions.Reverse(value);
#endif          
            }
            return value;
        }

        public static IPv6Address SpinelIPtoSystemIP(SpinelIPv6Address ipAddress)
        {
            if (ipAddress == null) return null;

            return new IPv6Address(ipAddress.bytes);
        }

        public static IPv6Address[] SpinelIPtoSystemIP(SpinelIPv6Address[] ipAddresses)
        {
            if (ipAddresses == null) return null;

            ArrayList ipAddr = new ArrayList();

            foreach (SpinelIPv6Address iPv6Address in ipAddresses)
            {
                ipAddr.Add(SpinelIPtoSystemIP(iPv6Address));
            }

            if (ipAddr.Count > 0)
            {
                return ipAddr.ToArray(typeof(IPv6Address)) as IPv6Address[];
            }

            return null;
        }
    }
}
