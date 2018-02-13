namespace ProxyPortRouter.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;

    public class PortProxyController : IPortProxyController
    {
        private readonly IProcessRunner processRunner;
        private readonly ISettings config;
        private string currentAddress;

        [UsedImplicitly]
        public PortProxyController(ISettings config, IProcessRunner processRunner)
        {
            this.config = config;
            this.processRunner = processRunner;
            RefreshCurrentConnectAddress();
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return config.Entries;
        }

        public CommandEntry GetCurrentEntry()
        {
            return GetEntries()?.FirstOrDefault(entry => entry.Address == currentAddress) ?? new CommandEntry
            {
                Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                Address = currentAddress
            };
        }

        public void SetCurrentEntry(string name)
        {
            var entry = GetEntries()?.FirstOrDefault(cmdEntry =>
                string.Equals(cmdEntry.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (entry == null)
            {
                throw new InvalidOperationException($"Unknown entry '{name}'");
            }

           SetConnectAddress(entry.Address);
        }

        private void SetConnectAddress(string address)
        {
            processRunner.Run(
                NetshCommandFactory.Executable,
                string.IsNullOrEmpty(address) ? NetshCommandFactory.GetDeleteCommandArguments(config.ListenAddress) : NetshCommandFactory.GetAddCommandArguments(config.ListenAddress, address));
            RefreshCurrentConnectAddress();
        }

        private void RefreshCurrentConnectAddress()
        {
            var parser = new CommandResultParser { ListenAddress = config.ListenAddress };
            currentAddress = parser.GetCurrentProxyAddress(processRunner.Run(
                NetshCommandFactory.Executable,
                NetshCommandFactory.GetShowCommandArguments()));
        }
    }
}