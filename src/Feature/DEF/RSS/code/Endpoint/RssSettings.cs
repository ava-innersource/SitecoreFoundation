using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.RSS
{
    public class RssSettings : Sitecore.DataExchange.IPlugin
    {
        public RssSettings()
        {

        }

        public string FeedUrl { get; set; }
    }
}
