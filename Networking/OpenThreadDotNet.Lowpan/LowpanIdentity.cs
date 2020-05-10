using OpenThreadDotNet;
using System;
using System.Text;

namespace OpenThreadDotNet.Networking.Lowpan
{
    public class LowpanIdentity
    {                      
        private WpanApi wpanApi;
        private string networkName;
        private ushort panid;
        private byte channel;
        private byte[] xpanid;

        public string NetworkName
        {
            get
            {
                return networkName;              
            }
            set
            {
                if (value != networkName)
                {
                    if(wpanApi.DoNetworkName(value))
                    {
                        networkName = value;
                    }                   
                }                          
            }
        }

        public ushort Panid
        {
            get
            {
                return panid;
            }
            set
            {
                if (value != panid)
                {
                    if (wpanApi.DoPanId(value))
                    {
                        panid = value;
                    }
                }              
            }
        }

        public byte Channel
        {
            get
            {
                return channel;
            }
            set
            {
                if (value != channel)
                {
                    if (wpanApi.DoChannel(value))
                    {
                        channel = value;
                    }
                }
            }
        }

        public byte[] Xpanid
        {
            get
            {
                return xpanid;
            }
            set
            {
                if (value != xpanid)
                {
                    if (wpanApi.DoXpanId(value))
                    {
                        xpanid = value;
                    }
                }              
            }
        }

        public LowpanIdentity(WpanApi wpanApi)
        {
            this.wpanApi = wpanApi;
            this.networkName=wpanApi.DoNetworkName();
            this.panid= wpanApi.DoPanId();
            this.channel= wpanApi.DoChannel();
            this.xpanid= wpanApi.DoXpanId();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Name:").Append(NetworkName);

            //if (mType.Length > 0)
            //{
            //    sb.Append(", Type:").Append(mType);
            //}

            //if (mXpanid.Length > 0)
            //{
            //    sb.Append(", XPANID:").Append(HexDump.toHexString(mXpanid));
            //}

            //if (mPanid != UNSPECIFIED_PANID)
            //{
            //    sb.Append(", PANID:").Append(string.Format("0x{0:X4}", mPanid));
            //}

            //if (mChannel != UNSPECIFIED_CHANNEL)
            //{
            //    sb.Append(", Channel:").Append(mChannel);
            //}

            return sb.ToString();
        }
    }
}
