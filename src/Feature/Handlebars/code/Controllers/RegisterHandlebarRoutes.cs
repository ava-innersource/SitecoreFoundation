using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SF.Feature.Handlebars
{
    public class RegisterHandlebarRoutes
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(Configure);
        }
        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;
            routes.MapHttpRoute("SF.HandlebarsAddItem", "sitecore/api/sf/additem", new
            {
                controller = "Handlebars",
                action = "AddItem"
            });
        }
    }
}
