using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    public class DictionaryValueWriter : DictionaryValueWriter<string, string>
    {
        public DictionaryValueWriter(string keyValue) : base(keyValue)
        {
        }

    }
}
