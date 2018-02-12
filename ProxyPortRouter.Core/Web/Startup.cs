namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Web.Http;

    using JetBrains.Annotations;

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

            config.DependencyResolver = new Resolver(ServiceProviderBuilder.BuildServiceProvider(Environment.GetCommandLineArgs()));

            app.UseWebApi(config);
        }
    }
}
