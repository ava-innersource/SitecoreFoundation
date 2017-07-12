using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Resources
{
    public class ClearResourceCacheOnPublish 
    {
        public void ClearResourceCache(object sender, EventArgs args)
        {
            var cacheEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnumerator.MoveNext())
                HttpRuntime.Cache.Remove(cacheEnumerator.Key.ToString());
        }
    }
    
 
}
