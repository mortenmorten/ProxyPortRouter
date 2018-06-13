namespace ProxyPortRouter.Core.Config
{
    public interface ILocalSettings
    {
        string SlaveAddress { get; set; }

        int SimulatorPort { get; set; }
    }
}