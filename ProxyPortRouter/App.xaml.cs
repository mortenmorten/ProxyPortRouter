using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProxyPortRouter.Clients;
using ProxyPortRouter.Config;
using ProxyPortRouter.Utilities;
using ProxyPortRouter.Web;

namespace ProxyPortRouter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const int RestApiPort = 8080;

        private IWebHost webHost;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private Task webHostTask;

        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureServices(ConfigureServices)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, RestApiPort);
                })
                .Build();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISettings>(SettingsFile.Load("entries.json"));
            serviceCollection.AddSingleton<IPortProxyManager, PortProxyManager>();
            serviceCollection.AddSingleton<IPortProxyController, PortProxyController>();
            serviceCollection.AddSingleton<ISlaveClient>(p =>
            {
                var syncAddress = p.GetService<IOptions>().SlaveAddress;
                return string.IsNullOrEmpty(syncAddress) ? null : new RestClient(new Uri($"http://{syncAddress}:{RestApiPort}"));
            });
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IOptions>(p => Options.Create(e.Args));
            ConfigureServices(serviceCollection);

            webHost = BuildWebHost(e.Args);

            var mainWindowViewModel = new MainWindowViewModel(webHost.Services);
            MainWindow = new MainWindow(mainWindowViewModel);
            MainWindow.Show();

            webHostTask = webHost.RunAsync(cts.Token);
        }

        private async void OnExit(object sender, ExitEventArgs e)
        {
            cts.Cancel();
            await webHostTask;
            webHost?.Dispose();
        }
    }
}
