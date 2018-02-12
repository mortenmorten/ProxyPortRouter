namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    public class LocalBackend : IBackend
    {
        private readonly IPortProxyController proxyController;
        private readonly ISlaveClient slaveClient;
        private readonly ISettings settings;

        [UsedImplicitly]
        public LocalBackend(
            ISettings settings,
            IPortProxyController proxyController,
            ISlaveClient slaveClient)
        {
            this.proxyController = proxyController;
            this.slaveClient = slaveClient;
            this.settings = settings;
        }

        public event EventHandler CurrentChanged;

        public string GetListenAddress()
        {
            return settings.ListenAddress;
        }

        public void SetCurrent(string name)
        {
            var previousEntry = GetCurrent()?.Name;
            proxyController.SetCurrentEntry(name);

            if (previousEntry != null && previousEntry != GetCurrent()?.Name)
            {
                CurrentChanged?.Invoke(this, EventArgs.Empty);
            }

            UpdateSlave(name);
        }

        public CommandEntry GetCurrent()
        {
            return proxyController.GetCurrentEntry();
        }

        public IEnumerable<CommandEntry> GetEntries()
        {
            return proxyController.GetEntries();
        }

        private void UpdateSlave(string name)
        {
            if (slaveClient == null)
            {
                return;
            }

            Task.Factory.StartNew(() => slaveClient.SetCurrentEntryAsync(name));
        }
    }
}
