using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using SF.Foundation.Configuration;

namespace SF.Feature.Language
{
    public class LanguageSiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }

        public Guid SiteConfigurationId { get; set; }

        public LanguageSiteSettings()
        {

        }

        public LanguageSiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("LanguageSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public LanguageSiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("LanguageSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public LanguageSiteSettings(Item item)
        {
            Load(item);
        }

        public void Load(Item item)
        {
            var db = item.Database;
            this.ConfigItem = item;
            if (item != null)
            {
                this.SiteConfigurationId = item.ID.Guid;
            }

            if (item.HasField(Templates.SiteLanguageSettings.Fields.ValidLanguages))
            {
                this.ValidLanguages = new List<Sitecore.Globalization.Language>();

                MultilistField field = (MultilistField)item.Fields[Templates.SiteLanguageSettings.Fields.ValidLanguages];
                foreach (var id in field.TargetIDs)
                {
                    try
                    {
                        Item langItem = db.GetItem(id);
                        if (langItem.HasField("Regional Iso Code"))
                        {
                            var isoCode = langItem.Fields["Regional Iso Code"].Value;
                            if (string.IsNullOrEmpty(isoCode))
                            {
                                if (langItem.HasField("Iso"))
                                {
                                    isoCode = langItem.Fields["Iso"].Value;
                                }
                            }
                            var culture = System.Globalization.CultureInfo.GetCultureInfo(isoCode);
                            var lang = db.Languages.Where(a => a.CultureInfo.Equals(culture)).FirstOrDefault();
                            if (lang != null && this.ValidLanguages.Where(a => a.CultureInfo == lang.CultureInfo).FirstOrDefault() == null)
                            {
                                this.ValidLanguages.Add(lang);
                            }

                        }


                    }
                    catch
                    {
                        //Something went wrong, let's not crash things.
                    }
                }
            }
        }

        #region Language Settings

        public List<Sitecore.Globalization.Language> ValidLanguages { get; set; }

        #endregion

    }
}
