namespace ProxyPortRouter.Core.Clients
{
    using System.Threading.Tasks;

    public interface ISlaveClientAsync
    {
        Task SetCurrentEntryAsync(string name);
    }
}