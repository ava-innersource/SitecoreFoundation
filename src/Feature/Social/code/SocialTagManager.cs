using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.Social
{
    /// <summary>
    /// This class manages rendering social tags given both
    /// site configuration and item settings.
    /// </summary>
    public class SocialTagManager
    {
        public Item Item { get; set; }
        public SiteSocialDataSettings SocialDataSiteSettings { get; set; }
        public PageSocialDataSettings SocialDataPageSettings { get; set; }

        public SocialTagManager() : this(Sitecore.Context.Item)
        {

        }

        public SocialTagManager(Item item)
        {
            this.Item = item;
            this.SocialDataSiteSettings = Sitecore.Context.Site.GetSiteSettings<SiteSocialDataSettings>();
            
            this.SocialDataPageSettings = new PageSocialDataSettings(item);
        }

        public string OpenGraphType
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphType))
                {
                    return SocialDataPageSettings.OpenGraphType.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.OpenGraphType))
                {
                    return SocialDataSiteSettings.OpenGraphType;
                }

                return null;
            }
        }

        public string OpenGraphImage
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphImage))
                {
                    return SocialDataPageSettings.OpenGraphImage;
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.OpenGraphImage))
                {
                    return SocialDataSiteSettings.OpenGraphImage;
                }

                return null;
            }
        }

        public string OpenGraphSiteName
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphSiteName))
                {
                    return SocialDataPageSettings.OpenGraphSiteName.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.OpenGraphSiteName))
                {
                    return SocialDataSiteSettings.OpenGraphSiteName;
                }

                return null;
            }
        }
        
        public string FacebookNumericId
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.FacebookNumericId))
                {
                    return SocialDataPageSettings.FacebookNumericId.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.FacebookNumericId))
                {
                    return SocialDataSiteSettings.FacebookNumericId;
                }

                return null;
            }
        }

        //TwitterPublisherHandle
        public string TwitterPublisherHandle
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterPublisherHandle))
                {
                    return SocialDataPageSettings.TwitterPublisherHandle.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.TwitterPublisherHandle))
                {
                    return SocialDataSiteSettings.TwitterPublisherHandle;
                }

                return null;
            }
        }

        public string TwitterAuthorHandle
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterAuthorHandle))
                {
                    return SocialDataPageSettings.TwitterAuthorHandle.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.TwitterAuthorHandle))
                {
                    return SocialDataSiteSettings.TwitterAuthorHandle;
                }

                return null;
            }
        }

        public string TwitterImage
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterImage))
                {
                    return SocialDataPageSettings.TwitterImage;
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.TwitterImage))
                {
                    return SocialDataSiteSettings.TwitterImage;
                }

                return null;
            }
        }

        public string TwitterCardType
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterCardType))
                {
                    return SocialDataPageSettings.TwitterCardType;
                }
                return null;
            }
        }

        public string GooglePlusAuthorUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusAuthorUrl))
                {
                    return SocialDataPageSettings.GooglePlusAuthorUrl.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.GooglePlusAuthorUrl))
                {
                    return SocialDataSiteSettings.GooglePlusAuthorUrl;
                }

                return null;
            }
        }

        public string GooglePlusPublisherUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusPublisherUrl))
                {
                    return SocialDataPageSettings.GooglePlusPublisherUrl.ReplacePlaceholders(Item);
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.GooglePlusPublisherUrl))
                {
                    return SocialDataSiteSettings.GooglePlusPublisherUrl;
                }

                return null;
            }
        }

        public string GooglePlusImage
        {
            get
            {
                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusImage))
                {
                    return SocialDataPageSettings.GooglePlusImage;
                }

                if (SocialDataSiteSettings != null && !string.IsNullOrEmpty(SocialDataSiteSettings.GooglePlusImage))
                {
                    return SocialDataSiteSettings.GooglePlusImage;
                }

                return null;
            }
        }

        public string GetSocialTags()
        {
            StringBuilder sb = new StringBuilder();

            //See if we have a Facebook Title
            if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphTitle))
            {
                sb.Append("<!-- Open Graph data -->").Append(Environment.NewLine);
                sb.Append(string.Format(@"<meta property=""og:title"" content=""{0}"" />", SocialDataPageSettings.OpenGraphTitle.ReplacePlaceholders(Item))).Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(this.OpenGraphType))
                {
                    sb.Append(string.Format(@"<meta property=""og:type"" content=""{0}"" />", this.OpenGraphType)).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphUrl))
                {
                    sb.Append(string.Format(@"<meta property=""og:url"" content=""{0}"" />", SocialDataPageSettings.OpenGraphUrl.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.OpenGraphImage))
                {
                    sb.Append(string.Format(@"<meta property=""og:image"" content=""{0}"" />", this.OpenGraphImage)).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(SocialDataPageSettings.OpenGraphDescription))
                {
                    sb.Append(string.Format(@"<meta property=""og:description"" content=""{0}"" />", SocialDataPageSettings.OpenGraphDescription.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.OpenGraphSiteName))
                {
                    sb.Append(string.Format(@"<meta property=""og:site_name"" content=""{0}"" />", this.OpenGraphSiteName)).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.FacebookNumericId))
                {
                    sb.Append(string.Format(@"<meta property=""fb:admins"" content=""{0}"" />", this.FacebookNumericId)).Append(Environment.NewLine);
                }
            }

            if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterTitle))
            {
                sb.Append("<!-- Twitter Card data -->").Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(this.TwitterCardType))
                {
                    sb.Append(string.Format(@"<meta name=""twitter:card"" content=""{0}"">", this.TwitterCardType)).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.TwitterPublisherHandle))
                {
                    sb.Append(string.Format(@"<meta name=""twitter:site"" content=""{0}"" />", this.TwitterPublisherHandle)).Append(Environment.NewLine);
                }

                sb.Append(string.Format(@"<meta name=""twitter:title"" content=""{0}"" />", SocialDataPageSettings.TwitterTitle.ReplacePlaceholders(Item))).Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(SocialDataPageSettings.TwitterDescription))
                {
                    sb.Append(string.Format(@"<meta name=""twitter:description"" content=""{0}"" />", SocialDataPageSettings.TwitterDescription.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.TwitterAuthorHandle))
                {
                    sb.Append(string.Format(@"<meta name=""twitter:creator"" content=""{0}"" />", this.TwitterAuthorHandle)).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(this.TwitterImage))
                {
                    sb.Append(string.Format(@"<meta name=""twitter:img:src"" content=""{0}"" />", this.TwitterImage)).Append(Environment.NewLine);
                }
            }

            if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusAuthorUrl) || !string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusPublisherUrl))
            {
                sb.Append("<!-- Google Authorship and Publisher Markup -->").Append(Environment.NewLine);
                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusAuthorUrl))
                {
                    sb.Append(string.Format(@"<link rel=""author"" href=""{0}""/>", SocialDataPageSettings.GooglePlusAuthorUrl.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }
                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusPublisherUrl))
                {
                    sb.Append(string.Format(@"<link rel=""publisher"" href=""{0}""/>", SocialDataPageSettings.GooglePlusPublisherUrl.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }
            }

            if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusName))
            {
                sb.Append("<!-- Schema.org markup for Google+ -->").Append(Environment.NewLine);
                sb.Append(string.Format(@"<meta itemprop=""name"" content=""{0}"">", SocialDataPageSettings.GooglePlusName.ReplacePlaceholders(Item))).Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusDescription))
                {
                    sb.Append(string.Format(@"<meta itemprop=""description"" content=""{0}"">", SocialDataPageSettings.GooglePlusDescription.ReplacePlaceholders(Item))).Append(Environment.NewLine);
                }

                if (!string.IsNullOrEmpty(SocialDataPageSettings.GooglePlusImage))
                {
                    sb.Append(string.Format(@"<meta itemprop=""image"" content=""{0}"">", SocialDataPageSettings.GooglePlusImage)).Append(Environment.NewLine);
                }
            }

            return sb.ToString();
        }
    }
}
