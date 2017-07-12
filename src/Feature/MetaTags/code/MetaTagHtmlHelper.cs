using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SF.Feature.MetaTags
{
    public static class MetaTagHtmlHelper
    {
        /// <summary>
        /// This HtmlHelper renders title and meta tags based on
        /// current site settings and current item settings.
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString GetMetaTags(this HtmlHelper helper)
        {
            MetaTagManager manager = new MetaTagManager();
            return new MvcHtmlString(manager.GetMetaTags());
        }
    }
}
