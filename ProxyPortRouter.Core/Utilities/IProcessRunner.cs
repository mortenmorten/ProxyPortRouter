namespace ProxyPortRouter.Core.Utilities
{
    public interface IProcessRunner
    {
        string Run(string command, string arguments);
    }
}