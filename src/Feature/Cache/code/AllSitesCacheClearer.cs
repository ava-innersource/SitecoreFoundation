using System;


namespace SF.Feature.Cache
{
    /// <summary>
    /// Avoids need to manually add sites to queue to be cleared on publish
    /// </summary>
    public class AllSitesCacheClearer : Sitecore.Publishing.HtmlCacheClearer
    {
        public void ClearCaches(object sender, EventArgs args)
        {
            Sitecore.Diagnostics.Assert.ArgumentNotNull(sender, "sender");
            Sitecore.Diagnostics.Assert.ArgumentNotNull(args, "args");
            string[] siteNames;

            if (this.Sites.Count > 0)
            {
                siteNames = (string[])this.Sites.ToArray();
            }
            else
            {
                siteNames = Sitecore.Configuration.Factory.GetSiteNames();
            }

            Sitecore.Diagnostics.Log.Info(
                this + " clearing HTML caches; " + siteNames.Length + " possible sites.",
                this);

            foreach (string siteName in siteNames)
            {
                Sitecore.Diagnostics.Assert.IsNotNullOrEmpty(siteName, "siteName");
                Sitecore.Sites.SiteContext site = Sitecore.Configuration.Factory.GetSite(siteName);
                Sitecore.Diagnostics.Assert.IsNotNull(site, "siteName: " + siteName);

                if (!site.CacheHtml)
                {
                    continue;
                }

                Sitecore.Caching.HtmlCache htmlCache = Sitecore.Caching.CacheManager.GetHtmlCache(
                  site);
                Sitecore.Diagnostics.Assert.IsNotNull(htmlCache, "htmlCache for " + siteName);

                if (htmlCache.InnerCache.Count < 1)
                {
                    Sitecore.Diagnostics.Log.Info(
                        this + " no entries in output cache for " + siteName,
                        this);
                    continue;
                }

                Sitecore.Diagnostics.Log.Info(
                    this + " clearing output cache for " + siteName,
                    this);
                htmlCache.Clear();
            }

            Sitecore.Diagnostics.Log.Info(this + " done.", this);
        }
    }
}
