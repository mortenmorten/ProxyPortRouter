namespace ProxyPortRouter.Core
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Owin.Hosting;

    using ProxyPortRouter.Core.Web;

    using Serilog;

    public class Main : IDisposable
    {
        private const int RestApiPort = 8080;

        private IDisposable webHost;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start()
        {
            Log.Debug("Starting Main");

            // Trick to bypass the assembly optimization of VS
            Trace.TraceInformation(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).FullName);
            webHost = WebApp.Start<Startup>($"http://*:{RestApiPort}");

            Task.Run(() => ServiceProviderBuilder.ServiceProvider.GetService<IBackendAsync>().InitializeAsync());
        }

        public void Stop()
        {
            Log.Debug("Stopping Main");
            webHost.Dispose();
            webHost = null;
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            webHost?.Dispose();
            Log.CloseAndFlush();
        }
    }
}
