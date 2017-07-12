using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.Database
{
    public class QuerySettings : EndpointSettings
    {
        public QuerySettings() : base()
        {
        }

        public string Query { get; set; }
    }
}
