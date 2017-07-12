using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public interface IDBConnectionProvider
    {
        IDbConnection GetConnection(string connectionString);
    }
}