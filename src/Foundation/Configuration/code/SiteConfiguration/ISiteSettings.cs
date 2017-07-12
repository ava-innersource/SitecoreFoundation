using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Configuration
{
    public interface ISiteSettings
    {
        void Load(Item item);
    }
}
