using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class HandlebarHelperRegistration
    {
        public HandlebarHelperRegistration()
        {

        }

        public HandlebarHelperRegistration(string name, HandlebarsHelper helper)
        {
            this.Name = name;
            this.Helper = helper;
        }

        public HandlebarHelperRegistration(string name, HandlebarsBlockHelper helper)
        {
            this.Name = name;
            this.BlockHelper = helper;
        }

        public string Name { get; set; }

        public HandlebarsHelper Helper { get; set; }

        public HandlebarsBlockHelper BlockHelper { get; set; }

    }
}
