using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Analytics
{
    public class PageEventDetails
    {
        public string name { get; set; }
        public string pageId { get; set; }

        public string data { get; set; }

        public string dataKey { get; set; }
        public string text { get; set; }

        public string pageEventDefinitionId { get; set; }

    }
}