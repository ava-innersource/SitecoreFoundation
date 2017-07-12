using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SF.Foundation.Container
{
    public static class VisibilityParameterHelper
    {
        public static bool GetCheckboxRenderingParameterValue(this HtmlHelper htmlHelper, string parameterName)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc == null || rc.Rendering == null) return false;
            var parametersAsString = rc.Rendering.Properties["Parameters"];
            var parameters = HttpUtility.ParseQueryString(parametersAsString);
            return parameters[parameterName] == "1";
        }

        public static string GetVisibilityParameterClasses(this HtmlHelper helper)
        {
            var args = new ContainerClassPipelineArgs();

            Sitecore.Pipelines.CorePipeline.Run("containerClasses", args);
            
            return string.Join(" ", args.CssClasses);
        }
    }
}
