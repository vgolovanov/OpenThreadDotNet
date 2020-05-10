using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThreadDotNet.Networking.Lowpan
{
    public class LowpanChannelInfo
    {
        public byte Channel { get; set; }
        public sbyte Rssi { get; set; }
    }
}
