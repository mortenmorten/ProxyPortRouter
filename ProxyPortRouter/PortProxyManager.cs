namespace ProxyPortRouter
{
    using JetBrains.Annotations;

    using Prism.Mvvm;

    using ProxyPortRouter.Core.Utilities;

    [UsedImplicitly]
    public class PortProxyManager : BindableBase, IConnectAddressSetter, IPortProxyManager
    {
        private string listenAddress;
        private string connectAddress;

        public string ListenAddress
        {
            get => this.listenAddress;
            set => this.SetProperty(ref this.listenAddress, value);
        }

        public string ConnectAddress
        {
            get => this.connectAddress;
            set => this.SetProperty(ref this.connectAddress, value);
        }

        public void RefreshCurrentConnectAddress()
        {
            var parser = new CommandResultParser { ListenAddress = this.ListenAddress };
            this.ConnectAddress = parser.GetCurrentProxyAddress(ProcessRunner.Run(
                NetshCommandFactory.Executable,
                NetshCommandFactory.GetShowCommandArguments()));
        }

        public void SetConnectAddress(string address)
        {
            ProcessRunner.Run(
                NetshCommandFactory.Executable,
                string.IsNullOrEmpty(address) ? NetshCommandFactory.GetDeleteCommandArguments(this.listenAddress) : NetshCommandFactory.GetAddCommandArguments(this.listenAddress, address));
            this.RefreshCurrentConnectAddress();
        }
    }
}
