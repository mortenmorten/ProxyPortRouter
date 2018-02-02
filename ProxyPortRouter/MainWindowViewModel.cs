using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace ProxyPortRouter
{
    public class MainWindowViewModel
        : BindableBase
    {
        private ObservableCollection<CommandViewModel> commandEntries = new ObservableCollection<CommandViewModel>();
        private readonly PortProxyManager proxyManager = new PortProxyManager();

        public MainWindowViewModel()
        {
            proxyManager.PropertyChanged += (sender, args) =>
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
        }

        public ObservableCollection<CommandViewModel> CommandEntries
        {
            get => commandEntries;
            set => SetProperty(ref commandEntries, value);
        }

        public EntryViewModel CurrentEntry => GetCurrentEntry();

        public string ListenAddress => proxyManager.ListenAddress;

        public void Load(string filename)
        {
            CommandEntries.Clear();
            var settings = JsonSerializer<CommandEntries>.Deserialize(filename);
            proxyManager.ListenAddress = settings.ListenAddress;
            settings.Entries?.ForEach(entry => CommandEntries.Add(new CommandViewModel(entry, proxyManager)));
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
