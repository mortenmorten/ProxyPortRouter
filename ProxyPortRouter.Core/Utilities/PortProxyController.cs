namespace ProxyPortRouter.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;

    [UsedImplicitly]
    public class PortProxyController : IPortProxyController
    {
        private readonly ISettings config;
        private readonly IPortProxyManager proxyManager;

        public PortProxyController(ISettings config, IPortProxyManager proxyManager)
        {
            this.config = config;
            this.proxyManager = proxyManager;
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return this.config.Entries;
        }

        public CommandEntry GetCurrentEntry()
        {
            var currentAddress = this.proxyManager.ConnectAddress;
            return this.GetEntries().FirstOrDefault(entry => entry.Address == currentAddress) ?? new CommandEntry
            {
                Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                Address = currentAddress
            };
        }

        public void SetCurrentEntry(string name)
        {
            var entry = this.GetEntries().FirstOrDefault(cmdEntry =>
                string.Equals(cmdEntry.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (entry == null)
            {
                throw new InvalidOperationException($"Unknown entry '{name}'");
            }

            this.proxyManager.SetConnectAddress(entry.Address);
        }
    }
}