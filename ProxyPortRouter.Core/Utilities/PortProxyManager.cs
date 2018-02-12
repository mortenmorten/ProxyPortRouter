namespace ProxyPortRouter.Core.Utilities
{
    using JetBrains.Annotations;

    using Prism.Mvvm;

    [UsedImplicitly]
    public class PortProxyManager : BindableBase, IConnectAddressSetter, IPortProxyManager
    {
        private string listenAddress;
        private string connectAddress;

        public string ListenAddress
        {
            get => listenAddress;
            set => SetProperty(ref this.listenAddress, value);
        }

        public string ConnectAddress
        {
            get => connectAddress;
            set => SetProperty(ref this.connectAddress, value);
        }

        public void RefreshCurrentConnectAddress()
        {
            var parser = new CommandResultParser { ListenAddress = this.ListenAddress };
            ConnectAddress = parser.GetCurrentProxyAddress(ProcessRunner.Run(
                NetshCommandFactory.Executable,
                NetshCommandFactory.GetShowCommandArguments()));
        }

        public void SetConnectAddress(string address)
        {
            ProcessRunner.Run(
                NetshCommandFactory.Executable,
                string.IsNullOrEmpty(address) ? NetshCommandFactory.GetDeleteCommandArguments(this.listenAddress) : NetshCommandFactory.GetAddCommandArguments(this.listenAddress, address));
            RefreshCurrentConnectAddress();
        }
    }
}
