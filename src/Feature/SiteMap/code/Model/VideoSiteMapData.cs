using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.SiteMap
{
    public class VideoSiteMapData
    {
        public VideoSiteMapData() : this(Sitecore.Context.Item)
        {

        }

        public VideoSiteMapData(Item item)
        {
            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.VideoTitle))
            {
                this.Title= item.Fields[Templates.PageVideoSiteMapSettings.Fields.VideoTitle].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.VideoDescription))
            {
                this.Description= item.Fields[Templates.PageVideoSiteMapSettings.Fields.VideoDescription].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.VideoCategory))
            {
                this.Category= item.Fields[Templates.PageVideoSiteMapSettings.Fields.VideoCategory].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.ContentLocation) && !string.IsNullOrEmpty(item.Fields[Templates.PageVideoSiteMapSettings.Fields.ContentLocation].Value))
            {
                var lnkFld = (LinkField) item.Fields[Templates.PageVideoSiteMapSettings.Fields.ContentLocation];
                this.ContentLocation = EnsureUrlIsQualified(lnkFld.LinkUrl());
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.PlayerLocation) && !string.IsNullOrEmpty(item.Fields[Templates.PageVideoSiteMapSettings.Fields.PlayerLocation].Value))
            {
                var lnkFld = (LinkField)item.Fields[Templates.PageVideoSiteMapSettings.Fields.PlayerLocation];
                this.PlayerLocation = EnsureUrlIsQualified(lnkFld.LinkUrl());
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.ThumbnailLocation))
            {
                var imgFld = (ImageField)item.Fields[Templates.PageVideoSiteMapSettings.Fields.ThumbnailLocation];
                this.ThumbnailLocation = imgFld.MediaItem.GetFullyQualifiedMediaUrl();
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.GalleryLocation) && !string.IsNullOrEmpty(item.Fields[Templates.PageVideoSiteMapSettings.Fields.GalleryLocation].Value))
            {
                var lnkFld = (LinkField)item.Fields[Templates.PageVideoSiteMapSettings.Fields.GalleryLocation];
                this.GalleryLocation = EnsureUrlIsQualified(lnkFld.LinkUrl());
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Duration))
            {
                int intVal;
                if (int.TryParse(item.Fields[Templates.PageVideoSiteMapSettings.Fields.Duration].Value, out intVal))
                {
                    this.Duration = intVal;
                }                
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Rating))
            {
                this.Rating = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Rating].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.ViewCount))
            {
                int intVal;
                if (int.TryParse(item.Fields[Templates.PageVideoSiteMapSettings.Fields.ViewCount].Value, out intVal))
                {
                    this.ViewCount = intVal;
                }
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.PublicationDate))
            {
                DateTime dateVal;
                if (DateTime.TryParse(item.Fields[Templates.PageVideoSiteMapSettings.Fields.PublicationDate].Value, out dateVal))
                {
                    this.PublicationDate = dateVal;
                }
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.ExpirationDate))
            {
                DateTime dateVal;
                if (DateTime.TryParse(item.Fields[Templates.PageVideoSiteMapSettings.Fields.ExpirationDate].Value, out dateVal))
                {
                    this.ExpirationDate = dateVal;
                }
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.FamilyFriendly))
            {
                this.FamilyFriendly = item.Fields[Templates.PageVideoSiteMapSettings.Fields.FamilyFriendly].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.VideoRestriction))
            {
                this.VideoRestriction = item.Fields[Templates.PageVideoSiteMapSettings.Fields.VideoRestriction].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Price))
            {
                this.Price = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Price].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Currency))
            {
                this.Currency = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Currency].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.RequiresSubscription))
            {
                var chkFld = (CheckboxField)item.Fields[Templates.PageVideoSiteMapSettings.Fields.RequiresSubscription];
                this.RequiresSubscription = chkFld.Checked;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Uploader))
            {
                this.Uploader = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Uploader].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Platform))
            {
                this.Platform = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Platform].Value;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.LiveStream))
            {
                var chkFld = (CheckboxField)item.Fields[Templates.PageVideoSiteMapSettings.Fields.LiveStream];
                this.LiveStream = chkFld.Checked;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.Tags))
            {
                this.Tags = item.Fields[Templates.PageVideoSiteMapSettings.Fields.Tags].Value;
            }

        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public string ContentLocation { get; set; }

        public string PlayerLocation { get; set; }

        public string ThumbnailLocation { get; set; }

        public string GalleryLocation { get; set; }

        public int Duration {get;set;}

        public string Rating { get; set; }

        public int ViewCount { get; set; }

        public DateTime PublicationDate {get;set;}

        public DateTime ExpirationDate {get;set;}

        public string FamilyFriendly { get; set; }

        public string VideoRestriction { get; set; }

        public string Price { get; set; }

        public string Currency { get; set; }

        public bool RequiresSubscription {get;set;}

        public string Uploader {get;set;}

        public string Platform {get;set;}

        public bool LiveStream {get;set; }

        public string Tags {get;set;}

        public string ToVideoSitemapXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<video>");

            if (!string.IsNullOrEmpty(this.ThumbnailLocation))
            {
                sb.Append(string.Format(@"<video:thumbnail_loc>{0}</video:thumbnail_loc>", this.ThumbnailLocation));
            }

            if (!string.IsNullOrEmpty(this.Title))
            {
                sb.Append(string.Format(@"<video:title>{0}</video:title>", this.Title));
            }

            if (!string.IsNullOrEmpty(this.Description))
            {
                sb.Append(string.Format(@"<video:description>{0}</video:description>", this.Description));
            }

            if (!string.IsNullOrEmpty(this.ContentLocation))
            {
                sb.Append(string.Format(@"<video:content_loc>{0}</video:content_loc>", this.ContentLocation));
            }

            if (!string.IsNullOrEmpty(this.PlayerLocation))
            {
                sb.Append(string.Format(@"<video:player_loc>{0}</video:player_loc>", this.PlayerLocation));
            }

            if (this.Duration > 0)
            {
                sb.Append(string.Format(@"<video:duration>{0}</video:duration>", this.Duration));
            }

            if (!string.IsNullOrEmpty(this.Rating))
            {
                sb.Append(string.Format(@"<video:rating>{0}</video:rating>", this.Rating));
            }

            if (this.ViewCount > 0)
            {
                sb.Append(string.Format(@"<video:view_count>{0}</video:view_count>", this.ViewCount));
            }

            if (this.PublicationDate != DateTime.MinValue && this.PublicationDate != DateTime.MaxValue)
            {
                sb.Append(string.Format(@"<video:publication_date>{0}</video:publication_date>", this.PublicationDate.ToString("YYYY-MM-DD")));
            }

            if (!string.IsNullOrEmpty(this.FamilyFriendly))
            {
                sb.Append(string.Format(@"<video:family_friendly>{0}</video:family_friendly>", this.FamilyFriendly));
            }

            if (!string.IsNullOrEmpty(this.Tags))
            {
                foreach(var tag in this.Tags.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    sb.Append(string.Format(@"<video:tag>{0}</video:tag>", tag));
                }                
            }

            if (!string.IsNullOrEmpty(this.Category))
            {
                sb.Append(string.Format(@"<video:category>{0}</video:category>", this.Category));
            }

            if (!string.IsNullOrEmpty(this.VideoRestriction))
            {
                sb.Append(string.Format(@"<video:restriction relationship=""allow"">{0}</video:restriction>", this.VideoRestriction));
            }

            if (!string.IsNullOrEmpty(this.GalleryLocation))
            {
                sb.Append(string.Format(@"<video:gallery_loc>{0}</video:gallery_loc>", this.GalleryLocation));
            }

            if (!string.IsNullOrEmpty(this.Price) && !string.IsNullOrEmpty(this.Currency))
            {
                sb.Append(string.Format(@"<video:price currency=""{0}"">{1}</video:price>", this.Currency, this.Price));
            }

            if (this.RequiresSubscription)
            {
                sb.Append(@"<video:requires_subscription>yes</video:requires_subscription>");
            }

            if (!string.IsNullOrEmpty(this.Uploader))
            {
                sb.Append(string.Format(@"<video:uploader>{0}</video:uploader>", this.Uploader));
            }

            if (!string.IsNullOrEmpty(this.Platform))
            {
                sb.Append(string.Format(@"<video:platform relationship=""allow"">{0}</video:platform>", this.Platform));
            }

            if (this.LiveStream)
            {
                sb.Append(@"<video:live>yes</video:live>");
            }

            sb.Append("</video>");
            return sb.ToString();
        }

        private string EnsureUrlIsQualified(string url)
        { 
            if (url.StartsWith("http"))
            {
                return url;
            }
            Uri uri = new Uri(System.Web.HttpContext.Current.Request.Url, url);
            return uri.ToString();
        }
    }
}
