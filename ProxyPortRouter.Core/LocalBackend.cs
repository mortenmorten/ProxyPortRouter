namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    public class LocalBackend : IBackendAsync
    {
        private readonly IPortProxyControllerAsync proxyController;
        private readonly ISlaveClientAsync slaveClient;
        private readonly ISettings settings;

        [UsedImplicitly]
        public LocalBackend(
            ISettings settings,
            IPortProxyControllerAsync proxyController,
            ISlaveClientAsync slaveClient)
        {
            this.proxyController = proxyController;
            this.slaveClient = slaveClient;
            this.settings = settings;
        }

        public event EventHandler CurrentChanged;

        public Task<string> GetListenAddressAsync()
        {
            return Task.FromResult(settings.ListenAddress);
        }

        public Task InitializeAsync()
        {
            return proxyController.RefreshCurrentConnectAddressAsync();
        }

        public async Task SetCurrentAsync(string name)
        {
            var previousEntry = (await GetCurrentAsync().ConfigureAwait(false))?.Name;
            await proxyController.SetCurrentEntryAsync(name).ConfigureAwait(false);

            if (previousEntry != null && previousEntry != (await GetCurrentAsync().ConfigureAwait(false))?.Name)
            {
                CurrentChanged?.Invoke(this, EventArgs.Empty);
            }

            await Task.Factory.StartNew(() => UpdateSlaveAsync(name)).ConfigureAwait(true);
        }

        public Task<CommandEntry> GetCurrentAsync()
        {
            return proxyController.GetCurrentEntryAsync();
        }

        public Task<IEnumerable<CommandEntry>> GetEntriesAsync()
        {
            return proxyController.GetEntriesAsync();
        }

        private Task UpdateSlaveAsync(string name)
        {
            return slaveClient == null ? Task.CompletedTask : slaveClient.SetCurrentEntryAsync(name);
        }
    }
}
