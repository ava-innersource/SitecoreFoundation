using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.Database
{
    public class DatabaseSettings : Sitecore.DataExchange.IPlugin
    {
        public DatabaseSettings()
        {
        }

        public string ConnectionType { get; set; }
        public string ConnectionString { get; set; }
        
    }
}
