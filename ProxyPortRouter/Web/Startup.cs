namespace ProxyPortRouter.Web
{
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

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
            app.UseMvcWithDefaultRoute();
        }
    }
}
