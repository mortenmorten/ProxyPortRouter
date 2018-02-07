namespace ProxyPortRouter.Config
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Settings : ISettings
    {
        [DataMember(Name = "listenAddress")]
        public string ListenAddress { get; set; }

        [DataMember(Name = "entries")]
        public List<CommandEntry> Entries { get; set; }
    }
}