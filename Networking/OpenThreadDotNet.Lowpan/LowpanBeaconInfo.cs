using OpenThreadDotNet.Networking.Sockets;

namespace OpenThreadDotNet.Networking.Lowpan
{
    public class LowpanBeaconInfo
    {
        public string NetworkName { get; set; }
        public byte Channel { get; set; }
        public sbyte Rssi { get; set; }
        public HardwareAddress HardwareAddress { get; set; }
        public ushort ShortAddress { get; set; }
        public ushort PanId { get; set; }
        public byte LQI { get; set; }
        public uint Protocol { get; set; }
        public byte Flags { get; set; }
        public byte[] XpanId { get; set; }
       
        public bool IsJoiningPermitted
        {
            get 
            { 
                return (Flags & (1 << 0)) != 0;
            }
        }

        public bool IsNative
        {
            get
            {
                return (Flags & (1 << 3)) != 0;
            }
        }      
    }
}
