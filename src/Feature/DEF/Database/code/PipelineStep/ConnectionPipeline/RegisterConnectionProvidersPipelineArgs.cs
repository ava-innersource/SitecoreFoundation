using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.DEF.Database
{
    public class RegisterConnectionProvidersPipelineArgs : PipelineArgs
    {
        public RegisterConnectionProvidersPipelineArgs()
        {
            ConnectionProviders = new Dictionary<string, IDBConnectionProvider>();
        }

        //key should be defined in Drop down and should be lower case when registering providers.
        public Dictionary<string, IDBConnectionProvider> ConnectionProviders { get; set; }
    }
}