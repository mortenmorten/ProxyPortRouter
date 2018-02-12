namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;

    using ProxyPortRouter.Core.Config;

    public interface IBackend
    {
        event EventHandler CurrentChanged;

        string GetListenAddress();

        CommandEntry GetCurrent();

        void SetCurrent(string name);

        IEnumerable<CommandEntry> GetEntries();
    }
}