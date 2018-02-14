namespace ProxyPortRouterService
{
    using System;
    using System.IO;

    using ProxyPortRouter.Core;
    using ProxyPortRouter.Core.Web;

    using Serilog;

    using Topshelf;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .WriteTo.File(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Proxy Port Router",
                    "service.log"))
                .CreateLogger();

            ServiceProviderBuilder.SetupBackendService(true);

            var rc = HostFactory.Run(
                x =>
                    {
                        x.UseSerilog();
                        x.Service<Main>(
                            s =>
                                {
                                    s.ConstructUsing(f => new Main());
                                    s.WhenStarted(tc => tc.Start());
                                    s.WhenStopped(tc => tc.Stop());
                                });
                        x.RunAsLocalSystem();

                        x.SetDescription("Service for hosting a proxy port router");
                        x.SetDisplayName("Proxy Port Router");
                        x.SetServiceName("ProxyPortRouterService");
                    });

            // ReSharper disable once PossibleNullReferenceException
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }
    }
}
