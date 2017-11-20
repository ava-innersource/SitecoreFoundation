using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Database
{
    public class DBConnectionFactory
    {
        public static IDbConnection GetConnection(string type, string connectionString)
        {
            IDbConnection conn = null;

            var args = new RegisterConnectionProvidersPipelineArgs();
            
            //Get all the available providers. Done this way so Oracle Provider can be added by other teams.
            Sitecore.Pipelines.CorePipeline.Run("registerConnectionProviders", args);

            if (args.ConnectionProviders.ContainsKey(type.ToLower()))
            {
                var provider = args.ConnectionProviders[type.ToLower()];
                conn = provider.GetConnection(connectionString);
            }
                
            return conn;
        }
    }
}
