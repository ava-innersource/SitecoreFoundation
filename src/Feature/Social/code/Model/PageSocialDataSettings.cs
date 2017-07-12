using Sitecore.Data.Items;
using SF.Foundation.Configuration;

namespace SF.Feature.Social
{
    /// <summary>
    /// Represents all of the Social Data Templates put together (Facebook, Twitter, GPlus).
    /// Can handle if fields are not defined if your item doesn't inherit all of them.
    /// </summary>
    public class PageSocialDataSettings
    {
        public PageSocialDataSettings() : this(Sitecore.Context.Item)
        {

        }

        public PageSocialDataSettings(Item item)
        {
            if (item.HasField(Templates.PageGooglePlusSettings.Fields.GooglePlusAuthorUrl))
            {
                this.GooglePlusAuthorUrl = item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusAuthorUrl].Value;
            }
            if (item.HasField(Templates.PageGooglePlusSettings.Fields.GooglePlusPublisherUrl))
            {
                this.GooglePlusPublisherUrl = item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusPublisherUrl].Value;
            }
            if (item.HasField(Templates.PageGooglePlusSettings.Fields.GooglePlusName))
            {
                this.GooglePlusName = item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusName].Value;
            }
            if (item.HasField(Templates.PageGooglePlusSettings.Fields.GooglePlusDescription))
            {
                this.GooglePlusDescription = item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusDescription].Value;
            }
            if (item.HasField(Templates.PageGooglePlusSettings.Fields.GooglePlusImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusImage]).MediaItem != null)
            {
                this.GooglePlusImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageGooglePlusSettings.Fields.GooglePlusImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }


            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterPublisherHandle))
            {
                this.TwitterPublisherHandle = item.Fields[Templates.PageTwitterSettings.Fields.TwitterPublisherHandle].Value;
                if (!string.IsNullOrEmpty(TwitterPublisherHandle) && !this.TwitterPublisherHandle.StartsWith("@"))
                {
                    this.TwitterPublisherHandle = "@" + this.TwitterPublisherHandle;
                }
            }
            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterTitle))
            {
                this.TwitterTitle = item.Fields[Templates.PageTwitterSettings.Fields.TwitterTitle].Value;
            }
            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterDescription))
            {
                this.TwitterDescription = item.Fields[Templates.PageTwitterSettings.Fields.TwitterDescription].Value;
            }
            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterAuthorHandle))
            {
                this.TwitterAuthorHandle = item.Fields[Templates.PageTwitterSettings.Fields.TwitterAuthorHandle].Value;
                if (!string.IsNullOrEmpty(TwitterAuthorHandle) && !this.TwitterAuthorHandle.StartsWith("@"))
                {
                    this.TwitterAuthorHandle = "@" + this.TwitterAuthorHandle;
                }
            }
            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageTwitterSettings.Fields.TwitterImage]).MediaItem != null)
            {
                this.TwitterImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageTwitterSettings.Fields.TwitterImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }
            if (item.HasField(Templates.PageTwitterSettings.Fields.TwitterCardType))
            {
                this.TwitterCardType = item.Fields[Templates.PageTwitterSettings.Fields.TwitterCardType].Value;
            }





            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphTitle))
            {                
                this.OpenGraphTitle = item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphTitle].Value;
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphType))
            {
                this.OpenGraphType = item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphType].Value;
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphUrl))
            {
                this.OpenGraphUrl = item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphUrl].Value;
                if (!string.IsNullOrEmpty(this.OpenGraphUrl) && this.OpenGraphUrl.Contains("$current"))
                {
                    this.OpenGraphUrl = this.OpenGraphUrl.Replace("$current", item.GetFullyQualifiedUrl());
                }
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphImage) && ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphImage]).MediaItem != null)
            {
                this.OpenGraphImage = ((Sitecore.Data.Fields.ImageField)item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphImage]).MediaItem.GetFullyQualifiedMediaUrl();
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphDescription))
            {
                this.OpenGraphDescription = item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphDescription].Value;
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.OpenGraphSiteName))
            {
                this.OpenGraphSiteName = item.Fields[Templates.PageFacebookSettings.Fields.OpenGraphSiteName].Value;
            }
            if (item.HasField(Templates.PageFacebookSettings.Fields.FacebookNumericId))
            {
                this.FacebookNumericId = item.Fields[Templates.PageFacebookSettings.Fields.FacebookNumericId].Value;
            }
        }

        public string GooglePlusAuthorUrl { get; set; }
        public string GooglePlusPublisherUrl { get; set; }
        public string GooglePlusName { get; set; }
        public string GooglePlusDescription { get; set; }
        public string GooglePlusImage { get; set; }

        public string TwitterPublisherHandle { get; set; }
        public string TwitterTitle { get; set; }
        public string TwitterDescription { get; set; }
        public string TwitterAuthorHandle { get; set; }
        public string TwitterImage { get; set; }
        public string TwitterCardType { get; set; }

        public string OpenGraphTitle { get; set; }
        public string OpenGraphType { get; set; }
        public string OpenGraphUrl { get; set; }
        public string OpenGraphImage { get; set; }
        public string OpenGraphDescription { get; set; }
        public string OpenGraphSiteName { get; set; }
        public string FacebookNumericId { get; set; }

    }
}
