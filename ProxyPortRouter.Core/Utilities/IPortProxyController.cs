namespace ProxyPortRouter.Core.Utilities
{
    using System.Collections.Generic;

    using ProxyPortRouter.Core.Config;

    public interface IPortProxyController
    {
        IEnumerable<CommandEntry> GetEntries();

        CommandEntry GetCurrentEntry();

        void SetCurrentEntry(string name);
    }
}