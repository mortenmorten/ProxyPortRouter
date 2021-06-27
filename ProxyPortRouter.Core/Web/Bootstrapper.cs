namespace ProxyPortRouter.Core.Web
{
    using System;
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
            services.AddSingleton<ILocalSettings>(provider => SettingsFile.LoadFromProgramData<LocalSettings>("settings.json"));
            services.AddSingleton<IPortProxyControllerAsync, PortProxyController>();
            services.AddSingleton<IOptions>(p => Options.Create(Environment.GetCommandLineArgs()));
            services.AddSingleton<ISlaveClientAsync>(p =>
            {
                var optionsAddress = p.GetService<IOptions>()?.SlaveAddress;
                if (string.IsNullOrEmpty(optionsAddress))
                {
                    optionsAddress = p.GetService<ILocalSettings>()?.SlaveAddress;
                }

                return string.IsNullOrEmpty(optionsAddress) ? null : new RestClient(new Uri($"http://{optionsAddress}:{8080}"));
            });
            services.AddTransient<IProcessRunnerAsync, ProcessRunner>();
            services.AddSingleton<TextCommandListener>();

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
