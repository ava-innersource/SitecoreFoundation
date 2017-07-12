using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class RegisterFormatDateHelper
    {
        public void Process(HandlebarHelpersPipelineArgs pipelineArgs)
        {
            pipelineArgs.Helpers.Add(new HandlebarHelperRegistration("formatDate", (writer, context, args) =>
            {
                string input = args[0].ToString();
                DateTime parsed = DateTime.MinValue;
                if (DateTime.TryParse(input, out parsed))
                {
                    //We got a date, woo hoo!
                }                
                else
                {
                    try
                    {
                        //Sitecore Date Vals are stored as Iso, could that be the issue?
                        parsed = Sitecore.DateUtil.IsoDateToDateTime(input);
                    }
                    catch
                    {
                        writer.Write("[Invalid Date]");
                    }
                }

                if (parsed != DateTime.MinValue)
                {
                    try
                    {
                        writer.Write(parsed.ToString(args[1].ToString()));
                    }
                    catch
                    {
                        writer.Write("[Invalid Format!]");
                    }
                }

            }));
        }
    }
}
