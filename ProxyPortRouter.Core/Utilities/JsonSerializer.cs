namespace ProxyPortRouter.Core.Utilities
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    public static class JsonSerializer<T>
        where T : class
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
                return default(T);
            }
        }

        public static T DeserializeFromString(string json)
        {
            using (var fs = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return Deserialize(fs);
            }
        }

        private static T Deserialize(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings());
            return serializer.ReadObject(stream) as T;
        }
    }
}
