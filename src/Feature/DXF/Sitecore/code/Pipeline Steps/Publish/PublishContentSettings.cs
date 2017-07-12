using Sitecore.Collections;
using Sitecore.DataExchange;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.SitecoreProvider
{
    public class PublishContentSettings : IPlugin
    {
        public string RootItem { get; set; }

        public bool RelatedItems { get; set; }
        
        public bool ChildItems { get; set; }
        
        public LanguageCollection Languages { get; set; }

        public string Target { get; set; }

        
    }
}