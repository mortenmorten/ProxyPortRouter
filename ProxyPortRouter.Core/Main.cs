namespace ProxyPortRouter.Core
{
    using System;
    using System.Diagnostics;

    using Microsoft.Owin.Hosting;

    using ProxyPortRouter.Core.Web;

    public class Main : IDisposable
    {
        private const int RestApiPort = 8080;

        private IDisposable webHost;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(string[] args)
        {
            // Trick to bypass the assembly optimization of VS
            Trace.TraceInformation(typeof(Microsoft.Owin.Host.HttpListener.OwinHttpListener).FullName);
            webHost = WebApp.Start<Startup>($"http://*:{RestApiPort}");
        }

        public void Stop()
        {
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
        }
    }
}
