using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace ProxyPortRouter.Utilities
{
    [UsedImplicitly]
    public class PortProxyController : IPortProxyController
    {
        private readonly IConfig config;
        private readonly IPortProxyManager proxyManager;

        public PortProxyController(IConfig config, IPortProxyManager proxyManager)
        {
            this.config = config;
            this.proxyManager = proxyManager;
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return config.Entries;
        }

        public CommandEntry GetCurrentEntry()
        {
            var currentAddress = proxyManager.ConnectAddress;
            return GetEntries().FirstOrDefault(entry => entry.Address == currentAddress) ?? new CommandEntry
            {
                Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                Address = currentAddress
            };
        }

        public void SetCurrentEntry(string name)
        {
            var entry = GetEntries().FirstOrDefault(cmdEntry =>
                string.Equals(cmdEntry.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (entry == null) throw new InvalidOperationException($"Unknown entry '{name}'");
            proxyManager.SetConnectAddress(entry.Address);
        }
    }
}