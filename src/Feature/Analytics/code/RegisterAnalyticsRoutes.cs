using Sitecore.Pipelines;
using System.Web.Http;
using SF.Foundation.API;

namespace SF.Feature.Analytics
{
    public class RegisterAnalyticsRoutes
    {
        public void Process(PipelineArgs args)
        {

            GlobalConfiguration.Configure(this.Configure);
        }

        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("SF.Analytics", "sitecore/api/sf/analytics/{action}", new
            {
                controller = "Analytics",
                action = "index"
            });

            var route = System.Web.Routing.RouteTable.Routes["SF.Analytics"] as System.Web.Routing.Route;
            route.RouteHandler = new SessionRouteHandler();
        }
    }
}
