using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.File
{
    public class FileSystemSettings : Sitecore.DataExchange.IPlugin
    {
        public FileSystemSettings()
        {
        }
        public bool ColumnHeadersInFirstLine { get; set; }
        public string ColumnSeparator { get; set; }
        public string Path { get; set; }
    }
}
