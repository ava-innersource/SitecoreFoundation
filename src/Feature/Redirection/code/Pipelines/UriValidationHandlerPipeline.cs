using Sitecore.Data.Items;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Linq;
using System.Web;
using SF.Foundation.Configuration;

namespace SF.Feature.Redirection
{
    public class UriValidationHandlerPipeline : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var site = Sitecore.Context.Site.GetSiteSettings<RedirectSiteSettings>();
            if (site == null)
            {
                return;
            }
            
            var requestedUri = HttpContext.Current.Request.Url;

            var sitecoreMode= new[] {"sc_mode=edit", "sc_mode=preview"};
            var isContentAuthoringEnv = sitecoreMode.Any(requestedUri.ToString().ToLower().Contains);

            //Exit if in page editor or in preview mode
            if (isContentAuthoringEnv) return;

            var hasWww = requestedUri.Host.ToLower().StartsWith("www");
            var hasHttps = requestedUri.Scheme == "https";
            var uriToRedirectTo = new UriBuilder(requestedUri);

            bool isUrlChanged = false;

            if (!hasWww)
            {
                bool ignoreForceWww = Sitecore.Configuration.Settings.GetBoolSetting("SF.IgnoreForceWWW", false);

                if (site.ForceWWW && !ignoreForceWww)
                {
                    uriToRedirectTo.Host = "www." + uriToRedirectTo.Host;
                    isUrlChanged = true;
                }
            }

            if (!hasHttps)
            {
                bool ignoreForceHttps = Sitecore.Configuration.Settings.GetBoolSetting("SF.IgnoreForceHTTPS", false);

                if (site.ForceHtps && !ignoreForceHttps)
                {
                    uriToRedirectTo.Scheme = "https";
                    isUrlChanged = true;
                }
            }

            if (isUrlChanged)
            {
                HttpContext.Current.Response.RedirectPermanent(uriToRedirectTo.Uri.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port, UriFormat.UriEscaped));
                args.Context.Response.Status = "301 Moved Permanently";
                args.Context.Response.StatusCode = 301;
                args.Context.Response.AddHeader("Location", uriToRedirectTo.Uri.AbsoluteUri);
                args.Context.Response.End();
            }
   
            
        }
    }
}
