using System.Collections.Generic;

namespace ProxyPortRouter.Utilities
{
    public interface IPortProxyController
    {
        IEnumerable<CommandEntry> GetEntries();

        CommandEntry GetCurrentEntry();

        void SetCurrentEntry(string name);
    }
}