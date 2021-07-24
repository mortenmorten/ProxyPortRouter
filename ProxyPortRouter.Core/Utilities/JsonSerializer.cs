namespace ProxyPortRouter.Core.Utilities
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static class JsonSerializer<T>
        where T : class, new()
    {
        public static T Deserialize(string fileName)
        {
            try
            {
                var content = File.ReadAllText(fileName);
                return DeserializeFromString(content);
            }
            catch
            {
                return new T();
            }
        }

        public static T DeserializeFromString(string json)
        {
            using var fs = new MemoryStream(Encoding.UTF8.GetBytes(json));
            return Deserialize(fs);
        }

        public static string Serialize(T o)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using var stream = new MemoryStream();
            serializer.WriteObject(stream, o);
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        private static T Deserialize(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings());
            return serializer.ReadObject(stream) as T;
        }
    }
}
