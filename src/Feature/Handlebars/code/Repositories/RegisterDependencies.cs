using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SF.Feature.Handlebars.Controllers;

namespace SF.Feature.Handlebars.Repositories
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
            serviceCollection.AddTransient<IAddNewItemRepository, AddNewItemRepository>();
            serviceCollection.AddTransient<IHandlebarCollectionContainerRepository, HandlebarCollectionContainerRepository>();
            serviceCollection.AddTransient<IHandlebarContainerRepository, HandlebarContainerRepository>();
            serviceCollection.AddTransient<IHandlebarFacetContainerRepository, HandlebarFacetContainerRepository>();
            serviceCollection.AddTransient<IHandlebarFormRepository, HandlebarFormRepository>();
            serviceCollection.AddTransient<IHandlebarJsonContainerRepository, HandlebarJsonContainerRepository>();
            serviceCollection.AddTransient<IHandlebarQueryContainerRepository, HandlebarQueryContainerRepository>();
            serviceCollection.AddTransient<IHandlebarTemplateRepository, HandlebarTemplateRepository>();
        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AddNewItemController>();
            serviceCollection.AddTransient<HandlebarCollectionContainerController>();
            serviceCollection.AddTransient<HandlebarContainerController>();
            serviceCollection.AddTransient<HandlebarFacetContainerController>();
            serviceCollection.AddTransient<HandlebarFormController>();
            serviceCollection.AddTransient<HandlebarJsonContainerController>();
            serviceCollection.AddTransient<HandlebarQueryContainerController>();
            serviceCollection.AddTransient<HandlebarTemplateController>();
        }

    }
}