using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Multisite;
using System;
using Microsoft.Extensions.DependencyInjection;
using Sitecore;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Globalization;

namespace SF.Foundation.Configuration
{
    /// <summary>
    /// Represents the Multi Site Context Configuration Data Template
    /// </summary>
    public class MultiSiteContext
    {
        #region constructors
        public Item ConfigItem { get; set; }
        private const string SiteConfigRootKey = "Foundation.ConfigRoot";
        private const string DefaultContextLanguage = "en";

        public MultiSiteContext()
        {
            SetConfigItem();
        }

        private void SetConfigItem()
        {
            //Override Config Item if Site has it set as a context property
            var contextSite = SiteExtensions.GetContextSite();
            string configRootId = contextSite.Properties[SiteConfigRootKey];

            if (!string.IsNullOrWhiteSpace(configRootId))
            {
                var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
                var configRoot = db.GetItem(new ID(configRootId));
                if (configRoot != null)
                {
                    ConfigItem = configRoot;
                    return;
                }
            }

            var settingsItem = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetSettingsItem(Context.Database.GetItem(Context.Site.StartPath));
            if (settingsItem != null)
            {
                var rootFolder = settingsItem.FirstChildInheritingFrom(Templates.SiteSettingsRootFolder.ID);
                if (rootFolder != null)
                {
                    ConfigItem = rootFolder;
                }
            }
        }



        #endregion

        public Guid SiteId { get; set; }

        public string this[string key]
        {
            get
            {
                using (new LanguageSwitcher(DefaultContextLanguage))
                {
                    //TODO: move to Lucene Query.
                    Item item = this.ConfigItem.Axes.SelectSingleItem("descendant-or-self::*[@key='" + key + "']");
                    if (item != null && item.HasField(Templates.SiteSetting.Fields.Value))
                    {
                        return item.Fields[Templates.SiteSetting.Fields.Value].Value;
                    }
                }
                return null;
            }
        }

        
    }
}
