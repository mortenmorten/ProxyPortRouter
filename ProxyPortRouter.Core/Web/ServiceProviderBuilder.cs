namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using Microsoft.Extensions.DependencyInjection;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;

    public static class ServiceProviderBuilder
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static IServiceProvider BuildServiceProvider(string[] args)
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISettings>(SettingsFile.LoadFromProgramData("entries.json"));
            services.AddSingleton<IPortProxyManager, PortProxyManager>();
            services.AddSingleton<IPortProxyController, PortProxyController>();
            services.AddSingleton<IOptions>(p => Options.Create(args));
            services.AddSingleton<ISlaveClient>(p =>
                {
                    var syncAddress = p.GetService<IOptions>()?.SlaveAddress;
                    return string.IsNullOrEmpty(syncAddress) ? null : new RestClient(new Uri($"http://{syncAddress}:{8080}"));
                });

            // For WebApi controllers, you may want to have a bit of reflection
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(ApiController).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            // It is only that you need to get service provider in the end
            ServiceProvider = services.BuildServiceProvider();
            return ServiceProvider;
        }
    }
}
