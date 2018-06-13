namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using Microsoft.Extensions.DependencyInjection;
    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Socket;
    using ProxyPortRouter.Core.Utilities;

    public static class ServiceProviderBuilder
    {
        private static readonly ServiceCollection Services = new ServiceCollection();

        static ServiceProviderBuilder()
        {
            Services.AddSingleton<ISettings>(SettingsFile.LoadFromProgramData<Settings>("entries.json"));
            Services.AddSingleton<ILocalSettings>(provider => SettingsFile.LoadFromProgramData<LocalSettings>("settings.json"));
            Services.AddSingleton<IPortProxyControllerAsync, PortProxyController>();
            Services.AddSingleton<IOptions>(p => Options.Create(Environment.GetCommandLineArgs()));
            Services.AddSingleton<ISlaveClientAsync>(p =>
                {
                    var optionsAddress = p.GetService<IOptions>()?.SlaveAddress;
                    if (string.IsNullOrEmpty(optionsAddress))
                    {
                        optionsAddress = p.GetService<ILocalSettings>()?.SlaveAddress;
                    }
                    return string.IsNullOrEmpty(optionsAddress) ? null : new RestClient(new Uri($"http://{optionsAddress}:{8080}"));
                });
            Services.AddTransient<IProcessRunnerAsync, ProcessRunner>();
            Services.AddSingleton<TextCommandListener>();
        }

        public static ServiceProvider ServiceProvider { get; private set; }

        public static ServiceProvider BuildServiceProvider()
        {
            // For WebApi controllers, you may want to have a bit of reflection
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(ApiController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            foreach (var type in controllerTypes)
            {
                Services.AddTransient(type);
            }

            // It is only that you need to get service provider in the end
            ServiceProvider = Services.BuildServiceProvider();
            return ServiceProvider;
        }

        public static void SetupBackendService(bool useLocal = false)
        {
            if (useLocal)
            {
                Services.AddSingleton<IBackendAsync, LocalBackend>();
            }
            else
            {
                Services.AddSingleton<IBackendAsync, RestBackend>();
            }
        }
    }
}
