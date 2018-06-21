using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class RegisterIfNotEqualHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("ifNotEqual", (writer, options, context, args) =>
            {
                if (args[0] != args[1] && !args[0].Equals(args[1]))
                 {
                     options.Template(writer, (object)context);
                 }
                else
                {
                    options.Inverse(writer, (object)context);
                }
            }));
        }
    }
}
