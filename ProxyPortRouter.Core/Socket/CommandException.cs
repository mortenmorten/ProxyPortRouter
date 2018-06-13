namespace ProxyPortRouter.Core.Socket
{
    using System;

    [Serializable]
    public class CommandException : Exception
    {
        public CommandException(string message)
            : base(message)
        {
        }
    }
}