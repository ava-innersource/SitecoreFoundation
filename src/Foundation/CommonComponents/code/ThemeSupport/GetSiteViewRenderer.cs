using Sitecore.Mvc.Pipelines.Response.GetRenderer;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.CommonComponents
{
    public class GetSiteViewRenderer : GetViewRenderer
    {
        public override void Process(GetRendererArgs args)
        {
            base.Process(args);

            var viewRendering = args.Result as ViewRenderer;
            if (viewRendering != null)
            {
                args.Result = new SiteViewRenderer
                {
                    ViewPath = viewRendering.ViewPath,
                    Rendering = viewRendering.Rendering
                };
            }
        }
    }
}
