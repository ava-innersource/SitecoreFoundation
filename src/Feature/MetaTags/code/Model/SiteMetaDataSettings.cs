using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.MetaTags

{
    public class SiteMetaDataSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public SiteMetaDataSettings()
        {

        }

        public SiteMetaDataSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public SiteMetaDataSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public SiteMetaDataSettings(Item item)
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

            if (item.HasField(Templates.SiteMetaTagSettings.Fields.TitlePrefix))
            {
                this.TitlePrefix = item.Fields[Templates.SiteMetaTagSettings.Fields.TitlePrefix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.TitleSuffix))
            {
                this.TitleSuffix = item.Fields[Templates.SiteMetaTagSettings.Fields.TitleSuffix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.MetaDescPrefix))
            {
                this.MetaDescPrefix = item.Fields[Templates.SiteMetaTagSettings.Fields.MetaDescPrefix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.MetaDescSuffix))
            {
                this.MetaDescSuffix = item.Fields[Templates.SiteMetaTagSettings.Fields.MetaDescSuffix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.MetaKeywordsPrefix))
            {
                this.MetaKeywordsPrefix = item.Fields[Templates.SiteMetaTagSettings.Fields.MetaKeywordsPrefix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.MetaKeywordsSuffix))
            {
                this.MetaKeywordsSuffix = item.Fields[Templates.SiteMetaTagSettings.Fields.MetaKeywordsSuffix].Value;
            }
            if (item.HasField(Templates.SiteMetaTagSettings.Fields.BaseCanoncialUrl))
            {
                this.BaseCanoncialUrl = item.Fields[Templates.SiteMetaTagSettings.Fields.BaseCanoncialUrl].Value;
            }
        }

        #region meta tag properties

        public string TitlePrefix { get; set; }
        public string TitleSuffix { get; set; }
        public string MetaDescPrefix { get; set; }
        public string MetaDescSuffix { get; set; }
        public string MetaKeywordsPrefix { get; set; }
        public string MetaKeywordsSuffix { get; set; }
        public string BaseCanoncialUrl { get; set; }

        #endregion

    }
}
