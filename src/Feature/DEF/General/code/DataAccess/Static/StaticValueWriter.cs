using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.General
{
    //Implementation for Convertor Completness. This should never really be used.
    public class StaticValueWriter : IValueWriter
    {
        public string Value { get; private set; }

        public StaticValueWriter(string value)
        {
            this.Value = value;
        }

        public bool Write(object target, object value, DataAccessContext context)
        {
            return false;
        }
        
    }
}
