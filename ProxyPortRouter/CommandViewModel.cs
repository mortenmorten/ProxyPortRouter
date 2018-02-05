﻿using System.Windows.Input;
using Prism.Commands;

namespace ProxyPortRouter
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