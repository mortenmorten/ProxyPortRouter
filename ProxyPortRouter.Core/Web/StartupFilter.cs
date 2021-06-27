namespace ProxyPortRouter.Core.Web
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    internal class StartupFilter : IStartupFilter
    {
        private IBackendAsync backend;

        public StartupFilter(IBackendAsync backend)
        {
            this.backend = backend;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            backend.InitializeAsync().GetAwaiter().GetResult();
            return next;
        }
    }
}
