namespace ProxyPortRouter.Core.Config
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CommandEntry
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "address")]
        public string Address { get; set; }
    }
}
