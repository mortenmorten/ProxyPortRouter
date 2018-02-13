namespace ProxyPortRouter.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProxyPortRouter.Core.Config;

    public interface IBackendAsync : IBackendEvents
    {
        Task<string> GetListenAddressAsync();

        Task<CommandEntry> GetCurrentAsync();

        Task SetCurrentAsync(string name);

        Task<IEnumerable<CommandEntry>> GetEntriesAsync();
    }
}