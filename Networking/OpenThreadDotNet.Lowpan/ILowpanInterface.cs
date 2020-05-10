using OpenThreadDotNet.Networking.Sockets;
using OpenThreadDotNet.Spinel;

namespace OpenThreadDotNet.Networking.Lowpan
{
    public interface ILowpanInterface
    {
        Capabilities[] Capabilities { get; }
        HardwareAddress ExtendedAddress { get; }
        InterfaceType InterfaceType { get; }
        IPv6Address[] IPAddresses { get; }
        IPv6Address IPLinkLocal { get; }
        IPv6Address IPMeshLocal { get; }
        IPv6Address[] IPMulticastAddresses { get; }
        LastStatus LastStatus { get; }
        LowpanCredential LowpanCredential { get; set; }
        LowpanIdentity LowpanIdentity { get; set; }
        string Name { get; }
        string NcpVersion { get; }
        bool NetworkInterfaceState { get; }
        uint PartitionId { get; }
        HardwareAddress HardwareAddress { get; }
        string ProtocolVersion { get; }
        State State { get; set; }
        byte[] ScanMask { get; set; }
        PowerState PowerState { get; }
        byte[] SupportedChannels { get; }
        bool ThreadStackState { get; }
        string Vendor { get; }
        bool Connected { get; }
        bool Commissioned { get; }

        event LowpanLastStatusHandler OnLastStatusHandler;       
        event LowpanRoleChanged OnLowpanStateChanged;
        event PacketReceivedEventHandler OnPacketReceived;
        event LowpanIpChanged OnIpChanged;
        
        void Form(string networkName, byte channel, string masterkey, ushort panid);
        void Attach(string networkName, byte channel, string masterkey, string xpanid, ushort panid, bool requireExistingPeers = false);
        void Join(string networkName, byte channel, string masterkey, string xpanid, ushort panid);
        void Leave();

        void EnableLowPower();

        bool NetworkInterfaceDown();
        bool NetworkInterfaceUp();
        void OnHostWake();
        void Open(IStream stream);
        void Reset();
        LowpanBeaconInfo[] ScanBeacon();
        LowpanChannelInfo[] ScanEnergy();
        bool ThreadDown();
        bool ThreadUp();
        void SendAndWait(byte[] frame);
        void Send(byte[] frame);
    }
}