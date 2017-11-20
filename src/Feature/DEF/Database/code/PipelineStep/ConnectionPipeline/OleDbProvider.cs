using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;

namespace SF.Feature.DEF.Database
{
    public class OleDbProvider: IDBConnectionProvider
    {
        public System.Data.IDbConnection GetConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }
    }
}