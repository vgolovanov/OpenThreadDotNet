using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThreadDotNet.Networking.Sockets
{
    public class SocketsException : Exception
    {
        public SocketsException(string message)
            : base(string.Format(message))
        {
        }
    }
}
