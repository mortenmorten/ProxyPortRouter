namespace ProxyPortRouter.UI
{
    using Prism.Mvvm;

    public class EntryViewModel : BindableBase
    {
        private bool isActive;

        public EntryViewModel(CommandEntry model)
        {
            Model = model;
        }

        public string Name
        {
            get => Model.Name;
            set
            {
                var backingName = Model.Name;
                if (SetProperty(ref backingName, value))
                {
                    Model.Name = backingName;
                }
            }
        }

        public string Address => Model.Address;

        public bool IsActive
        {
            get => isActive;
            set => SetProperty(ref isActive, value);
        }

        protected CommandEntry Model { get; }
    }
}