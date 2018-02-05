using System.Collections.Generic;

namespace ProxyPortRouter
{
    public interface IConfig
    {
        string ListenAddress { get; set; }

        List<CommandEntry> Entries { get; set; }
    }
}