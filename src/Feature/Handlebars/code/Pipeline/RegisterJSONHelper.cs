using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SF.Feature.Handlebars
{
    public class RegisterJSONHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("json", (writer, context, args) =>
            {
                object obj = args[0];

                //didn't like how Web Extensions Serializer looked for Expandos (Arrays of key values)
                //var jString = new JavaScriptSerializer().Serialize(obj);

                try
                {
                    var jString = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    writer.Write(jString);
                }
                catch(Exception ex)
                {
                    writer.Write(ex.ToString());
                }
            }));
        }
    }
}
