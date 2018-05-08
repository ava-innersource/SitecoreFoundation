using SF.Feature.Doc.Pipeline;
using Sitecore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.Doc.Controllers
{
    public class DocController : SitecoreController
    {
        [HttpGet]
        public override ActionResult Index()
        {
            var sb = new StringBuilder();

            DocumentationArgs args = new DocumentationArgs(Sitecore.Context.Item);
            Sitecore.Pipelines.CorePipeline.Run("docProcessors", args);

            foreach(var section in args.Document.Sections)
            {
                sb.Append(section.Content);
            }

            return Content(sb.ToString());
        }
    }
}