namespace ProxyPortRouter
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using Microsoft.Extensions.DependencyInjection;

    using Prism.Mvvm;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.UI;

    public class MainWindowViewModel
        : BindableBase
    {
        private readonly IBackendAsync backend;

        private readonly Dispatcher guiDispatcher;
        private EntryViewModel currentEntry;

        public MainWindowViewModel(IServiceProvider serviceProvider)
            : this(serviceProvider.GetService<IBackendAsync>())
        {
        }

        private MainWindowViewModel(IBackendAsync backend)
        {
            guiDispatcher = Dispatcher.CurrentDispatcher;
            this.backend = backend;
            backend.CurrentChanged += (s, e) => UpdateCurrentEntryAsync().ConfigureAwait(false);
        }

        public ObservableCollection<CommandViewModel> CommandEntries { get; } = new ObservableCollection<CommandViewModel>();

        public EntryViewModel CurrentEntry
        {
            get => currentEntry;
            private set => SetProperty(ref currentEntry, value);
        }

        public string ListenAddress => backend.GetListenAddressAsync().Result;

        public async Task InitializeAsync()
        {
            var entries = (await backend.GetEntriesAsync().ConfigureAwait(false))
                .Select(entry => new CommandViewModel(entry, backend));

            guiDispatcher.Invoke(() => CommandEntries.AddRange(entries));

            await UpdateCurrentEntryAsync().ConfigureAwait(false);
        }

        private async Task UpdateCurrentEntryAsync()
        {
            var current = await backend.GetCurrentAsync().ConfigureAwait(false);
            CurrentEntry = CommandEntries.FirstOrDefault(entry => entry.Name == current.Name)
                           ?? new EntryViewModel(current);
            foreach (var entry in CommandEntries)
            {
                entry.IsActive = CurrentEntry == entry;
            }
        }
    }
}
