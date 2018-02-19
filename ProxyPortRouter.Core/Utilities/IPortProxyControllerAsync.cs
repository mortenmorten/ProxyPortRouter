namespace ProxyPortRouter.Core.Utilities
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ProxyPortRouter.Core.Config;

    public interface IPortProxyControllerAsync
    {
        Task<IEnumerable<CommandEntry>> GetEntriesAsync();

        Task<CommandEntry> GetCurrentEntryAsync();

        Task SetCurrentEntryAsync(string name);

        Task RefreshCurrentConnectAddressAsync();
    }
}