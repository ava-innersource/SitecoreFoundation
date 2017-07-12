using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Handlebars
{
    public class HandlebarHelpersPipelineArgs : PipelineArgs
    {
        public HandlebarHelpersPipelineArgs()
        {
            this.Helpers = new List<HandlebarHelperRegistration>();
        }
        public List<HandlebarHelperRegistration> Helpers { get; set; }
    }
}
