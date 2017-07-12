
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public static class ResourceHtmlExtensions
    {
        public static IHtmlString GetCDNHeaderStyles(this HtmlHelper helper)
        {
            // Get CDN Helper Prefix from Sitecore Setting.
            var site = SiteExtensions.GetContextSite();
            var cdnURL = GetFormattedCdnHostName(site.Properties["cdnHostName"]);
            return helper.GetHeaderStyles(cdnURL);
        }

        public static IHtmlString GetCDNFooterScripts(this HtmlHelper helper)
        {
            //Get CDN Helper Prefix from Sitecore Setting.
            var site = SiteExtensions.GetContextSite();
            var cdnURL = GetFormattedCdnHostName(site.Properties["cdnHostName"]);
            return helper.GetFooterScripts(cdnURL);
        }

        public static string GetFormattedCdnHostName(string cdnHostName)
        {
            if (!string.IsNullOrWhiteSpace(cdnHostName))
            {
                if (!cdnHostName.Trim().StartsWith(@"//"))
                {
                    return @"//" + cdnHostName;
                }
            }

            return string.Empty;
        }

        private static string GetLastUpdateStamp(Item item, string fieldName)
        {
            var key = item.ID.ToString() + fieldName;
            var lastUpdated = item.Statistics.Updated;

            if (HttpRuntime.Cache[key] != null)
            {
                lastUpdated = (DateTime)HttpRuntime.Cache[key];
            }
            else
            {
                var listField = (Sitecore.Data.Fields.MultilistField)item.Fields[fieldName];
                foreach (var listItem in listField.GetItems())
                {
                    if (listItem.Statistics.Updated > lastUpdated)
                    {
                        lastUpdated = listItem.Statistics.Updated;
                    }
                }

                HttpRuntime.Cache.Insert(key, lastUpdated);
            }
            return lastUpdated.ToString("yyyyMMddHHmmss");
        }

        public static IHtmlString GetHeaderStyles(this HtmlHelper helper, string hostNamePrefix = "")
        {
            StringBuilder sb = new StringBuilder();
            
            var site = Sitecore.Context.Site.GetSiteSettings<SiteResources>();
            var page = new PageResources(Sitecore.Context.Item);

            TagBuilder pageCssTag = null;
            TagBuilder siteCssTag = null;
            TagBuilder designCssTag = null;

            //Design CSS
            if (Sitecore.Context.Item.HasField("Design") && 
                !string.IsNullOrEmpty(Sitecore.Context.Item.Fields["Design"].Value))
            {
                var desginGuid = new Guid(Sitecore.Context.Item.Fields["Design"].Value);
                var designId = desginGuid.ToString("N");
                Item designItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(desginGuid));
                if (designItem != null && designItem.HasField("Styles") && !string.IsNullOrEmpty(designItem.Fields["Styles"].Value))
                {
                    designCssTag = new TagBuilder("link");
                    designCssTag.MergeAttribute("rel", "stylesheet");

                    var queryString = "?u=" + GetLastUpdateStamp(designItem, "Styles");

                    designCssTag.MergeAttribute("href", string.Format("{0}/resources/css/design/{1}.css{2}", hostNamePrefix, designId, queryString));
                }
            }

            //Site CSS
            if (site != null && site.SiteCSS != null && site.SiteCSS.Count > 0)
            {
                siteCssTag = new TagBuilder("link");
                siteCssTag.MergeAttribute("rel", "stylesheet");

                var siteDb = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
                var siteItem = siteDb.GetItem(new Sitecore.Data.ID(site.SiteConfigurationId));
                var queryString = "?u=" + GetLastUpdateStamp(siteItem, "SiteCSS");

                siteCssTag.MergeAttribute("href", string.Format("{0}/resources/css/site/{1}.css{2}", hostNamePrefix, site.SiteConfigurationId.ToString("N"), queryString));
            }

            //PageCSS
            if (page.PageCSS != null && page.PageCSS.Count > 0)
            {
                pageCssTag = new TagBuilder("link");
                pageCssTag.MergeAttribute("rel", "stylesheet");
                
                var queryString = "?u=" + GetLastUpdateStamp(Sitecore.Context.Item, "PageCSS");

                pageCssTag.MergeAttribute("href", string.Format("{0}/resources/css/page/{1}.css{2}", hostNamePrefix,Sitecore.Context.Item.ID.ToGuid().ToString("N"), queryString));
            }

            //SiteHeader
            if (site != null)
            {
                sb.Append(site.Header).Append(Environment.NewLine);
            }

            if (page != null)
            {
                //PageHeader
                sb.Append(page.Header).Append(Environment.NewLine);
            }

            return MvcHtmlString.Create(string.Format("{0}{1}{2}{3}",
                (designCssTag != null ? designCssTag.ToString() : string.Empty), 
                (siteCssTag != null ? siteCssTag.ToString() : string.Empty), 
                (pageCssTag != null ? pageCssTag.ToString() : string.Empty),
                sb.ToString()));
        }

        public static IHtmlString GetFooterScripts(this HtmlHelper helper, string hostNamePrefix = "")
        {
            StringBuilder sb = new StringBuilder();
            var site = Sitecore.Context.Site.GetSiteSettings<SiteResources>();
            var page = new PageResources(Sitecore.Context.Item);

            TagBuilder siteJsTag = null;
            TagBuilder pageJsTag = null;
            TagBuilder designJsTag = null;

            //Design JS
            if (Sitecore.Context.Item.HasField("Design") &&
                !string.IsNullOrEmpty(Sitecore.Context.Item.Fields["Design"].Value))
            {
                var desginGuid = new Guid(Sitecore.Context.Item.Fields["Design"].Value);
                var designId = desginGuid.ToString("N");
                Item designItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(desginGuid));
                if (designItem != null && designItem.HasField("Scripts") && !string.IsNullOrEmpty(designItem.Fields["Scripts"].Value))
                {
                    designJsTag = new TagBuilder("script");
                    var queryString = "?u=" + GetLastUpdateStamp(designItem, "Scripts");
                    designJsTag.MergeAttribute("src", string.Format("{0}/resources/js/design/{1}.js{2}", hostNamePrefix, designId, queryString));
                }
            }

            //SiteJS
            if (site != null && site.SiteScripts != null && site.SiteScripts.Count > 0)
            {
                siteJsTag = new TagBuilder("script");

                var siteDb = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
                var siteItem = siteDb.GetItem(new Sitecore.Data.ID(site.SiteConfigurationId));
                var queryString = "?u=" + GetLastUpdateStamp(siteItem, "SiteScripts");

                siteJsTag.MergeAttribute("src", string.Format("{0}/resources/js/site/{1}.js{2}", hostNamePrefix, site.SiteConfigurationId.ToString("N"), queryString));
            }

            // PageScripts
            if (page != null && page.PageScripts != null && page.PageScripts.Count > 0)
            {
                pageJsTag = new TagBuilder("script");
                var queryString = "?u=" + GetLastUpdateStamp(Sitecore.Context.Item, "PageScripts");

                pageJsTag.MergeAttribute("src", string.Format("{0}/resources/js/page/{1}.js{2}", hostNamePrefix, Sitecore.Context.Item.ID.ToGuid().ToString("N"), queryString));
            }

            //SiteHeader
            if (site != null)
            {
                sb.Append(site.Footer).Append(Environment.NewLine);
            }
            //PageHeader
            if (page != null)
            {
                sb.Append(page.Footer).Append(Environment.NewLine);
            }

            return MvcHtmlString.Create(string.Format("{0}{1}{2}{3}",
                (designJsTag != null ? designJsTag.ToString() : string.Empty),
                (siteJsTag != null ? siteJsTag.ToString() : string.Empty),
                (pageJsTag != null ? pageJsTag.ToString() : string.Empty),
                sb.ToString()));
        }
    }

}
