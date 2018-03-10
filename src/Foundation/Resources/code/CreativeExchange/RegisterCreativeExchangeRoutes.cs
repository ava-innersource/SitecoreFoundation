using Sitecore.Pipelines;
using System.Web.Http;
using SF.Foundation.API;

namespace SF.Foundation.Resources
{
    public class RegisterCreativeExchangeRoutes : RegisterRoutesBase
    {
        public void Process(PipelineArgs args)
        {

            GlobalConfiguration.Configure(this.Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            MapRouteWithSession(configuration, "SF.CreativeExchange", "api/sf/1.0/creativeExchange/import/{homePageId}", "CreativeExchange", "Import");
        }
    }
}
