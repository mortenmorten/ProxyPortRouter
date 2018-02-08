namespace ProxyPortRouterService
{
    using System;
    using System.IO;
    using System.ServiceProcess;

    using Serilog;

    public partial class Service : ServiceBase
    {
        public Service()
        {
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
        }

        protected override void OnStop()
        {
            Log.Information("Proxy Port Router is stopping");
        }
    }
}
