namespace ProxyPortRouter.Clients
{
    using System.Threading.Tasks;

    public interface ISlaveClient
    {
        Task SetCurrentEntryAsync(string name);
    }
}