using Sitecore.DataExchange.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    public class DictionaryValueWriter : IValueWriter
    {
        public string Key { get; private set; }
        
        public DictionaryValueWriter(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException("key", key, "Key must be specified.");
            }
            this.Key = key;
        }

        public CanWriteResult CanWrite(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            bool flag = false;
            if (target != null)
            {
                IDictionary<string, string> dictionary = target as IDictionary<string, string>;
                if (dictionary != null)
                {
                    flag = true;
                }
            }
            return new CanWriteResult { CanWriteValue = flag };
        }
        
        public bool Write(object target, object value, DataAccessContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (!this.CanWrite(target, value, context).CanWriteValue)
            {
                return false;
            }

            IDictionary<string, string> dictionary = target as IDictionary<string, string>;
            if (dictionary != null)
            {
                if (dictionary.ContainsKey(this.Key))
                {
                    dictionary[this.Key] = value.ToString();
                }
                else
                {
                    dictionary.Add(this.Key, value.ToString());
                }
            }

            return true;
        }
        
    }
}
