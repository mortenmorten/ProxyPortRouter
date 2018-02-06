using System.Collections.Generic;

namespace ProxyPortRouter.Config
{
    public interface ISettings
    {
        string ListenAddress { get; set; }

        List<CommandEntry> Entries { get; set; }
    }
}