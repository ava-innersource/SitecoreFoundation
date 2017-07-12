using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SF.Foundation.Resources
{
    public class RegisterResourceRoutes
    {
        public void Process(PipelineArgs args)
        {

            GlobalConfiguration.Configure(Configure);
        }
        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("SF.SyncResource", "sitecore/api/sf/sync", new
            {
                controller = "Sync",
                action = "Sync"
            });
        }
    }
}
