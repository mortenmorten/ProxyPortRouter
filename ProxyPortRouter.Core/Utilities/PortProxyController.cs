namespace ProxyPortRouter.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;

    public class PortProxyController : IPortProxyController, IPortProxyControllerAsync
    {
        private readonly IProcessRunnerAsync processRunner;
        private readonly ISettings config;
        private string currentAddress;

        [UsedImplicitly]
        public PortProxyController(ISettings config, IProcessRunnerAsync processRunner)
        {
            this.config = config;
            this.processRunner = processRunner;
            InitializeAsync().Wait();
        }

        public Task InitializeAsync()
        {
            return RefreshCurrentConnectAddressAsync();
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return GetEntriesAsync().Result;
        }

        public Task<IEnumerable<CommandEntry>> GetEntriesAsync()
        {
            return Task.FromResult(config.Entries.AsEnumerable());
        }

        public CommandEntry GetCurrentEntry()
        {
            return GetCurrentEntryAsync().Result;
        }

        public async Task<CommandEntry> GetCurrentEntryAsync()
        {
            return (await GetEntriesAsync().ConfigureAwait(false))?.FirstOrDefault(entry => entry.Address == currentAddress)
                   ?? new CommandEntry
                          {
                              Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                              Address = currentAddress
                          };
        }

        public void SetCurrentEntry(string name)
        {
            SetCurrentEntryAsync(name).Wait();
        }

        public async Task SetCurrentEntryAsync(string name)
        {
            var entry = (await GetEntriesAsync().ConfigureAwait(false))?.FirstOrDefault(cmdEntry =>
                string.Equals(cmdEntry.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (entry == null)
            {
                throw new InvalidOperationException($"Unknown entry '{name}'");
            }

            await SetConnectAddressAsync(entry.Address).ConfigureAwait(false);
        }

        private async Task SetConnectAddressAsync(string address)
        {
            await processRunner.RunAsync(
                NetshCommandFactory.Executable,
                string.IsNullOrEmpty(address) ? NetshCommandFactory.GetDeleteCommandArguments(config.ListenAddress) : NetshCommandFactory.GetAddCommandArguments(config.ListenAddress, address))
                .ConfigureAwait(false);
            await RefreshCurrentConnectAddressAsync().ConfigureAwait(false);
        }

        private async Task RefreshCurrentConnectAddressAsync()
        {
            var parser = new CommandResultParser { ListenAddress = config.ListenAddress };
            currentAddress = parser.GetCurrentProxyAddress(
                await processRunner.RunAsync(
                    NetshCommandFactory.Executable,
                    NetshCommandFactory.GetShowCommandArguments()).ConfigureAwait(false));
        }
    }
}