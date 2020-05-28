////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Globalization;
using System.Text;

namespace OpenThreadDotNet.Networking.Sockets
{
    /// <devdoc>
    ///    <para>Provides an internet protocol (IP) address.</para>
    /// </devdoc>
    [Serializable]
    public class IPv6Address : Object
    {
        internal const int IPv6AddressBytes = 16;

        public static readonly IPv6Address IPv6Any = new IPv6Address(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0);
        public static readonly IPv6Address IPv6Loopback = new IPv6Address(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, 0);
        public static readonly IPv6Address IPv6None = new IPv6Address(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 0);

        private byte[] m_Numbers = new byte[IPv6AddressBytes];

        private int m_HashCode = 0;
        private long m_ScopeId = 0;

        private AddressFamily m_Family = AddressFamily.InterNetworkV6;

        /// <devdoc>
        ///    <para>
        ///       Constructor for an IPv6 Address with a specified Scope.
        ///    </para>
        /// </devdoc>
        public IPv6Address(byte[] address, long scopeid)
        {

            if (address == null)
            {
                throw new ArgumentNullException("address");
            }

            if (address.Length != IPv6AddressBytes)
            {
                throw new ArgumentException("dns_bad_ip_address");
            }

            m_Family = AddressFamily.InterNetworkV6;

            Array.Copy(address, m_Numbers, 16);

            if (scopeid < 0 || scopeid > 0x00000000FFFFFFFF)
            {
                throw new ArgumentOutOfRangeException("scopeid");
            }

            m_ScopeId = scopeid;
        }

        /// <devdoc>
        ///    <para>
        ///       Constructor for IPv4 and IPv6 Address.
        ///    </para>
        /// </devdoc>
        public IPv6Address(byte[] address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address");
            }
            if (address.Length != IPv6AddressBytes)
            {
                throw new ArgumentException("dns_bad_ip_address");
            }

            m_Family = AddressFamily.InterNetworkV6;
     
            Array.Copy(address, m_Numbers, 16);
        }

        public bool Equals(object comparandObj, bool compareScopeId)
        {
            IPv6Address addr = comparandObj as IPv6Address;

            if (addr == null) return false;

            if (m_Family != addr.m_Family)
            {
                return false;
            }

            for (int i = 0; i < IPv6AddressBytes; i++)
            {
                if (addr.m_Numbers[i] != this.m_Numbers[i])
                    return false;
            }

            if (addr.m_ScopeId == this.m_ScopeId)
                return true;
            else
                return (compareScopeId ? false : true);       
        }

        /// <devdoc>
        ///    <para>
        ///       Compares two IP addresses.
        ///    </para>
        /// </devdoc>
        public override bool Equals(object comparand)
        {
            return Equals(comparand, true);
        }

        public byte[] GetAddressBytes()
        {          
            return m_Numbers;
        }
        
        public override string ToString()
        {         
            int addressStringLength = 16;
            StringBuilder addressString = new StringBuilder(addressStringLength);

            const string numberFormat = "{0:x2}{1:x2}:{2:x2}{3:x2}:{4:x2}{5:x2}:{6:x2}{7:x2}:{8:x2}{9:x2}:{10:x2}{11:x2}:{12:x2}{13:x2}:{14:x2}{15:x2}";
            string address = String.Format(numberFormat, m_Numbers[0], m_Numbers[1], m_Numbers[2], m_Numbers[3], m_Numbers[4], m_Numbers[5], m_Numbers[6], m_Numbers[7], m_Numbers[8], m_Numbers[9], m_Numbers[10], m_Numbers[11], m_Numbers[12], m_Numbers[13], m_Numbers[14], m_Numbers[15]);
            addressString.Append(address);

            if (m_ScopeId != 0)
            {
                addressString.Append('%').Append((uint)m_ScopeId);
            }

            return addressString.ToString();
        }

      

        public override int GetHashCode()
        {
            //int hashCode = -595054056;
            //hashCode = hashCode * -1521134295 + EqualityComparer<byte[]>.Default.GetHashCode(m_Numbers);
            //hashCode = hashCode * -1521134295 + m_ScopeId.GetHashCode();
            //hashCode = hashCode * -1521134295 + m_Family.GetHashCode();
            //return hashCode;

            if (m_HashCode == 0)
            {
                const int p = 16777619;
                m_HashCode = -595054056;

                for (int i = 0; i < m_Numbers.Length; i++)
                    m_HashCode = (m_HashCode ^ m_Numbers[i]) * p;

                m_HashCode += m_HashCode << 13;
                m_HashCode ^= m_HashCode >> 7;
                m_HashCode += m_HashCode << 3;
                m_HashCode ^= m_HashCode >> 17;
                m_HashCode += m_HashCode << 5;           
            }
               
            return m_HashCode;
        }
    } 
}


