
namespace OpenThreadDotNet.Networking.Lowpan
{
    public class LowpanCounters
    {
        public class BufferCounters
        {
            public ushort TotalBuffers { get; internal set; }
            public ushort FreeBuffers { get; internal set; }
            public ushort LowpanSendMessages { get; internal set; }
            public ushort LowpanSendBuffers { get; internal set; }
            public ushort LowpanReassemblyMessages { get; internal set; }
            public ushort LowpanReassemblyBuffers { get; internal set; }
            public ushort Ip6Messages { get; internal set; }
            public ushort Ip6Buffers { get; internal set; }
            public ushort MplMessages { get; internal set; }
            public ushort MplBuffers { get; internal set; }
            public ushort MleMessages { get; internal set; }
            public ushort MleBuffers { get; internal set; }
            public ushort ArpMessages { get; internal set; }
            public ushort ArpBuffers { get; internal set; }
            public ushort CoapMessages { get; internal set; }
            public ushort CoapBuffers { get; internal set; }
        }
      
        public uint TransmissionsTotalPackets { get; } //SPINEL_PROP_CNTR_TX_PKT_TOTAL 
        public uint TransmissionsWithAck { get; } //SPINEL_PROP_CNTR_TX_PKT_ACK_REQ 
        public uint TransmissionsAcked { get; } //SPINEL_PROP_CNTR_TX_PKT_ACKED 
        public uint TransmissionsNoAck { get; } //SPINEL_PROP_CNTR_TX_PKT_NO_ACK_REQ        
        public uint TxPacketaData { get; } //SPINEL_PROP_CNTR_TX_PKT_DATA 

        public BufferCounters BufferCountersInfo
        {
            get 
            { 
                ushort[] bufferCountersInfo = wpanApi.DoCountersMessageBuffer();
                BufferCounters bufferCounters = new BufferCounters();

                bufferCounters.TotalBuffers = bufferCountersInfo[0];
                bufferCounters.FreeBuffers = bufferCountersInfo[1];
                bufferCounters.LowpanSendMessages = bufferCountersInfo[2];
                bufferCounters.LowpanSendBuffers = bufferCountersInfo[3];
                bufferCounters.LowpanReassemblyMessages = bufferCountersInfo[4];
                bufferCounters.LowpanReassemblyBuffers = bufferCountersInfo[5];
                bufferCounters.Ip6Messages = bufferCountersInfo[6];
                bufferCounters.Ip6Buffers = bufferCountersInfo[7];
                bufferCounters.MplMessages = bufferCountersInfo[8];
                bufferCounters.MplBuffers = bufferCountersInfo[9];
                bufferCounters.MleMessages = bufferCountersInfo[10];
                bufferCounters.MleBuffers = bufferCountersInfo[11];
                bufferCounters.ArpMessages = bufferCountersInfo[12];
                bufferCounters.ArpBuffers = bufferCountersInfo[13];
                bufferCounters.CoapMessages = bufferCountersInfo[14];
                bufferCounters.CoapBuffers = bufferCountersInfo[15];

                return bufferCounters;
            }
        }

        private WpanApi wpanApi;

        public LowpanCounters(WpanApi wpanApi)
        {
            this.wpanApi = wpanApi;                       
        }

        public void Reset() 
        {
            wpanApi.DoCountersReset();
        }
    }
}
