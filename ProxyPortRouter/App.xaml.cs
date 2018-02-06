using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProxyPortRouter.Utilities;
using ProxyPortRouter.Web;

namespace ProxyPortRouter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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
                    options.Listen(IPAddress.Any, 8080);
                })
                .Build();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConfig>(ConfigFile.Load("entries.json"));
            serviceCollection.AddSingleton<IPortProxyManager, PortProxyManager>();
            serviceCollection.AddSingleton<IPortProxyController, PortProxyController>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
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
