using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MultiSiteContext(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            ProccessItem(item);
        }

        public MultiSiteContext(string path)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            ProccessItem(item);
        }

        public MultiSiteContext(Item item)
        {
            ProccessItem(item);
        }

        private void ProccessItem(Item item)
        {
            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            if (Sitecore.Diagnostics.Log.IsDebugEnabled)
            {
                if (item != null && item.Fields != null)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < item.Fields.Count; i++)
                    {
                        sb.Append(string.Format("Item name: {0}, Field name: {1}, Field key: {2}, Field ID: {3}\n", item.Name, item.Fields[i].Name, item.Fields[i].Key, item.Fields[i].ID.Guid.ToString()));
                    }

                    if (sb.Length > 0)
                    {
                        Sitecore.Diagnostics.Log.Debug("Item fields\n" + sb.ToString(), this);
                    }
                }
            }

            ConfigItem = item;

            if (item != null)
            {
                this.SiteId = item.ID.Guid;
            }

            //Override Config Item if Site has it set as a context property
            var contextSite = SiteExtensions.GetContextSite();
            string configRootId = contextSite.Properties[SiteConfigRootKey];
            if (!string.IsNullOrWhiteSpace(configRootId))
            {
                var configRoot = db.GetItem(new ID(configRootId));
                if (configRoot != null)
                {
                    ConfigItem = configRoot;
                }
            }
            
        }



        #endregion

        public Guid SiteId { get; set; }

        public string this[string key]
        {
            get
            {
                Item item = this.ConfigItem.Axes.SelectSingleItem("descendant-or-self::*[@key='" + key + "']");
                if (item != null && item.HasField(Templates.SiteSetting.Fields.Value))
                {
                    return item.Fields[Templates.SiteSetting.Fields.Value].Value;
                }
                return null;
            }
        }

        
    }
}
