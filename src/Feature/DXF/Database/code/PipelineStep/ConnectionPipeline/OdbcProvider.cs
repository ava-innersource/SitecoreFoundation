using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public class OdbcProvider: IDBConnectionProvider
    {
        public System.Data.IDbConnection GetConnection(string connectionString)
        {
            return new OdbcConnection(connectionString);
        }
    }
}