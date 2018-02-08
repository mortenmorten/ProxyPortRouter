namespace ProxyPortRouter.Core.Web
{
    using JetBrains.Annotations;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Serilog;

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
            loggerFactory.AddSerilog();
            app.UseMvcWithDefaultRoute();
        }
    }
}
