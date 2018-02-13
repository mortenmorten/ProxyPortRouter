namespace ProxyPortRouter.UI
{
    using System.Windows.Input;
    using Prism.Commands;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.Core.Config;

    public class CommandViewModel : EntryViewModel
    {
        private readonly IBackendAsync backend;

        public CommandViewModel(CommandEntry model, IBackendAsync backend)
            : base(model)
        {
            this.backend = backend;
        }

        public ICommand ExecuteCommand =>
            new DelegateCommand(
                async () =>
                    {
                        if (backend != null)
                        {
                            await backend.SetCurrentAsync(Model.Name).ConfigureAwait(false);
                        }
                    });
    }
}