using System;

namespace OpenThreadDotNet.Networking.Sockets
{
    public class SocketsException : Exception
    {        
        public SocketsException(string message) : base(message)
        {
        }
    }
}
