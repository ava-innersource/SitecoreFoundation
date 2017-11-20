using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class StaticValueReader : IValueReader
    {
        public string Value { get; private set; }
        public StaticValueReader(string value)
        {
            this.Value = value;
        }
        
        public CanReadResult CanRead(object source, DataAccessContext context)
        {
            return new CanReadResult { CanReadValue = true };
        }
        
        public ReadResult Read(object source, DataAccessContext context)
        {
            return new ReadResult(DateTime.UtcNow) { 
                WasValueRead = true,
                ReadValue = this.Value ?? string.Empty
            };
        }
        
        
    }
}
