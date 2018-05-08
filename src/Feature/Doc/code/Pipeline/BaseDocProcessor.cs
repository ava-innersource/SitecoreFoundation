using SF.Feature.Doc.Controllers;
using SF.Feature.Doc.Model;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;

namespace SF.Feature.Doc.Pipeline
{
    public class BaseDocProcessor
    {
        public virtual void Process(DocumentationArgs args)
        {
            var section = new Section();

            ConfigureSection(args.Item);

            section.Name = this.SectionName;
            section.Content = this.SectionContent;
            section.IncludeInTOC = this.IncludeInTOC;

            args.Document.Sections.Add(section);
        }

        public virtual void ConfigureSection(Item item)
        {

        }

        public string SectionName { get; set; }
        public string SectionContent { get; set; }

        public bool IncludeInTOC { get; set; }

        public static string RenderPartialView(string partialLocation, object model)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(HttpContext.Current);
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new DocController());
            var razor = new RazorView(controllerContext, partialLocation, null, false, null);
            razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), st), st);
            return st.ToString();
            
        }

    }

}