namespace OpenThreadDotNet.Networking.Lowpan
{
    public delegate void onLowpanEnabledChanged(bool value);
    public delegate void onLowpanConnectedChanged(bool value);
    public delegate void onLowpanUpChanged(bool value);
    public delegate void LowpanRoleChanged();
    public delegate void LowpanIpChanged();
    public delegate void onLowpanStateChanged(string value);
    public delegate void onLowpanIdentityChanged(LowpanIdentity value);
    public delegate void onLowpanReceiveFromCommissioner(out byte[] packet);
    public delegate void LowpanLastStatusHandler(LastStatus value);
   
    public delegate void PacketReceivedEventHandler(object sender, byte[] buffer);

    //  public delegate void NetStreamDataHandler(byte[] packet);

    // public delegate void PropertyChangedHandler(string propertyName, string propertyValue);

    //  public delegate void PropertyChangedHandler(string propertyName);
    //public delegate void onLinkNetworkAdded(@in android);
    //public delegate void onLinkNetworkRemoved(@in android);
    //public delegate void onLinkAddressAdded(string value);
    //public delegate void onLinkAddressRemoved(string value);
}
