namespace ProxyPortRouter
{
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

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IDisposable
    {
        private const int RestApiPort = 8080;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private IWebHost webHost;
        private Task webHostTask;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

        private void OnStartup(object sender, StartupEventArgs e)
        {
            webHost = BuildWebHost(e.Args);

            var mainWindowViewModel = new MainWindowViewModel(webHost.Services);
            MainWindow = new MainWindow(mainWindowViewModel);
            MainWindow.Show();

            webHostTask = webHost.RunAsync(cts.Token);
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            cts.Cancel();
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
