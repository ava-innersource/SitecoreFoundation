using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.TraceLogger
{
    public class FlushTraceLoggerProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            TraceLogger.Current.Flush();
        }
    }
}
