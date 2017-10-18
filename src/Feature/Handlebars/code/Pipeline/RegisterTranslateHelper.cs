using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class RegisterTranslateHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("translate", (writer, context, args) =>
            {
                string input = args[0].ToString();
                var translated = Sitecore.Globalization.Translate.Text(input);
                if (!string.IsNullOrEmpty(translated))
                {
                    writer.Write(translated);
                }
                else
                {
                    writer.Write("<!-- Nothing Found for Key: " + input + " -->");
                }

            }));
        }
    }
}
