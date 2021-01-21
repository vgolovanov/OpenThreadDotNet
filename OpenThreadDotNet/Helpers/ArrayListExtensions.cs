using System;
using System.Collections;
using System.Text;

namespace OpenThreadDotNet
{
    public static class ArrayListExtensions
    {
        public static void AddRange(this ArrayList arrayList, ICollection c)
        {
#if NETCORE
             arrayList.AddRange(c);
#else
            foreach(var value in c)
            {
                arrayList.Add(value);
            }          
#endif

        }

        //public static void RemoveRange(this ArrayList arrayList,)
        //{

        //}
    }
}
