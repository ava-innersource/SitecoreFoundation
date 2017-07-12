using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using SF.Foundation.Configuration;

namespace SF.Feature.SiteMap
{
    /// <summary>
    /// Represents the SitemapData Base Data Template
    /// </summary>
    public class PageSitemapSettings
    {
        public PageSitemapSettings() : this(Sitecore.Context.Item)
        {

        }

        public PageSitemapSettings(Item item)
        {
            if (item.HasField(Templates.PageSiteMapSettings.Fields.ChangeFrequency))
            {
                this.ChangeFrequency = item.Fields[Templates.PageSiteMapSettings.Fields.ChangeFrequency].Value;
            }
            if (item.HasField(Templates.PageSiteMapSettings.Fields.Priority))
            {
                this.Priority = item.Fields[Templates.PageSiteMapSettings.Fields.Priority].Value;
            }
            if (item.HasField(Templates.PageSiteMapSettings.Fields.OmitFromSitemap))
            {
                this.OmitFromSitemap = ((CheckboxField)item.Fields[Templates.PageSiteMapSettings.Fields.OmitFromSitemap]).Checked;
            }

            if (item.HasField(Templates.PageVideoSiteMapSettings.Fields.HasVideo))
            {
                CheckboxField chkbx = (CheckboxField) item.Fields[Templates.PageVideoSiteMapSettings.Fields.HasVideo];
                if (chkbx.Checked)
                {
                    this.Video = new VideoSiteMapData(item);
                }                
            }

            this.Images = new List<ImageSiteMapData>();

            if (item.HasField(Templates.PageSiteMapSettings.Fields.Images))
            {
                var multiFld = (MultilistField)item.Fields[Templates.PageSiteMapSettings.Fields.Images];
                foreach (var imgItm in multiFld.GetItems())
                {
                    this.Images.Add(new ImageSiteMapData(imgItm));
                }
            }
        }

        public bool OmitFromSitemap { get; set; }
        public string ChangeFrequency { get; set; }
        public string Priority { get; set; }


        public VideoSiteMapData Video { get; set; }

        public List<ImageSiteMapData> Images { get; set; }
    }
}
