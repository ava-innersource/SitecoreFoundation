using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Database
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
