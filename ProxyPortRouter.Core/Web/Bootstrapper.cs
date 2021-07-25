namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Net.Http;
    using Microsoft.Extensions.DependencyInjection;
    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Socket;
    using ProxyPortRouter.Core.Utilities;

    public static class Bootstrapper
    {
        public static void Initialize(this IServiceCollection services)
        {
            services.AddSingleton<ISettings>(SettingsFile.LoadFromProgramData<Settings>("entries.json"));
            services.AddSingleton<ILocalSettings>(SettingsFile.LoadFromProgramData<LocalSettings>("settings.json"));
            services.AddSingleton<IPortProxyControllerAsync, PortProxyController>();
            services.AddSingleton<IOptions>(p => Options.Create(Environment.GetCommandLineArgs()));
            services.AddHttpClient();
            services.AddSingleton<ISlaveClientAsync>(p =>
            {
                var optionsAddress = p.GetService<IOptions>()?.SlaveAddress;
                if (string.IsNullOrEmpty(optionsAddress))
                {
                    optionsAddress = p.GetService<ILocalSettings>()?.SlaveAddress;
                }

                if (string.IsNullOrEmpty(optionsAddress))
                {
                    return null;
                }

                var addressParts = optionsAddress.Split(':');
                var host = addressParts.Length > 0 ? addressParts[0] : optionsAddress;
                var port = addressParts.Length > 1 && int.TryParse(addressParts[1], out var parsedPort) ? parsedPort : 8080;
                var uriBuilder = new UriBuilder()
                {
                    Scheme = "http",
                    Host = host,
                    Port = port,
                };

                var httpClient = p.GetRequiredService<IHttpClientFactory>().CreateClient();
                httpClient.BaseAddress = uriBuilder.Uri;
                httpClient.Timeout = TimeSpan.FromSeconds(5);

                return new RestClient(httpClient);
            });

            services.AddTransient<IProcessRunnerAsync, ProcessRunner>();
            services.AddHostedService<TextCommandListener>();
        }

        public static void SetupBackendService(this IServiceCollection services, bool useLocal = false)
        {
            if (useLocal)
            {
                services.AddSingleton<IBackendAsync, LocalBackend>();
            }
            else
            {
                services.AddSingleton<IBackendAsync, RestBackend>();
            }
        }
    }
}
