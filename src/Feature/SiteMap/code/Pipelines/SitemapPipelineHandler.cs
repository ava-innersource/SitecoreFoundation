using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SF.Foundation.Configuration;

namespace SF.Feature.SiteMap
{
    /// <summary>
    /// This pipeline processor intercepts requests for sitemap.xml and dynamically builds a sitemap
    /// based on both global (multi site specific) and item Sitemap settings.
    /// </summary>
    public class SitemapPipelineHandler : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            HttpContext currentContext = args.Context;
            if (currentContext != null)
            {
                var requestedUrl = currentContext.Request.Url.ToString().ToLower();
                if (requestedUrl.EndsWith("sitemap.xml"))
                {
                    var site = Sitecore.Context.Site.GetSiteSettings<SitemapSiteSettings>();
                    if (site == null)
                    {
                        return;
                    }

                    if (site.SitemapRootId != Guid.Empty)
                    {
                        var rootItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(site.SitemapRootId));

                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?><urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"" xmlns:video=""http://www.google.com/schemas/sitemap-video/1.1"" xmlns:image=""http://www.google.com/schemas/sitemap-image/1.1"" xmlns:xhtml=""http://www.w3.org/1999/xhtml"">");
                        RenderItemInSiteMap(rootItem, site, sb);
                        sb.Append("</urlset>");
                        HttpResponse httpResponse = HttpContext.Current.Response;
                        httpResponse.ContentType = "text/xml";
                        httpResponse.Write(sb.ToString());
                        httpResponse.End();
                        args.AbortPipeline();
                    }
                }
            }
        }

        private void RenderItemInSiteMap(Item item, SitemapSiteSettings site, StringBuilder output)
        {
            var siteMapSettings = new PageSitemapSettings(item);
            var hasLayout = item.Visualization.GetLayout(Sitecore.Context.Device) != null;
            var hasLanguageVersion = HasLanguageVersion(item, Sitecore.Context.Language.Name);

            //only render in sitemap if it has layout and is not explicitly not in Sitemap
            if (hasLayout && !siteMapSettings.OmitFromSitemap && hasLanguageVersion)
            {
                var itemUrl = Sitecore.Links.LinkManager.GetItemUrl(item);
                Uri itemUri = new Uri(HttpContext.Current.Request.Url, itemUrl);

                output.Append("<url>");
                output.Append(string.Format("<loc>{0}</loc>", itemUri.ToString()));

                List<Item> langVersions = new List<Item>();
                foreach(var itemLanguage in item.Languages)
                {
                    var langItem = item.Database.GetItem(item.ID, itemLanguage);
                    if (langItem.Versions.Count > 0)
                    {
                        langVersions.Add(langItem);
                    }
                }

                if (langVersions.Count > 1)
                {
                    foreach(var langItem in langVersions)
                    {
                        var langItemUrl = Sitecore.Links.LinkManager.GetItemUrl(langItem, new Sitecore.Links.UrlOptions() { LanguageEmbedding = Sitecore.Links.LanguageEmbedding.Always, Language = langItem.Language, AlwaysIncludeServerUrl = true });
                        output.Append(string.Format(@"<xhtml:link rel=""alternate"" hreflang=""{0}"" href=""{1}"" />", langItem.Language.ToString(), langItemUrl));
                    }
                }

                DateTime lastMod = item.Statistics.Updated;
                if (lastMod != DateTime.MinValue || lastMod != DateTime.MaxValue)
                {
                    output.Append(string.Format("<lastmod>{0}</lastmod>", lastMod.ToString("yyyy-MM-dd")));
                }

                if (!string.IsNullOrEmpty(siteMapSettings.ChangeFrequency))
                {
                    output.Append(string.Format("<changefreq>{0}</changefreq>", siteMapSettings.ChangeFrequency));
                }
                else if (!string.IsNullOrEmpty(site.SitemapDefaultChangeFrequency))
                {
                    output.Append(string.Format("<changefreq>{0}</changefreq>", site.SitemapDefaultChangeFrequency));
                }

                if (!string.IsNullOrEmpty(siteMapSettings.Priority))
                {
                    output.Append(string.Format("<priority>{0}</priority>", siteMapSettings.Priority));
                }
                else if (!string.IsNullOrEmpty(site.SitemapDefaultPriority))
                {
                    output.Append(string.Format("<priority>{0}</priority>", site.SitemapDefaultPriority));
                }

                foreach(var img in siteMapSettings.Images)
                {
                    output.Append(img.ToImageSiteMapXml());
                }

                if (siteMapSettings.Video != null)
                {
                    output.Append(siteMapSettings.Video.ToVideoSitemapXml());
                }

                

                output.Append("</url>");

            }

            foreach (Item child in item.Children)
            {
                RenderItemInSiteMap(child, site, output);
            }
        }

        private bool HasLanguageVersion(Item item, string languageName)
        {
            var language = item.Languages.FirstOrDefault(l => l.Name == languageName);
            if (language != null)
            {
                var languageSpecificItem = global::Sitecore.Context.Database.GetItem(item.ID, language);
                if (languageSpecificItem != null && languageSpecificItem.Versions.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
