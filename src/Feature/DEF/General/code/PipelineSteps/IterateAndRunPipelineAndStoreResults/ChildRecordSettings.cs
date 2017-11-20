using Sitecore.DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.DEF.General
{
    public class ChildRecordSettings : IPlugin
    {
        public ChildRecordSettings()
        {
            Records = new List<Dictionary<string, string>>();
        }

        public List<Dictionary<string, string>> Records { get; set; }
    }
}