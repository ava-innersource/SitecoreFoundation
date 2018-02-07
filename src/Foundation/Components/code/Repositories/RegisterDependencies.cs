using Microsoft.Extensions.DependencyInjection;
using SF.Foundation.Components.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Repositories
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
            serviceCollection.AddTransient<IGenericVariantRepository, GenericVariantRepository>();

        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<GenericVariantController>();

        }

    }
}