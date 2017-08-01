using Sitecore.Mvc.Pipelines;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace SF.Feature.Handlebars
{
    public class RegisterPlaceholderHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("placeholder", (writer, context, args) =>
            {
                var placeholderName = args[0].ToString();

                Guid currentRenderingId = RenderingContext.Current.Rendering.UniqueId;

                if (currentRenderingId != Guid.Empty)
                {
                    placeholderName = String.Format("{0}_{1}", placeholderName, currentRenderingId);
                }

                if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    writer.Write(@"<div data-container-title=""{0}"">", placeholderName);
                }

                //save current context for later
                var oldContext = HttpContext.Current.Items["HandlebarDataSource"];
                
                
                PipelineService.Get().RunPipeline<RenderPlaceholderArgs>("mvc.renderPlaceholder", new RenderPlaceholderArgs(placeholderName, writer, Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering));

                //put it back.
                HttpContext.Current.Items["HandlebarDataSource"] = oldContext;

                if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    writer.Write(@"</div>");
                }
            }));
        }
    }
}
