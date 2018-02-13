namespace ProxyPortRouter.Core
{
    using System.Collections.Generic;

    using ProxyPortRouter.Core.Config;

    public interface IBackend : IBackendEvents
    {
        string GetListenAddress();

        CommandEntry GetCurrent();

        void SetCurrent(string name);

        IEnumerable<CommandEntry> GetEntries();
    }
}