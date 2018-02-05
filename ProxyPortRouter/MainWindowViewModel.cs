using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Prism.Mvvm;

namespace ProxyPortRouter
{
    public class MainWindowViewModel
        : BindableBase
    {
        private ObservableCollection<CommandViewModel> commandEntries = new ObservableCollection<CommandViewModel>();
        private readonly PortProxyManager proxyManager;
        private readonly IPortProxyController proxyController;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        : this(serviceProvider.GetService<IConfig>(), serviceProvider.GetService<IPortProxyManager>(), serviceProvider.GetService<IPortProxyController>())
        { }

        private MainWindowViewModel(IConfig config, IPortProxyManager proxyManager, IPortProxyController proxyController)
        {
            this.proxyManager = (PortProxyManager)proxyManager;
            this.proxyManager.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(PortProxyManager.ConnectAddress):
                        RaisePropertyChanged(nameof(CurrentEntry));
                        RefreshActiveEntry();
                        break;
                    case nameof(PortProxyManager.ListenAddress):
                        RaisePropertyChanged(nameof(ListenAddress));
                        break;
                }
            };

            this.proxyController = proxyController;

            SetConfig(config);
        }

        public ObservableCollection<CommandViewModel> CommandEntries
        {
            get => commandEntries;
            set => SetProperty(ref commandEntries, value);
        }

        public EntryViewModel CurrentEntry => GetCurrentEntry();

        public string ListenAddress => proxyManager.ListenAddress;

        private void SetConfig(IConfig config)
        {
            CommandEntries.Clear();
            proxyManager.ListenAddress = config.ListenAddress;
            config.Entries?.ForEach(entry => CommandEntries.Add(new CommandViewModel(entry, proxyController)));
            proxyManager.RefreshCurrentConnectAddress();
        }

        private EntryViewModel GetCurrentEntry()
        {
            var currentAddress = proxyManager.ConnectAddress;
            return CommandEntries.FirstOrDefault(entry => entry.Address == currentAddress) ?? new EntryViewModel(
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
    }
}
