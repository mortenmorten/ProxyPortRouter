#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
namespace ProxyPortRouter.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Config;

    public class PortProxyController : IPortProxyControllerAsync
    {
        private readonly IProcessRunnerAsync processRunner;
        private readonly ISettings config;
        private string currentAddress;

        [UsedImplicitly]
        public PortProxyController(ISettings config, IProcessRunnerAsync processRunner)
        {
            this.config = config;
            this.processRunner = processRunner;
        }

        public Task<IEnumerable<CommandEntry>> GetEntriesAsync()
        {
            return Task.FromResult(config.Entries.AsEnumerable());
        }

        public async Task<CommandEntry> GetCurrentEntryAsync()
        {
            return (await GetEntriesAsync().ConfigureAwait(false))?.FirstOrDefault(IsCurrentEntry)
                   ?? new CommandEntry
                   {
                       Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                       Address = currentAddress
                   };
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

        public async Task RefreshCurrentConnectAddressAsync()
        {
            var parser = new CommandResultParser { ListenAddress = config.ListenAddress };
            currentAddress = parser.GetCurrentProxyAddress(
                await processRunner.RunAsync(
                    NetshCommandFactory.Executable,
                    NetshCommandFactory.GetShowCommandArguments()).ConfigureAwait(false));
        }

        private static (string, int) SplitAddressAndPort(string value)
        {
            var split = value?.Split(':');
            var address = split?.Length > 0 ? split[0] : value;
            var port = split?.Length > 1 && int.TryParse(split[1], out var iPort) ? iPort : 80;
            return (address, port);
        }

        private async Task SetConnectAddressAsync(string address)
        {
            var (hostname, port) = SplitAddressAndPort(address);
            await processRunner.RunAsync(
                NetshCommandFactory.Executable,
                string.IsNullOrEmpty(hostname) ? NetshCommandFactory.GetDeleteCommandArguments(config.ListenAddress) : NetshCommandFactory.GetAddCommandArguments(config.ListenAddress, hostname, port))
                .ConfigureAwait(false);
            await RefreshCurrentConnectAddressAsync().ConfigureAwait(false);
        }

        private bool IsCurrentEntry(CommandEntry entry)
        {
            var (entryHostname, entryPort) = SplitAddressAndPort(entry.Address);
            var (currentHostname, currentPort) = SplitAddressAndPort(currentAddress);
            return entryHostname == currentHostname && entryPort == currentPort;
        }
    }
}
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
