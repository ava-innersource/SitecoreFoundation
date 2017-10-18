using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Facets;

namespace SF.Feature.Handlebars
{
    public class RegisterUserSettingsHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("userSettings", (writer, context, args) =>
            {
                string key = args[0].ToString();
                string area = UserSettingsConfiguration.DefaultArea;
                if (args.Length > 1 && !string.IsNullOrEmpty(args[1].ToString()))
                {
                    area = args[1].ToString();
                }

                var setting = SF.Foundation.Facets.Facades.UserSettings.Settings[key, area];
                if (!string.IsNullOrEmpty(setting))
                {
                    writer.Write(setting);
                }
                else
                {
                    //writer.Write("<!-- Nothing Found for Key: " + key + " -->");
                }

            }));
        }
    }
}
