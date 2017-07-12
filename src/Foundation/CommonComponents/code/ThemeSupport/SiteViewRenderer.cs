using Sitecore;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace SF.Foundation.CommonComponents
{
    /// <summary>
    /// Allows for Themed Component Rendering cshtml files
    /// </summary>
    public class SiteViewRenderer : ViewRenderer
    {
        public override void Render(TextWriter writer)
        {
            if (Context.Site != null)
            {
                string vrKey = Context.Site.Name + ViewPath;

                //This cache is cleared on publish by resource pipeline
                //cache is used to avoid expensive check for path on every request.
                var newViewPath = HttpRuntime.Cache.Get(vrKey);
                if (newViewPath == null)
                {
                    newViewPath = ViewPath;
                    string filePath = ViewPath.Replace(@"Views/", @"Themes/" + Context.Site.Name + @"/Views");
                    if (HostingEnvironment.VirtualPathProvider.FileExists(filePath))
                    {
                        newViewPath = filePath;
                    }
                     HttpRuntime.Cache.Insert(vrKey, newViewPath);
                }

                ViewPath = newViewPath.ToString();
            }

            base.Render(writer);
        }
    }
}
