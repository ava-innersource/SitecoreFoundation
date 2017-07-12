using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.MetaTags
{
    /// <summary>
    /// Represents the Meta Data Base Date Template
    /// </summary>
    public class PageMetaDataSettings
    {
        public PageMetaDataSettings() : this(Sitecore.Context.Item)
        {

        }

        public PageMetaDataSettings(Item item)
        {
            if (item.HasField(Templates.PageMetaTagSettings.Fields.TitleTag))
            {
                this.TitleTag = item.Fields[Templates.PageMetaTagSettings.Fields.TitleTag].Value;
            }
            if (item.HasField(Templates.PageMetaTagSettings.Fields.MetaDescription))
            {
                this.MetaDescription = item.Fields[Templates.PageMetaTagSettings.Fields.MetaDescription].Value;
            }
            if (item.HasField(Templates.PageMetaTagSettings.Fields.MetaKeywords))
            {
                this.MetaKeywords = item.Fields[Templates.PageMetaTagSettings.Fields.MetaKeywords].Value;
            }
            if (item.HasField(Templates.PageMetaTagSettings.Fields.CanoncialUrl))
            {
                this.CanonicalUrl = item.Fields[Templates.PageMetaTagSettings.Fields.CanoncialUrl].Value;
            }
            if (item.HasField(Templates.PageMetaTagSettings.Fields.DisableGlobalSettings))
            {
                this.DisableGlobalSettings = ((CheckboxField)item.Fields[Templates.PageMetaTagSettings.Fields.DisableGlobalSettings]).Checked;
            }
            
        }

        public string TitleTag { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public bool DisableGlobalSettings { get; set; }
        public string CanonicalUrl { get; set; }
    }
}
