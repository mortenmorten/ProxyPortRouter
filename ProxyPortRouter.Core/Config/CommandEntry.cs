namespace ProxyPortRouter.Core.Config
{
    using System.Runtime.Serialization;

    [DataContract]
    public class CommandEntry : NameEntry
    {
        [DataMember(Name = "address")]
        public string Address { get; set; }
    }
}
