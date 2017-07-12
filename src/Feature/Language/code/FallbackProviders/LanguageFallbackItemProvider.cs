using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.Language
{
    public class LanguageFallbackItemProvider : ItemProvider
    {
        protected override Item GetItem(ID itemId, Sitecore.Globalization.Language language, Version version, Database database)
        {
            var item = base.GetItem(itemId, language, version, database);

            if (item == null)
            {
                return item;
            }

            if (item.Versions.GetVersionNumbers().Length > 0)
            {
                return item;
            }

            var fallbackLanguage = Sitecore.Context.Site.GetFallbackLanguage();

            if (fallbackLanguage == null)
            {
                return item;
            }

            Item fallback = base.GetItem(itemId, fallbackLanguage, Version.Latest, database);

            if (fallback == null)
            {
                return item;
            }

            if (fallback.Versions.GetVersionNumbers().Length == 0)
            {
                return item;
            }
            
            var stubData = new ItemData(fallback.InnerData.Definition, item.Language, item.Version, fallback.InnerData.Fields);
            var stub = new LanguageStub(itemId, stubData, database) { OriginalLanguage = item.Language };
            stub.RuntimeSettings.SaveAll = true;

            return stub;

        }
    }
}
