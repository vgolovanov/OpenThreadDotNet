using OpenThreadDotNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenThreadDotNet.Networking.Lowpan
{
    public class LowpanCredential
    {
        private WpanApi wpanApi;
        private byte[] masterKey;

        public byte[] MasterKey
        {
            get
            {
                return masterKey;
            }

            set
            {
                if (value != masterKey)
                {
                    if (wpanApi.DoMasterkey(value))
                    {
                        masterKey = value;
                    }
                }              
            }
        }

        public LowpanCredential(WpanApi wpanApi)
        {
            this.wpanApi = wpanApi;
            masterKey = wpanApi.DoMasterkey();
        }
    }
}
