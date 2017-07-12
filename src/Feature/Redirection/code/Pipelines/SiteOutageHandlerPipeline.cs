using Sitecore.Data;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Web;
using SF.Foundation.Configuration;

namespace SF.Feature.Redirection
{
    public class SiteOutageHandlerPipeline : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var site = Sitecore.Context.Site.GetSiteSettings<RedirectSiteSettings>();
            if (site == null)
            {
                return;
            }

            var requestedUri = HttpContext.Current.Request.Url;

            if (site.TakeSiteOffline && !string.IsNullOrWhiteSpace(site.SiteOfflinePage))
            {
                var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
                var pageItem = db.GetItem(new ID(site.SiteOfflinePage));
                if (pageItem != null)
                {
                    var pageUrl = pageItem.GetItemUrl();
                    if (!requestedUri.PathAndQuery.Equals(pageUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        args.Context.Response.Status = "302 Moved Temporarily";
                        args.Context.Response.StatusCode = 302;
                        args.Context.Response.AddHeader("Location", pageUrl);
                        args.Context.Response.End();
                    }
                }
            }
        }
    }
}
