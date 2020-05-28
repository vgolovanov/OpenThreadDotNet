using System;
using System.ComponentModel;

namespace OpenThreadDotNet
{
    public static class Converter
    {
        public static bool ToBoolean(Object value)
        {
#if NETCORE
            //return Convert.ToBoolean(value);
            return (bool)value;;
#else
            return (bool)value;
#endif
        }

        public static byte ToByte(Object value)
        {
#if NETCORE
           //return Convert.ToByte(value);
             return (byte)value;
#else
            return (byte)value;
#endif
        }

        public static ushort ToUInt16(Object value)
        {
#if NETCORE
          //  return Convert.ToUInt16(value);
            return (ushort)value;
#else
            return (ushort)value;
#endif
        }

        public static int ToInt32(Object value)
        {
#if NETCORE
           // return Convert.ToInt32(value);
            return (int)value;
#else
            return (int)value;
#endif
        }

        public static uint ToUInt32(Object value)
        {
#if NETCORE
          //  return Convert.ToUInt32(value);
            return (uint)value;
#else
            return (uint)value;
#endif
        }

        public static ulong ToUInt64(Object value)
        {
#if NETCORE
           // return Convert.ToUInt64(value);
             return (ulong)value;
#else
            return (ulong)value;
#endif
        } 
    }
}
