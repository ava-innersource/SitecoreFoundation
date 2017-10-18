using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SF.Foundation.API;

namespace SF.Feature.Handlebars
{
    public class RegisterHandlebarRoutes : RegisterRoutesBase
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(Configure);
        }
        protected void Configure(HttpConfiguration configuration)
        {
            MapRouteWithSession(configuration, "SF.Handlebars.AddItem", "sitecore/api/sf/additem", "HandlebarsAPI", "AddItem");
        }
    }
}
