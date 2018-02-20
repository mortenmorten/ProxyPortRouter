namespace ProxyPortRouter.Core.Config
{
    using System;
    using System.IO;

    using ProxyPortRouter.Core.Utilities;

    using Serilog;

    public static class SettingsFile
    {
        public static T LoadFromProgramData<T>(string filename)
            where T : class
        {
            try
            {
                return Load<T>(Path.Combine(GetMyCommonApplicationDataFolder(), filename));
            }
            catch (Exception exception)
            {
                Log.Error("Error: {Exception}", exception);
                return default(T);
            }
        }

        private static T Load<T>(string filename)
            where T : class
        {
            Log.Information("JSON deserializing file: {Filename}", filename);
            return JsonSerializer<T>.Deserialize(filename);
        }

        private static string GetMyCommonApplicationDataFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Proxy Port Router");
        }
    }
}
