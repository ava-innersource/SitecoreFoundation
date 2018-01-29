using Microsoft.Extensions.DependencyInjection;
using SF.Feature.Composite.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Composite.Repositories
{
    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            RegisterRepositories(serviceCollection);
            RegisterControllers(serviceCollection);
        }

        private void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<ICompositeComponentRepository, CompositeComponentRepository>();

        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CompositeComponentController>();

        }

    }
}