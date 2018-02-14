namespace ProxyPortRouter.Core.Web
{
    using System.Web.Http;

    using JetBrains.Annotations;

    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.Logging;
    using Microsoft.Owin.StaticFiles;
    using Microsoft.Owin.StaticFiles.Infrastructure;

    using Owin;

    public class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host.
            var config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            config.DependencyResolver = new Resolver(ServiceProviderBuilder.BuildServiceProvider());

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
