using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.IPWhiteList
{
    public class WhiteListSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }

        public Guid SiteConfigurationId { get; set; }

        public WhiteListSettings()
        {

        }

        public WhiteListSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("IPWhiteListSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public WhiteListSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("IPWhiteListSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public WhiteListSettings(Item item)
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

            if (item.HasField("White Listing Enabled"))
            {
                CheckboxField field = (CheckboxField)item.Fields["White Listing Enabled"];
                this.WhiteListingEnabled = field.Checked;
            }

            this.RestrictedAccessPageId = Guid.Empty;
            if (item.HasField("Restricted Access Page") && !string.IsNullOrEmpty(item.Fields["Restricted Access Page"].Value))
            {
                try
                {
                    this.RestrictedAccessPageId = new Guid(item.Fields["Restricted Access Page"].Value);
                }
                catch { }
            }
        }

        #region IP White List Settings

        public bool WhiteListingEnabled { get; set; }
        public Guid RestrictedAccessPageId { get; set; }

        #endregion
    }
}
