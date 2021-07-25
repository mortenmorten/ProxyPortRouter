namespace ProxyPortRouterService
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    using ProxyPortRouter.Core.Web;

    using Serilog;

    using Topshelf;
    using Topshelf.Extensions.Hosting;

    using Host = Microsoft.Extensions.Hosting.Host;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Proxy Port Router",
                    "service.log"))
                .CreateLogger();

            var rc = CreateHostBuilder(args).RunAsTopshelfService(x =>
            {
                x.RunAsLocalSystem();
                x.SetDescription("Service for hosting a proxy port router");
                x.SetDisplayName("Proxy Port Router");
                x.SetServiceName("ProxyPortRouterService");
            });

            // ReSharper disable once PossibleNullReferenceException
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .ConfigureServices((context, services) =>
                {
                    services.Initialize();
                    services.SetupBackendService(true);
                })
                .UseSerilog()
                ;
    }
}
