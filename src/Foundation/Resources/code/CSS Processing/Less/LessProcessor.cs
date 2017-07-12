using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Resources
{ 
    public class LessProcessor : ICSSProcessor
    {
        public string Process(string input)
        {
            string output = input;
            try
            {
                //Parse for Less Stuff
                output = dotless.Core.Less.Parse(input, new dotless.Core.configuration.DotlessConfiguration
                {
                    Logger = typeof(LessLogger),
                    // change to an appropriate log level for your needs
                    LogLevel = dotless.Core.Loggers.LogLevel.Info
                });

                // if less fails to parse CSS, it will return empty string,
                // so check logger error messages
                if (string.IsNullOrWhiteSpace(output))
                {
                    if (HttpContext.Current.Items.Contains(LessLogger.LESS_LOGGER_KEY))
                    {
                        output += string.Format(@"/* Error Compiling Less: {0} */", HttpContext.Current.Items[LessLogger.LESS_LOGGER_KEY].ToString());
                    }
                }

                if (HttpContext.Current.Items.Contains(LessLogger.LESS_LOGGER_KEY))
                {
                    HttpContext.Current.Items.Remove(LessLogger.LESS_LOGGER_KEY);
                }
            }
            catch (Exception ex)
            {
                output += string.Format(@"/* Error Compiling Less: {0} */", ex.ToString());
            }

            return output;
        }
    }
}
