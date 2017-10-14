using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;


namespace SF.Feature.Language
{
    public class CustomLanguageStrategyFallbackItemProvider : ItemProvider
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

            //Try Default Language
            var defaultLang = Sitecore.Configuration.Settings.GetSetting("SF.LanguageStrategy.DefaultLanguage");
            if (string.IsNullOrEmpty(defaultLang))
            {
                return item;
            }

            var fallbackLanguage = LanguageManager.GetLanguage(defaultLang);
            if (fallbackLanguage == null)
            {
                return item;
            }

            Item fallback = base.GetItem(itemId, fallbackLanguage, Version.Latest, database);
            if (fallback != null && fallback.Versions.GetVersionNumbers().Length > 0)
            {
                var stubData = new ItemData(fallback.InnerData.Definition, fallback.Language, fallback.Version, fallback.InnerData.Fields);
                var stub = new LanguageStub(itemId, stubData, database) { OriginalLanguage = item.Language };
                stub.RuntimeSettings.SaveAll = true;

                return stub;    
            }

            //Item doesn't exist in default language, Let's take any other version.
            var installedLanguages = LanguageManager.GetLanguages(database);
            //Assume this will be in same order of languages as defined in System Languages
            foreach (var installedLanguage in installedLanguages)
            {
                fallback = base.GetItem(itemId, installedLanguage, Version.Latest, database);

                if (fallback != null && fallback.Versions.GetVersionNumbers().Length > 0)
                {
                    var stubData = new ItemData(fallback.InnerData.Definition, fallback.Language, fallback.Version, fallback.InnerData.Fields);
                    var stub = new LanguageStub(itemId, stubData, database) { OriginalLanguage = item.Language };
                    stub.RuntimeSettings.SaveAll = true;

                    return stub;
                }
            }

            //Nothing Worked return the null item
            return item;
        }
    }
}
