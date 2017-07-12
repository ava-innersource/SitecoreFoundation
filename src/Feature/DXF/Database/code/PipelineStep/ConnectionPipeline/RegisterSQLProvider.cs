using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public class RegisterSQLProvider
    {
        public void Process(RegisterConnectionProvidersPipelineArgs args)
        {
            args.ConnectionProviders.Add("sql", new SQLProvider());
        }
    }
}