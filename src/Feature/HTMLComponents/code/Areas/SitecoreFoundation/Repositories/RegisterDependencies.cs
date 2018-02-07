using Microsoft.Extensions.DependencyInjection;
using SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Repositories
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
            serviceCollection.AddTransient<IHorizontalRuleRepository, HorizontalRuleRepository>();

        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<HorizontalRuleController>();

        }
    }
}