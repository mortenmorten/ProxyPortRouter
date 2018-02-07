namespace ProxyPortRouter.Config
{
    using System.Collections.Generic;

    public interface ISettings
    {
        string ListenAddress { get; set; }

        List<CommandEntry> Entries { get; set; }
    }
}