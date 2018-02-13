namespace ProxyPortRouter.Core
{
    using System;

    public interface IBackendEvents
    {
        event EventHandler CurrentChanged;
    }
}