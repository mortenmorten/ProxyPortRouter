namespace ProxyPortRouter.UI
{
    using System.Windows.Input;
    using Prism.Commands;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.Core.Config;

    public class CommandViewModel : EntryViewModel
    {
        private readonly IBackend backend;

        public CommandViewModel(CommandEntry model, IBackend backend)
            : base(model)
        {
            this.backend = backend;
        }

        public ICommand ExecuteCommand =>
            new DelegateCommand(() => backend?.SetCurrent(Model.Name));
    }
}