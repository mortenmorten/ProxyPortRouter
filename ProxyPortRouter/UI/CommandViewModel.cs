using System.Windows.Input;
using Prism.Commands;
using ProxyPortRouter.Utilities;

namespace ProxyPortRouter.UI
{
    public class CommandViewModel : EntryViewModel
    {
        private readonly IPortProxyController proxyController;

        public CommandViewModel(CommandEntry model, IPortProxyController proxyController)
            : base(model)
        {
            this.proxyController = proxyController;
        }

        public ICommand ExecuteCommand =>
            new DelegateCommand(() => proxyController?.SetCurrentEntry(Model.Name));
    }
}