using System;
using System.Collections;

namespace OpenThreadDotNet
{
    public static class ArrayExtensions
    {
       
        public static void Reverse(this Array array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
         
            Reverse(array, 0, array.Length);
        }

        public static void Reverse(Array array, int index, int length)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (index < 0  || length < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "length"), "ArgumentOutOfRange_NeedNonNegNum");
            if ((array.Length -index) < length)
                throw new ArgumentException("Argument_InvalidOffLen");

            int i = index;
            int j = index + length - 1;
            Object[] objArray = array as Object[];
            if (objArray != null)
            {
                while (i < j)
                {
                    Object temp = objArray[i];
                    objArray[i] = objArray[j];
                    objArray[j] = temp;
                    i++;
                    j--;
                }
            }
        }     
    }
}
