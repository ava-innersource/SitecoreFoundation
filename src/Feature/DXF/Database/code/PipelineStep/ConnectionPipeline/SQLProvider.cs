using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public class SQLProvider : IDBConnectionProvider
    {
        public System.Data.IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}