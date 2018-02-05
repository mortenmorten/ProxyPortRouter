using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ProxyPortRouter.Web
{
    [UsedImplicitly]
    public class Startup
    {
        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug(LogLevel.Debug);
            //app.UseMvc(routes => { routes.MapRoute("default", "api/{controller}/{id?}"); });
            app.UseMvcWithDefaultRoute();
        }
    }
}
