using ProxyPortRouter.Utilities;

namespace ProxyPortRouter
{
    public static class ConfigFile
    {
        public static Config Load(string filename)
        {
            return JsonSerializer<Config>.Deserialize(filename);
        }
    }
}
