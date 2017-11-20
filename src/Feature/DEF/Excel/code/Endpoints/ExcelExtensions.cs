using Sitecore.DataExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Excel
{

    public static class ExcelExtensions
    {
        public static ExcelSettings GetExcelSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<ExcelSettings>();
        }
        public static bool HasExcelSettings(this Endpoint endpoint)
        {
            return (GetExcelSettings(endpoint) != null);
        }
    }
}
