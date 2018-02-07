namespace ProxyPortRouter.Core
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using ProxyPortRouter.Core.Clients;
    using ProxyPortRouter.Core.Config;
    using ProxyPortRouter.Core.Utilities;
    using ProxyPortRouter.Core.Web;

    public class Main : IDisposable
    {
        private const int RestApiPort = 8080;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private IWebHost webHost;
        private Task webHostTask;

        public IServiceProvider Services => webHost?.Services;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(string[] args)
        {
            webHost = BuildWebHost(args);

            webHostTask = webHost.RunAsync(cts.Token);
        }

        public void Stop()
        {
            cts.Cancel();
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(sc => ConfigureServices(sc, args))
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, RestApiPort);
                })
                .Build();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, string[] args)
        {
            serviceCollection.AddSingleton<ISettings>(SettingsFile.LoadFromProgramData("entries.json"));
            serviceCollection.AddSingleton<IPortProxyManager, PortProxyManager>();
            serviceCollection.AddSingleton<IPortProxyController, PortProxyController>();
            serviceCollection.AddSingleton<IOptions>(p => Options.Create(args));
            serviceCollection.AddSingleton<ISlaveClient>(p =>
            {
                var syncAddress = p.GetService<IOptions>()?.SlaveAddress;
                return string.IsNullOrEmpty(syncAddress) ? null : new RestClient(new Uri($"http://{syncAddress}:{RestApiPort}"));
            });
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            cts.Dispose();
            webHost?.Dispose();
            webHostTask?.Dispose();
        }
    }
}
