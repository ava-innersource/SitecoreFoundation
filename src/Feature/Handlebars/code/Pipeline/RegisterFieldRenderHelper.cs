using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class RegisterRenderFieldWithParameters
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("fieldRenderer", (writer, context, args) =>
            {
                var field = args[0] as Field;
                if (field == null)
                {
                    return;
                }

                string renderedValue = string.Empty;

                try
                {
                    renderedValue = Sitecore.Web.UI.WebControls.FieldRenderer.Render(field.Item, field.Name, args[1] == null ? string.Empty : args[1].ToString());
                }
                catch (Exception ex)
                {
                    if (Sitecore.Context.PageMode.IsExperienceEditor)
                    {
                        renderedValue = ex.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(renderedValue))
                {
                    writer.Write(renderedValue);
                }

            }));
        }
    }
}
