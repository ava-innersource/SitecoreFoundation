using Microsoft.Extensions.DependencyInjection;
using SF.Feature.OwlCarousel.Controllers;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.OwlCarousel.Repositories
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A register dependencies. </summary>
    ///
    /// <remarks>   David San Filippo, 5/7/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class RegisterDependencies : IServicesConfigurator
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Configures the given service collection. </summary>
        ///
        /// <remarks>   David San Filippo, 5/7/2018. </remarks>
        ///
        /// <param name="serviceCollection">    Collection of services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Configure(IServiceCollection serviceCollection)
        {
            RegisterRepositories(serviceCollection);
            RegisterControllers(serviceCollection);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Registers the repositories described by serviceCollection./ </summary>
        ///
        /// <remarks>   David San Filippo, 5/7/2018. </remarks>
        ///
        /// <param name="serviceCollection">    Collection of services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void RegisterRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IOwlCarouselRepository, OwlCarouselRepository>();
            serviceCollection.AddTransient<IOwlCarouselItemRepository, OwlCarouselItemRepository>();

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Registers the controllers described by serviceCollection. </summary>
        ///
        /// <remarks>   David San Filippo, 5/7/2018. </remarks>
        ///
        /// <param name="serviceCollection">    Collection of services. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<OwlCarouselController>();

        }

    }
}