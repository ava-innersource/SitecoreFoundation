using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SF.Feature.Handlebars
{
    public class RegisterPlaceholderHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("placeholder", (writer, context, args) =>
            {
                var passedInName = args[0].ToString(); 
                var placeholderName = args[0].ToString();
                var placeholderIndex = args.Length > 1 ? args[1].ToString() : "1";
                placeholderIndex = string.IsNullOrEmpty(placeholderIndex) ? "1" : placeholderIndex;
                
                Guid currentRenderingId = RenderingContext.Current.Rendering.UniqueId;

                if (currentRenderingId != Guid.Empty)
                {
                    placeholderName = String.Format("{0}-{1}-{2}", placeholderName, currentRenderingId.ToString("B"), placeholderIndex);
                }

                if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    writer.Write(@"<div data-container-title=""{0}"">", placeholderName);
                }

                //save current context for later
                var oldContext = HttpContext.Current.Items["HandlebarDataSource"];
                var oldRenderingItem = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.Item;

                //Helper set from Variant field class as it's passed as arg.
                HtmlHelper helper = HttpContext.Current.Items["htmlHelper"] as HtmlHelper;
                if (helper != null)
                {
                    var placeholderStr = (new Sitecore.Mvc.Helpers.SitecoreHelper(helper)).DynamicPlaceholder(passedInName).ToHtmlString();
                    writer.Write(placeholderStr);
                }
                else
                {
                    //The old manual way where we have no access to helper.
                    PipelineService.Get().RunPipeline<RenderPlaceholderArgs>("mvc.renderPlaceholder", new RenderPlaceholderArgs(placeholderName, writer, new ContentRendering()));
                }
                //put it back.
                HttpContext.Current.Items["HandlebarDataSource"] = oldContext;
                Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering.Item = oldRenderingItem;

                if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    writer.Write(@"</div>");
                }
            }));
        }
    }
}
