using System;

namespace OpenThreadDotNet.Spinel
{
    public class SpinelProtocolExceptions : Exception
    {
        public SpinelProtocolExceptions(string message)
            : base(message)
        {
        }
    }

    public class SpinelFormatException : Exception
    {
        public SpinelFormatException(string message)
            : base(message)
        {
        }
    }
}
