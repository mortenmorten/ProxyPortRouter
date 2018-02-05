using Prism.Mvvm;

namespace ProxyPortRouter
{
    public class PortProxyManager : BindableBase, IConnectAddressSetter, IPortProxyManager
    {
        private string listenAddress;
        private string connectAddress;

        public string ListenAddress
        {
            get => listenAddress;
            set => SetProperty(ref listenAddress, value);
        }

        public string ConnectAddress
        {
            get => connectAddress;
            set => SetProperty(ref connectAddress, value);
        }

        public void RefreshCurrentConnectAddress()
        {
            var parser = new CommandResultParser { ListenAddress = ListenAddress };
            ConnectAddress = parser.GetCurrentProxyAddress(ProcessRunner.Run(NetshCommandFactory.Executable,
                NetshCommandFactory.GetShowCommandArguments()));
        }

        public void SetConnectAddress(string address)
        {
            ProcessRunner.Run(NetshCommandFactory.Executable, string.IsNullOrEmpty(address)
                ? NetshCommandFactory.GetDeleteCommandArguments(listenAddress)
                : NetshCommandFactory.GetAddCommandArguments(listenAddress, address));
            RefreshCurrentConnectAddress();
        }
    }
}
