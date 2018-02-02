using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyPortRouter
{
    [DataContract]
    public class CommandEntries
    {
        [DataMember(Name = "listenAddress")]
        public string ListenAddress { get; set; }

        [DataMember(Name = "entries")]
        public List<CommandEntry> Entries { get; set; }
    }
}