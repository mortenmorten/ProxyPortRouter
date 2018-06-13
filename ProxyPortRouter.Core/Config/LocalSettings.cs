namespace ProxyPortRouter.Core.Config
{
    using System.ComponentModel;
    using System.Runtime.Serialization;

    [DataContract]
    public class LocalSettings : ILocalSettings
    {
        [DataMember(Name = "slave", IsRequired = false)]
        public string SlaveAddress { get; set; }

        [DataMember(Name = "simulatorPort", IsRequired = false)]
        [DefaultValue(80)]
        public int SimulatorPort { get; set; }
    }
}
