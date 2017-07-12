using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.SiteMap
{
    public class SitemapSiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public SitemapSiteSettings()
        {

        }

        public SitemapSiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("SitemapSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public SitemapSiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("SitemapSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public SitemapSiteSettings(Item item)
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

            this.SitemapRootId = Guid.Empty;
            if (item.HasField(Templates.SiteSiteMapSettings.Fields.SitemapRootId) && !string.IsNullOrEmpty(item.Fields[Templates.SiteSiteMapSettings.Fields.SitemapRootId].Value))
            {
                this.SitemapRootId = new Guid(item.Fields[Templates.SiteSiteMapSettings.Fields.SitemapRootId].Value);
            }

            if (item.HasField(Templates.SiteSiteMapSettings.Fields.SitemapDefaultChangeFrequency))
            {
                this.SitemapDefaultChangeFrequency = item.Fields[Templates.SiteSiteMapSettings.Fields.SitemapDefaultChangeFrequency].Value;

            }
            if (item.HasField(Templates.SiteSiteMapSettings.Fields.SitemapDefaultPriority))
            {
                this.SitemapDefaultPriority = item.Fields[Templates.SiteSiteMapSettings.Fields.SitemapDefaultPriority].Value;
            }
        }


        #region Sitemap properties

        public Guid SitemapRootId { get; set; }
        public string SitemapDefaultChangeFrequency { get; set; }
        public string SitemapDefaultPriority { get; set; }

        #endregion
    }
}
