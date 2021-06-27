namespace ProxyPortRouter.Core.Web
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceProviderBuilder
    {
        private static readonly ServiceCollection Services = new ServiceCollection();

        static ServiceProviderBuilder()
        {
            Bootstrapper.Initialize(Services);
        }

        public static ServiceProvider ServiceProvider { get; private set; }

        public static ServiceProvider BuildServiceProvider()
        {
            // For WebApi controllers, you may want to have a bit of reflection
            var controllerTypes = Assembly.GetExecutingAssembly().GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(t)
                            || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            foreach (var type in controllerTypes)
            {
                Services.AddTransient(type);
            }

            // It is only that you need to get service provider in the end
            ServiceProvider = Services.BuildServiceProvider();
            return ServiceProvider;
        }
    }
}
