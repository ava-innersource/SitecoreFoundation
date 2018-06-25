using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Pipelines.RenderVariantField;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.RenderVariantField;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.Mvc;
using System.Web.Routing;
using SF.Foundation.Components.Controllers;

namespace SF.Foundation.Components.Variants
{
    public class RenderViewRenderingTemplate : RenderRenderingVariantFieldProcessor
    {
        public override RendererMode RendererMode
        {
            get
            {
                return RendererMode.Html;
            }
        }

        public override Type SupportedType
        {
            get
            {
                return typeof(ViewRenderingTemplate);
            }
        }

        public RenderViewRenderingTemplate()
        {
        }

        public override void RenderField(RenderVariantFieldArgs args)
        {
            var variantField = args.VariantField as ViewRenderingTemplate;
            if (variantField != null)
            {
                HtmlGenericControl htmlGenericControl = new HtmlGenericControl((string.IsNullOrWhiteSpace(variantField.Tag) ? "div" : variantField.Tag));
                this.AddClass(htmlGenericControl, variantField.CssClass);
                this.AddWrapperDataAttributes(variantField, args, htmlGenericControl);
                
                htmlGenericControl.InnerHtml = RenderViewToString(args.Item, variantField.ViewPath);

                args.ResultControl = htmlGenericControl;
                args.Result = this.RenderControl(args.ResultControl);
            }
        }

        public static string RenderViewToString(Item item, string viewPath)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(HttpContext.Current);
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new GenericVariantController(null));
            var razor = new RazorView(controllerContext, viewPath, null, false, null);
            razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(item), new TempDataDictionary(), st), st);
            return st.ToString();
        }
    }
}