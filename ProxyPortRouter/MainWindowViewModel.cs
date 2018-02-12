namespace ProxyPortRouter
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Microsoft.Extensions.DependencyInjection;

    using Prism.Mvvm;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.UI;

    public class MainWindowViewModel
        : BindableBase
    {
        private readonly IBackend backend;

        private EntryViewModel currentEntry;

        public MainWindowViewModel(IServiceProvider serviceProvider)
            : this(serviceProvider.GetService<IBackend>())
        {
        }

        private MainWindowViewModel(IBackend backend)
        {
            this.backend = backend;
            backend.CurrentChanged += (s, e) => UpdateCurrentEntry();

            CommandEntries = new ObservableCollection<CommandViewModel>();
            foreach (var commandEntry in backend.GetEntries())
            {
                CommandEntries.Add(new CommandViewModel(commandEntry, backend));
            }

            UpdateCurrentEntry();
        }

        public ObservableCollection<CommandViewModel> CommandEntries { get; }

        public EntryViewModel CurrentEntry
        {
            get => currentEntry;
            private set => SetProperty(ref currentEntry, value);
        }

        public string ListenAddress => backend.GetListenAddress();

        private void UpdateCurrentEntry()
        {
            var current = backend.GetCurrent();
            CurrentEntry = CommandEntries.FirstOrDefault(entry => entry.Name == current.Name)
                           ?? new EntryViewModel(current);
            foreach (var entry in CommandEntries)
            {
                entry.IsActive = CurrentEntry == entry;
            }
        }
    }
}
