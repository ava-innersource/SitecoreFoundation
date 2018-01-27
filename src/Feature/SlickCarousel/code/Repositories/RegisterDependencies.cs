using Microsoft.Extensions.DependencyInjection;
using SF.Feature.SlickCarousel.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.SlickCarousel.Repositories
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
            serviceCollection.AddTransient<ISlickCarouselRepository, SlickCarouselRepository>();
            serviceCollection.AddTransient<ISlickCarouselItemRepository, SlickCarouselItemRepository>();

        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<SlickCarouselController>();

        }

    }
}