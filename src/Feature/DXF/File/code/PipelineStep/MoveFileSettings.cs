using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.File
{
    public class MoveFileSettings : EndpointSettings
    {
        public MoveFileSettings()
        {

        }

        public string DestinationDirectory { get; set; }
        public bool Copy { get; set; }
        public bool AppendTimeStamp { get; set; }
    }
}
