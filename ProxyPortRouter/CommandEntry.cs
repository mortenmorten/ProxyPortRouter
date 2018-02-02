using System.Runtime.Serialization;

namespace ProxyPortRouter
{
    [DataContract]
    public class CommandEntry
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }
    }
}
