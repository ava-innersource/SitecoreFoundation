using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.SitecoreProvider
{
    public class EnrollContactInEngagementPlanSettings : EndpointSettings
    {
        public EnrollContactInEngagementPlanSettings()
            : base()
        {
        }

        public string EngagementPlanStateID { get; set; }
    }
}
