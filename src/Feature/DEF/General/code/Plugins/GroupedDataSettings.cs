using Sitecore.DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.DEF.General
{ 
    public class GroupedDataSettings : IPlugin
    {
        public GroupedDataSettings()
        {
            Data = new Dictionary<string, List<Dictionary<string, string>>>(StringComparer.OrdinalIgnoreCase);
        }

        public Dictionary<string, List<Dictionary<string, string>>> Data { get; set; }
    }
}