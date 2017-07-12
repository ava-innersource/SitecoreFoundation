using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Foundation.Configuration;

namespace SF.Feature.SiteMap
{
    public class ImageSiteMapData
    {
         public ImageSiteMapData() : this(Sitecore.Context.Item)
        {

        }

        public ImageSiteMapData(Item item)
        {
            this.Location = item.GetFullyQualifiedMediaUrl();

            if (item.HasField("Title"))
            {
                this.Title = item.Fields["Title"].Value;
            }

            if (item.HasField("Description"))
            {
                this.Caption = item.Fields["Description"].Value;
            }
        }

        public string Title { get; set; }
        public string Location { get; set; }
        public string Caption { get; set; }
     
        public string ToImageSiteMapXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<image:image>");
            sb.Append(string.Format(@"<image:loc>{0}</image:loc>", this.Location));

            if (!string.IsNullOrEmpty(this.Title))
            {
                sb.Append(string.Format(@"<image:title>{0}</image:title>", this.Title));
            }

            if (!string.IsNullOrEmpty(this.Caption))
            {
                sb.Append(string.Format(@"<image:caption>{0}</image:caption>", this.Caption));
            }

            sb.Append(@"</image:image>");
            return sb.ToString();
        }
    }
}
