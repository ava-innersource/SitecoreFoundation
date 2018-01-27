using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SF.Feature.VideoJSPlayer.Controllers;

namespace SF.Feature.VideoJSPlayer.Repositories
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
            serviceCollection.AddTransient<IVideoJSPlayerRepository, VideoJSPlayerRepository>();
            
        }

        private void RegisterControllers(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<VideoJSPlayerController>();
            
        }

    }
}