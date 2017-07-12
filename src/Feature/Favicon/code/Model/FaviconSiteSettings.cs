using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Feature.Favicon
{
    public class FaviconSiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public FaviconSiteSettings()
        {

        }

        public FaviconSiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("FaviconSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public FaviconSiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("FaviconSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public FaviconSiteSettings(Item item)
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

            this.Favicon = (FileField)item.Fields[Templates.SiteFaviconSettings.Fields.Favicon];
        }

        public FileField Favicon { get; set; }
    }
}
