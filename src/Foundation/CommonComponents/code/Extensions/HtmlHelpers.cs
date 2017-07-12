using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SF.Foundation.CommonComponents
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// This Html Helper returns the value of the renderings configured parameter given the parameter name.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static string GetRenderingParameter(this HtmlHelper helper, string parameterName)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc == null || rc.Rendering == null) return (string)null;
            var parametersAsString = rc.Rendering.Properties["Parameters"];
            var parameters = HttpUtility.ParseQueryString(parametersAsString);

            return parameters[parameterName];
        }

        public static string GetPlaceholderPrefix(this HtmlHelper helper, string parameterName = "Name")
        {
            string name = helper.GetRenderingParameter(parameterName);
            if (!string.IsNullOrEmpty(name))
            {
                return name + "_";
            }
            return string.Empty;
        }

        public static Item GetRenderingItem(this HtmlHelper helper)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc != null && rc.Rendering != null)
            {
                if (!string.IsNullOrEmpty(rc.Rendering.DataSource))
                {
                    return Sitecore.Context.Database.GetItem(rc.Rendering.DataSource);
                }
            }
            return Sitecore.Context.Item;
        }

        

       
    }
}
