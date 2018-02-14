namespace ProxyPortRouter.Core.Config
{
    using System;
    using System.IO;

    using ProxyPortRouter.Core.Utilities;

    using Serilog;

    public static class SettingsFile
    {
        public static Settings Load(string filename)
        {
            Log.Information("JSON deserializing file: {Filename}", filename);
            return JsonSerializer<Settings>.Deserialize(filename);
        }

        public static Settings LoadFromProgramData(string filename)
        {
            return Load(Path.Combine(GetMyCommonApplicationDataFolder(), filename));
        }

        private static string GetMyCommonApplicationDataFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Proxy Port Router");
        }
    }
}
