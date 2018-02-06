using System.Threading.Tasks;

namespace ProxyPortRouter.Clients
{
    public interface ISlaveClient
    {
        Task SetCurrentEntry(string name);
    }
}