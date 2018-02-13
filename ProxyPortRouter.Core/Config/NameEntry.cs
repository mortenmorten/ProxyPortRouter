namespace ProxyPortRouter.Core.Config
{
    using System.Runtime.Serialization;

    [DataContract]
    public class NameEntry
    {
        public NameEntry()
        {
        }

        public NameEntry(string name)
        {
            Name = name;
        }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}