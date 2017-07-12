using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class RegisterLookupElementDateHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("LookupElement", (writer, context, args) =>
            {
                var input = args[0];
                var lookupKey = args[1].ToString();
                var lookupProp = args[2].ToString();


                System.Reflection.PropertyInfo info = input.GetType().GetProperties()[1];
                object dic = info.GetValue(input);



                writer.Write("testing");

            }));
        }
    }
}
