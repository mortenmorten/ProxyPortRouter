namespace ProxyPortRouter
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Prism.Mvvm;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;
    using ProxyPortRouter.UI;

    public class MainWindowViewModel
        : BindableBase
    {
        private readonly PortProxyManager proxyManager;
        private readonly IPortProxyController proxyController;
        private readonly IServiceProvider serviceProvider;
        private readonly ISlaveClient syncClient;
        private ObservableCollection<CommandViewModel> commandEntries = new ObservableCollection<CommandViewModel>();
        private EntryViewModel currentEntry;

        public MainWindowViewModel(IServiceProvider serviceProvider)
            : this(
                serviceProvider.GetService<ISettings>(),
                serviceProvider.GetService<IPortProxyManager>(),
                serviceProvider.GetService<IPortProxyController>(),
                serviceProvider.GetService<ISlaveClient>())
        {
            this.serviceProvider = serviceProvider;
        }

        private MainWindowViewModel(ISettings config, IPortProxyManager proxyManager, IPortProxyController proxyController, ISlaveClient syncClient)
        {
            this.proxyManager = (PortProxyManager)proxyManager;
            this.proxyManager.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(PortProxyManager.ConnectAddress):
                        UpdateCurrentEntry();
                        RefreshActiveEntry();
                        break;
                    case nameof(PortProxyManager.ListenAddress):
                        RaisePropertyChanged(nameof(ListenAddress));
                        break;
                }
            };

            this.proxyController = proxyController;

            SetConfig(config);

            this.syncClient = syncClient;
        }

        public ObservableCollection<CommandViewModel> CommandEntries
        {
            get => commandEntries;
            set => SetProperty(ref commandEntries, value);
        }

        public EntryViewModel CurrentEntry
        {
            get => currentEntry;

            set
            {
                if (SetProperty(ref currentEntry, value))
                {
                    UpdateSlave();
                }
            }
        }

        public string ListenAddress => proxyManager.ListenAddress;

        private void SetConfig(ISettings config)
        {
            CommandEntries.Clear();
            proxyManager.ListenAddress = config.ListenAddress;
            config.Entries?.ForEach(entry => CommandEntries.Add(new CommandViewModel(entry, proxyController)));
            proxyManager.RefreshCurrentConnectAddress();
        }

        private void UpdateCurrentEntry()
        {
            var currentAddress = proxyManager.ConnectAddress;
            CurrentEntry = CommandEntries.FirstOrDefault(entry => entry.Address == currentAddress) ?? new EntryViewModel(
                       new CommandEntry
                       {
                           Name = string.IsNullOrEmpty(currentAddress) ? "<not set>" : "<unknown>",
                           Address = currentAddress
                       });
        }

        private void RefreshActiveEntry()
        {
            foreach (var entry in CommandEntries)
            {
                entry.IsActive = CurrentEntry == entry;
            }
        }

        private void UpdateSlave()
        {
            if (syncClient == null)
            {
                return;
            }

            Task.Factory.StartNew(() => syncClient.SetCurrentEntryAsync(CurrentEntry.Name));
        }
    }
}
