namespace ProxyPortRouterService
{
    using System;
    using System.IO;
    using System.ServiceProcess;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.Core.Web;

    using Serilog;

    public partial class Service : ServiceBase
    {
        private Main app;

        public Service()
        {
            ServiceProviderBuilder.SetupBackendService(true);

            InitializeComponent();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                            "Proxy Port Router",
                            "service.log"))
                .CreateLogger();
        }

        protected override void OnStart(string[] args)
        {
            Log.Information("Proxy Port Router is starting");
            try
            {
                app = new Main();
                app.Start(args);
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Unhandled exception in OnStart");
            }
        }

        protected override void OnStop()
        {
            Log.Information("Proxy Port Router is stopping");
            app.Stop();
            app.Dispose();
            app = null;
        }
    }
}
