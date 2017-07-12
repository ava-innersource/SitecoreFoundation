using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SF.Feature.Social
{
    public static class SocialTagHtmlHelper
    {
        /// <summary>
        /// This html Helper will render all the Social Tags configured for the current Item
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString GetSocialTags(this HtmlHelper helper)
        {
            SocialTagManager manager = new SocialTagManager();
            return new MvcHtmlString(manager.GetSocialTags());
        }
    }
}
