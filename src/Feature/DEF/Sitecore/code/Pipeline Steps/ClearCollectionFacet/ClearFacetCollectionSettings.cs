using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class ClearFacetCollectionSettings : EndpointSettings
    {
        public ClearFacetCollectionSettings()
            : base()
        {
        }

        public string FacetName { get; set; }

        public string CollectionMemberName { get; set; }
    }
}
