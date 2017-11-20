using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class MapChildRecordsToFacetCollectionSettings : EndpointSettings
    {
        public MapChildRecordsToFacetCollectionSettings()
            : base()
        {
        }

        public string FacetName { get; set; }

        public string CollectionMemberName { get; set; }

        public bool RemoveChildRecordsWhenComplete { get; set; }
    }
}
