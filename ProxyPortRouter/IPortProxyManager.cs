namespace ProxyPortRouter
{
    public interface IPortProxyManager
    {
        string ConnectAddress { get; set; }
        string ListenAddress { get; set; }

        void RefreshCurrentConnectAddress();
        void SetConnectAddress(string address);
    }
}