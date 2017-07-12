using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Feature.Social
{
    public class SiteSocialDataSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public SiteSocialDataSettings()
        { }

        public SiteSocialDataSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("SocialDataSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public SiteSocialDataSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("SocialDataSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public SiteSocialDataSettings(Item item)
        {
            Load(item);
        }

        public void Load(Item item)
        {
            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
            this.ConfigItem = item;
            if (item != null)
            {
                this.SiteConfigurationId = item.ID.Guid;
            }

            if (item.HasField(Templates.SiteFacebookSettings.Fields.OpenGraphSiteName))
            {
                this.OpenGraphSiteName = item.Fields[Templates.SiteFacebookSettings.Fields.OpenGraphSiteName].Value;
            }
            if (item.HasField(Templates.SiteFacebookSettings.Fields.FacebookNumericId))
            {
                this.FacebookNumericId = item.Fields[Templates.SiteFacebookSettings.Fields.FacebookNumericId].Value;
            }
            if (item.HasField(Templates.SiteFacebookSettings.Fields.OpenGraphImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteFacebookSettings.Fields.OpenGraphImage]).MediaItem != null)
            {
                this.OpenGraphImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteFacebookSettings.Fields.OpenGraphImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }
            if (item.HasField(Templates.SiteFacebookSettings.Fields.OpenGraphType))
            {
                this.OpenGraphType = item.Fields[Templates.SiteFacebookSettings.Fields.OpenGraphType].Value;
            }


            if (item.HasField(Templates.SiteTwitterSettings.Fields.TwitterAuthorHandle))
            {
                this.TwitterAuthorHandle = item.Fields[Templates.SiteTwitterSettings.Fields.TwitterAuthorHandle].Value;
                if (!string.IsNullOrEmpty(TwitterAuthorHandle) && !this.TwitterAuthorHandle.StartsWith("@"))
                {
                    this.TwitterAuthorHandle = "@" + this.TwitterAuthorHandle;
                }
            }
            if (item.HasField(Templates.SiteTwitterSettings.Fields.TwitterPublisherHandle))
            {
                this.TwitterPublisherHandle = item.Fields[Templates.SiteTwitterSettings.Fields.TwitterPublisherHandle].Value;
                if (!string.IsNullOrEmpty(TwitterPublisherHandle) && !this.TwitterPublisherHandle.StartsWith("@"))
                {
                    this.TwitterPublisherHandle = "@" + this.TwitterPublisherHandle;
                }
            }
            if (item.HasField(Templates.SiteTwitterSettings.Fields.TwitterImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteTwitterSettings.Fields.TwitterImage]).MediaItem != null)
            {
                this.TwitterImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteTwitterSettings.Fields.TwitterImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }

            if (item.HasField(Templates.SiteGooglePlusSettings.Fields.GooglePlusAuthorUrl))
            {
                this.GooglePlusAuthorUrl = item.Fields[Templates.SiteGooglePlusSettings.Fields.GooglePlusAuthorUrl].Value;
            }
            if (item.HasField(Templates.SiteGooglePlusSettings.Fields.GooglePlusPublisherUrl))
            {
                this.GooglePlusPublisherUrl = item.Fields[Templates.SiteGooglePlusSettings.Fields.GooglePlusPublisherUrl].Value;
            }
            if (item.HasField(Templates.SiteGooglePlusSettings.Fields.GooglePlusImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteGooglePlusSettings.Fields.GooglePlusImage]).MediaItem != null)
            {
                this.GooglePlusImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.SiteGooglePlusSettings.Fields.GooglePlusImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }
        }

        #region SocialDefaults

        public string GooglePlusAuthorUrl { get; set; }
        public string GooglePlusPublisherUrl { get; set; }
        public string GooglePlusImage { get; set; }

        public string TwitterPublisherHandle { get; set; }
        public string TwitterAuthorHandle { get; set; }
        public string TwitterImage { get; set; }

        public string OpenGraphType { get; set; }
        public string OpenGraphImage { get; set; }
        public string OpenGraphSiteName { get; set; }
        public string FacebookNumericId { get; set; }

        #endregion
    }
}
