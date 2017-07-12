using Sitecore.DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public class AuditDictionarySettings : IPlugin
    {
        public string Context { get; set; }

        public bool IsSource
        {
            get
            {
                return this.Context.ToLower() == "pipeline context source";
            }
        }
    }
}