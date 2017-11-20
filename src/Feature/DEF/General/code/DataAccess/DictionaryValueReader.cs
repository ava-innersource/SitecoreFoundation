using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    public class DictionaryValueReader : IValueReader
    {
        public string Key { get; private set; }
        public DictionaryValueReader(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException("key", key, "Key must be specified.");
            }
            this.Key = key;
        }
        
        public CanReadResult CanRead(object source, DataAccessContext context)
        {
            bool flag = false;
            if (source != null)
            {
                IDictionary<string, string> dictionary = source as IDictionary<string, string>;
                if (dictionary != null)
                {
                    flag = true;
                }
            }
            return new CanReadResult { CanReadValue = flag };
        }
        
        public ReadResult Read(object source, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            bool flag = false;
            string val = null;
            
            IDictionary<string, string> dictionary = source as IDictionary<string, string>;
            if (dictionary != null)
            {
                if (dictionary.ContainsKey(this.Key))
                {
                    val = dictionary[this.Key];
                    flag = true;
                }
            }

            return new ReadResult(DateTime.UtcNow) { 
                WasValueRead = flag,
                ReadValue = val
            };
        }
        
        
    }
}
