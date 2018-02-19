namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Web.Http;

    using JetBrains.Annotations;

    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.StaticFiles;

    using Owin;

    using Serilog.Context;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host.
            var config = new HttpConfiguration();

            config.EnableCors();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.DependencyResolver = new Resolver(ServiceProviderBuilder.BuildServiceProvider());

            app.Use<LoggingMiddelware>();

            app.UseWebApi(config);

            var physicalFileSystem = new PhysicalFileSystem(@".\wwwroot");
            var options = new FileServerOptions
                              {
                                  EnableDefaultFiles = true,
                                  FileSystem = physicalFileSystem,
                                  StaticFileOptions =
                                      {
                                          FileSystem = physicalFileSystem,
                                          ServeUnknownFileTypes = true
                                      },
                                  DefaultFilesOptions = { DefaultFileNames = new[] { "index.html" } }
                              };

            app.UseFileServer(options);
        }
    }
}
