using Sitecore.DataExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.Database
{

    public static class DatabaseExtensions
    {
        public static DatabaseSettings GetDatabaseSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<DatabaseSettings>();
        }
        public static bool HasDatabaseSettings(this Endpoint endpoint)
        {
            return (GetDatabaseSettings(endpoint) != null);
        }
    }
}
