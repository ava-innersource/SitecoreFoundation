using Sitecore.DataExchange.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Excel
{
    public class ReadExcelTabSettings : EndpointSettings
    {
        public ReadExcelTabSettings() : base()
        {
        }

        public string Sheet { get; set; }
        public bool FirstRowHasColumnNames { get; set; }
    }
}
