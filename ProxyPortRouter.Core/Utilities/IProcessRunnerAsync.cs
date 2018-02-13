namespace ProxyPortRouter.Core.Utilities
{
    using System.Threading.Tasks;

    public interface IProcessRunnerAsync
    {
        Task<string> RunAsync(string command, string arguments);
    }
}