using Sitecore.DataExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.RSS
{
    public static class RssExtensions
    {
        public static RssSettings GetRssSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<RssSettings>();
        }
        public static bool HasRssSettings(this Endpoint endpoint)
        {
            return (GetRssSettings(endpoint) != null);
        }
    }
}
