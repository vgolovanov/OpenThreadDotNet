using System;

namespace OpenThreadDotNet
{
    public static class Utilities
    {
        public static int GetUID(uint PropertyId, byte Tid)
        {
            return GetUID((int)PropertyId, Tid);
        }

        public static int GetUID(int PropertyId, byte Tid)
        {
            return ((int)Tid << 24) | (int)PropertyId;
        }

        public static byte[] HexToBytes(string HexString)
        {
            HexString = HexString.ToUpper();

            byte[] data = new byte[(HexString.Length + 1) / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(("0123456789ABCDEF".IndexOf(HexString[i * 2]) * 16) + "0123456789ABCDEF".IndexOf(HexString[(i * 2) + 1]));
            }

            return data;
        }

        public static byte[] CombineArrays(byte[] Array1, byte[] Array2)
        {
            byte[] mergedArray = new byte[Array1.Length + Array2.Length];

            Array.Copy(Array1, mergedArray, Array1.Length);
            Array.Copy(Array2, 0, mergedArray, Array1.Length, Array2.Length);

            return mergedArray;
        }

        public static byte[] CombineArrays(byte[] Array1, byte[] Array2, byte[] Array3)
        {
            byte[] mergedArray = new byte[Array1.Length + Array2.Length+Array3.Length];

            Array.Copy(Array1, mergedArray, Array1.Length);
            Array.Copy(Array2, 0, mergedArray, Array1.Length, Array2.Length);
            Array.Copy(Array3, 0, mergedArray, Array1.Length + Array2.Length, Array3.Length);

            return mergedArray;
        }

        public static bool ByteArrayCompare(byte[] a1, byte[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (a1[i] != a2[i])
                    return false;

            return true;
        }

        public static bool IsNumeric(string input)
        {
            //return int.TryParse(input, out _);

            try
            {
                int.Parse(input);
                return true;
            }
            catch (Exception)
            {
                return false;
            }                        
        }       
    }
}
