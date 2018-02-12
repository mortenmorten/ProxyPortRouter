namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;

    using Microsoft.Extensions.DependencyInjection;

    public class Resolver : IDependencyResolver
    {
        private readonly IServiceProvider provider;

        public Resolver(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public object GetService(Type serviceType)
        {
            return provider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return provider.GetServices(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
