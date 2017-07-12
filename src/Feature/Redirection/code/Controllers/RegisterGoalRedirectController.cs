using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using System.Web.Mvc;

namespace SF.Feature.Redirection
{
    public class RegisterGoalRedirectController
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("goalRedirectRoute", "redirect/{goal}", new 
                {
                    controller = "GoalRedirect",
                    action = "Redirect"
                });
        }
       

    }
}
