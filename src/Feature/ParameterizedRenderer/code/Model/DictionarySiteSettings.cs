using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.ParameterizedRenderer
{
    public class DictionarySiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public DictionarySiteSettings()
        {

        }

        public DictionarySiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("DictionarySiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public DictionarySiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("DictionarySiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public DictionarySiteSettings(Item item)
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


        }

        public string DictionaryDomain
        {
            get
            {
                var siteConfig = Sitecore.Context.Database.GetItem(new ID(this.SiteConfigurationId));

                var domain = siteConfig.Children.FirstOrDefault(a => a.TemplateID == new ID("{0A2847E6-9885-450B-B61E-F9E6528480EF}"));

                if (domain != null)
                {
                    return domain.ID.ToString();
                }

                return null;
            }
        }
    }
}
