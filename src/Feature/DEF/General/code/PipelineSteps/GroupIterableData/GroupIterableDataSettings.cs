using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    public class GroupIterableDataSettings : EndpointSettings
    {
        public GroupIterableDataSettings()
            : base()
        {
        }

        public string GroupFieldKey { get; set; }
        public bool RemoveIterableSettingsPlugin { get; set; }


    }
}
