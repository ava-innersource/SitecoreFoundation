using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Excel
{
    public class ExcelSettings : Sitecore.DataExchange.IPlugin
    {
        public ExcelSettings()
        {
        }

        public string FileLocation { get; set; }
        
    }
}
