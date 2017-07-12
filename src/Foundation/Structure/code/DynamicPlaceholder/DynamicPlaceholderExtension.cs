using Sitecore.Mvc.Common;
using Sitecore.Mvc.Helpers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Structure
{
    public static class DynamicPlaceholderExtension
    {
        public static HtmlString DynamicPlaceholder(this SitecoreHelper helper, string dynamicKey)
        {
            Guid currentRenderingId = RenderingContext.Current.Rendering.UniqueId;

            if (currentRenderingId != Guid.Empty)
            {
                return helper.Placeholder(String.Format("{0}_{1}", dynamicKey, currentRenderingId));
            }

            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                return new HtmlString(string.Format(@"<div data-container-title=""{0}"">{1}</div>",
                    dynamicKey, 
                    helper.Placeholder(dynamicKey).ToHtmlString()));
            }
            else
            {
                return helper.Placeholder(dynamicKey);
            }
        }

    }
}
