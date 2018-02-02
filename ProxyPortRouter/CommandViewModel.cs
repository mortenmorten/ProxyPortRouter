using System.Windows.Input;
using Prism.Commands;

namespace ProxyPortRouter
{
    public class CommandViewModel : EntryViewModel
    {
        private readonly IConnectAddressSetter addressSetter;

        public CommandViewModel(CommandEntry model, IConnectAddressSetter addressSetter)
            : base(model)
        {
            this.addressSetter = addressSetter;
        }

        public ICommand ExecuteCommand =>
            new DelegateCommand(() => addressSetter?.SetConnectAddress(Model.Address));
    }
}