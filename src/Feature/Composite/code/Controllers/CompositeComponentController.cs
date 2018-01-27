using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Common;
using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;
using Sitecore.Text;
using System.IO;
using SF.Foundation.Components;

namespace SF.Feature.Composite
{
    public class CompositeComponentController : Controller
    {
        public ActionResult EditLayout()
        {
            var pageContext = PageContext.CurrentOrNull;
            Assert.IsNotNull(pageContext, "Page context is required");
            var stringWriter = new StringWriter();
            stringWriter.Write("<html><head></head><body>");
            PipelineService.Get().RunPipeline<RenderPlaceholderArgs>("mvc.renderPlaceholder",
                new RenderPlaceholderArgs(pageContext.Item["PlaceholderName"] ?? "compositecontent", (TextWriter)stringWriter, new ContentRendering()));
            stringWriter.Write("</body></html>");
            return Content(stringWriter.ToString());
        }

        // GET: Snippet
        public ActionResult CompositeComponent()
        {
            var renderingContext = RenderingContext.CurrentOrNull;
            if (renderingContext == null)
                throw new ApplicationException("Could not find current rendering context, aborting");
            var hasDataSource = !string.IsNullOrWhiteSpace(renderingContext.Rendering.DataSource);
            var pageContext = new PageContext()
            {
                RequestContext = this.ControllerContext.RequestContext,
                Item = !hasDataSource ? null : renderingContext.Rendering.Item
            };
            var oldDisplayMode = global::Sitecore.Context.Site != null ? global::Sitecore.Context.Site.DisplayMode : DisplayMode.Normal;
            try
            {
                if (oldDisplayMode == DisplayMode.Edit && global::Sitecore.Context.Site != null)
                {
                    //disable the editing of the nested component
                    global::Sitecore.Context.Site.SetDisplayMode(DisplayMode.Preview, DisplayModeDuration.Temporary);
                }
                
                using (PlaceholderContext.Enter(new PlaceholderContext("/")))
                using (ContextService.Get().Push<PageContext>(pageContext))
                {
                    var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());

                    var stringWriter = new StringWriter();
                    if (oldDisplayMode == DisplayMode.Edit)
                    {
                        if (hasDataSource)
                        {
                            UrlString webSiteUrl = SiteContext.GetWebSiteUrl(Sitecore.Context.Database);
                            webSiteUrl.Add("sc_mode", "edit");
                            webSiteUrl.Add("sc_itemid", pageContext.Item.ID.ToString());
                            webSiteUrl.Add("sc_lang", pageContext.Item.Language.ToString());

                            //copied style from bootstrap alert
                            stringWriter.Write("<div role=\"alert\" class=\"alert alert-warning\" style=\"box-sizing: border-box; margin-bottom: 20px; border-top: rgb(250,235,204) 1px solid; border-right: rgb(250,235,204) 1px solid; white-space: normal; word-spacing: 0px; border-bottom: rgb(250,235,204) 1px solid; text-transform: none; color: rgb(138,109,59); padding-bottom: 15px; padding-top: 15px; font: 14px/20px 'Helvetica Neue', helvetica, arial, sans-serif; padding-left: 15px; border-left: rgb(250,235,204) 1px solid; widows: 1; letter-spacing: normal; padding-right: 15px; background-color: rgb(252,248,227); text-indent: 0px; border-radius: 4px; -webkit-text-stroke-width: 0px\">");
                            stringWriter.Write("<a class=\"alert-link\" style=\"box-sizing: border-box; text-decoration: none; font-weight: 700; color: rgb(102,81,44); background-color: transparent\" href=\"");
                            stringWriter.Write(webSiteUrl);
                            stringWriter.Write("\" target=\"_blank\" onmousedown=\"window.open(this.href)\">&quot;");
                            stringWriter.Write(pageContext.Item.DisplayName);
                            stringWriter.Write("&quot; is a 'composite component'. Click here to open it's editor</a><br /></div>");
                        }
                        else
                        {
                            //copied style from bootstrap alert
                            stringWriter.Write("<div role=\"alert\" class=\"alert alert-warning\" style=\"box-sizing: border-box; margin-bottom: 20px; border-top: rgb(250,235,204) 1px solid; border-right: rgb(250,235,204) 1px solid; white-space: normal; word-spacing: 0px; border-bottom: rgb(250,235,204) 1px solid; text-transform: none; color: rgb(138,109,59); padding-bottom: 15px; padding-top: 15px; font: 14px/20px 'Helvetica Neue', helvetica, arial, sans-serif; padding-left: 15px; border-left: rgb(250,235,204) 1px solid; widows: 1; letter-spacing: normal; padding-right: 15px; background-color: rgb(252,248,227); text-indent: 0px; border-radius: 4px; -webkit-text-stroke-width: 0px\">");
                            stringWriter.Write("<a class=\"alert-link\" style=\"box-sizing: border-box; text-decoration: none; font-weight: 700; color: rgb(102,81,44); background-color: transparent\" href=\"\" onmousedown=\"");
                            stringWriter.Write("Sitecore.PageModes.PageEditor.postRequest('webedit:setdatasource(referenceId=");
                            stringWriter.Write(renderingContext.Rendering.UniqueId.ToString("B").ToUpper());
                            stringWriter.Write(",renderingId=");
                            stringWriter.Write(renderingContext.Rendering.RenderingItem.ID);
                            stringWriter.Write(",id=");
                            stringWriter.Write(renderingContext.Rendering.Item.ID);
                            stringWriter.Write(")',null,false)");
                            stringWriter.Write("\" target=\"_blank\">This is a 'composite component' without a datasource. Click here to associate a composite component instance</a><br /></div>");

                        }
                    }
                    else
                    {
                        var enableAync = true; // htmlHelper.GetCheckboxRenderingParameterValue("Enable Async");
                        var baseUrl = htmlHelper.GetRenderingParameter("Async Fetch Base Url");

                        var componentClass = enableAync ? "composite async" : "composite";
                        var tagAttributes = string.Empty; // htmlHelper.GetContainerTagAttributes(componentClass);

                        var asyncUrl = renderingContext.Rendering.Item.GetItemUrl();
                        if (!string.IsNullOrEmpty(baseUrl))
                        {
                            asyncUrl = baseUrl + "/" + asyncUrl;
                        }

                        var asyncAttr = enableAync ? string.Format(@"data-src=""{0}""", asyncUrl) : string.Empty;

                        stringWriter.Write(string.Format(@"<div {0} {1}>", tagAttributes, asyncAttr));
                    }

                    if (hasDataSource)
                    {
                        var loadAsyncOnly = true; // htmlHelper.GetCheckboxRenderingParameterValue("Load Content Async Only");
                        if (!loadAsyncOnly || oldDisplayMode == DisplayMode.Edit)
                        {
                            PipelineService.Get().RunPipeline<RenderPlaceholderArgs>("mvc.renderPlaceholder", new RenderPlaceholderArgs(pageContext.Item["PlaceholderName"] ?? "compositecontent", (TextWriter)stringWriter, new ContentRendering()));
                        }
                    }

                    if (oldDisplayMode != DisplayMode.Edit)
                    {
                        stringWriter.Write("</div>");
                    }

                    return Content(stringWriter.ToString());
                }
            }
            finally
            {
                global::Sitecore.Context.Site.SetDisplayMode(oldDisplayMode, DisplayModeDuration.Temporary);
            }
        }
    }
}
