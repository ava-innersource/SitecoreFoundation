using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HandlebarsDotNet;
using Sitecore.Mvc.Extensions;

namespace SF.Feature.Handlebars
{
    public class RegisterEditFrameHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("editFrame", (writer, options, context, args) =>
            {

                string id = args[0] as string;
                string buttons = args[1] as string;
                string title = args[2] as string;

                var editFrame = new Sitecore.Web.UI.WebControls.EditFrame();
                editFrame.DataSource = id;
                editFrame.Buttons = buttons;
                editFrame.Title = title;

                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                var htmlWriter = new HtmlTextWriter(sw);
                editFrame.RenderFirstPart(htmlWriter);
                writer.WriteSafeString(sb.ToString());

                options.Template(writer, (object)context);

                sb.Clear();
                editFrame.RenderLastPart(htmlWriter);
                writer.WriteSafeString(sb.ToString());
            }));
        }
    }
}
