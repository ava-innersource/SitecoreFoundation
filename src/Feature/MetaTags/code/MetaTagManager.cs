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
    /// This class manages rendering meta tags based on site specific
    /// configuration and item settings.
    /// </summary>
    public class MetaTagManager
    {
        public Item Item { get; set; }
        public SiteMetaDataSettings SiteSettings { get; set; }
        public PageMetaDataSettings PageSettings { get; set; }

        public MetaTagManager() : this(Sitecore.Context.Item)
        {

        }

        public MetaTagManager(Item item)
        {
            this.Item = item;
            this.SiteSettings = Sitecore.Context.Site.GetSiteSettings<SiteMetaDataSettings>();
            this.PageSettings = new PageMetaDataSettings(item);
        }

        public string HtmlTitleTag
        {
            get
            {
                if (PageSettings.DisableGlobalSettings || SiteSettings == null || (string.IsNullOrEmpty(SiteSettings.TitlePrefix) && string.IsNullOrEmpty(SiteSettings.TitleSuffix)))
                {
                    return PageSettings.TitleTag;
                }
                else
                {
                    if (PageSettings.TitleTag == null)
                    {
                        return string.Empty;
                    }
                    return string.Format("{0} {1} {2}", SiteSettings.TitlePrefix.Trim(), PageSettings.TitleTag.Trim().ReplacePlaceholders(Item), SiteSettings.TitleSuffix.Trim());
                }
            }
        }

        public string MetaDescription
        {
            get
            {
                if (PageSettings.DisableGlobalSettings || SiteSettings == null || (string.IsNullOrEmpty(SiteSettings.MetaDescPrefix) && string.IsNullOrEmpty(SiteSettings.MetaDescSuffix)))
                {
                    return PageSettings.MetaDescription;
                }
                else
                {
                    if (PageSettings.MetaDescription == null)
                    {
                        return string.Empty;
                    }

                    return string.Format("{0} {1} {2}", SiteSettings.MetaDescPrefix.Trim(), PageSettings.MetaDescription.Trim().ReplacePlaceholders(Item), SiteSettings.MetaDescSuffix.Trim());
                }
            }
        }

        public string MetaKeywords
        {
            get
            {
                if (PageSettings.DisableGlobalSettings)
                {
                    return PageSettings.MetaKeywords;
                }
                else
                {
                    if (PageSettings.MetaKeywords == null || SiteSettings == null || (string.IsNullOrEmpty(SiteSettings.MetaKeywordsPrefix) && string.IsNullOrEmpty(SiteSettings.MetaKeywordsSuffix))) 
                    {
                        return string.Empty;
                    }

                    return string.Format("{0} {1} {2}", SiteSettings.MetaKeywordsPrefix.Trim(), PageSettings.MetaKeywords.Trim().ReplacePlaceholders(Item), SiteSettings.MetaKeywordsSuffix.Trim());
                }
            }
        }

        public string CanonicalUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(PageSettings.CanonicalUrl))
                {
                    return PageSettings.CanonicalUrl;
                }
                if (SiteSettings != null && !string.IsNullOrEmpty(SiteSettings.BaseCanoncialUrl))
                {
                    try
                    {
                        var baseUri = new Uri(SiteSettings.BaseCanoncialUrl);
                        var itemUrl = Sitecore.Links.LinkManager.GetItemUrl(this.Item);
                        var newUri = new Uri(baseUri, itemUrl);
                        return newUri.ToString();
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error with Base Canonical Url", ex, this);
                    }
                }

                return string.Empty;
            }
        }

        public string GetMetaTags()
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(this.HtmlTitleTag))
            {
                sb.Append(string.Format(@"<title>{0}</title>", this.HtmlTitleTag));
                sb.Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(this.MetaDescription))
            {
                sb.Append(string.Format(@"<meta name=""description"" content=""{0}"" />", this.MetaDescription));
                sb.Append(Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(this.MetaKeywords))
            {
                sb.Append(string.Format(@"<meta name=""keywords"" content=""{0}"" />", this.MetaKeywords));
                sb.Append(Environment.NewLine);
            }
            if(!string.IsNullOrEmpty(this.CanonicalUrl))
            {
                sb.Append(string.Format(@"<link rel=""canonical"" href=""{0}"" />", this.CanonicalUrl));
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
