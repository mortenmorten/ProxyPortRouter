namespace ProxyPortRouter.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProxyPortRouter.Core.Config;

    public interface IBackendAsync
    {
        event EventHandler CurrentChanged;

        Task<string> GetListenAddressAsync();

        Task<CommandEntry> GetCurrentAsync();

        Task InitializeAsync();

        Task SetCurrentAsync(string name);

        Task<IEnumerable<CommandEntry>> GetEntriesAsync();
    }
}