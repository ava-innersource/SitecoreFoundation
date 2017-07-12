using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.SitecoreProvider
{
    //Implementation for Convertor Completness. This should never really be used.
    public class StaticValueWriter : IValueWriter
    {
        public string Value { get; private set; }

        public StaticValueWriter(string value)
        {
            this.Value = value;
        }

        public CanWriteResult CanWrite(object target, object value, DataAccessContext context)
        {
            //You really can't write to this thing.
            return new CanWriteResult { CanWriteValue = false };
        }
        
        public bool Write(object target, object value, DataAccessContext context)
        {
            return false;
        }
        
    }
}
