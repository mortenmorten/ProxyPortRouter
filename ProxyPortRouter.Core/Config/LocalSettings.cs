namespace ProxyPortRouter.Core.Config
{
    using System.Runtime.Serialization;

    [DataContract]
    public class LocalSettings : ILocalSettings
    {
        [DataMember(Name = "slave", IsRequired = false)]
        public string SlaveAddress { get; set; }
    }
}
