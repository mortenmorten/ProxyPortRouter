﻿namespace ProxyPortRouter.Core.Socket
{
    using System;

    public class CommandReceivedArgs : EventArgs
    {
        public CommandReceivedArgs(Command command)
        {
            Command = command;
        }

        public Command Command { get; }
    }
}