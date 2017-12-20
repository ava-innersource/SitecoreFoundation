using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    public class DictionaryValueReader : DictionaryValueReader<string, string>
    {

        public DictionaryValueReader(string keyValue) : base(keyValue)
        {
        }

    }
}
