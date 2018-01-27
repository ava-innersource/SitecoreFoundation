using Microsoft.Extensions.DependencyInjection;
using SF.Feature.OwlCarousel.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.OwlCarousel.Repositories
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
            serviceCollection.AddTransient<IOwlCarouselRepository, OwlCarouselRepository>();
            serviceCollection.AddTransient<IOwlCarouselItemRepository, OwlCarouselItemRepository>();

        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<OwlCarouselController>();

        }

    }
}