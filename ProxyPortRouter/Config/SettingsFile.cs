using ProxyPortRouter.Utilities;

namespace ProxyPortRouter.Config
{
    public static class SettingsFile
    {
        public static Settings Load(string filename)
        {
            return JsonSerializer<Settings>.Deserialize(filename);
        }
    }
}
