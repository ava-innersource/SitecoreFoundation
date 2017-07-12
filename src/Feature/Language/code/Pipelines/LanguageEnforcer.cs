using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using SF.Foundation.Configuration;

namespace SF.Feature.Language
{
    public class LanguageEnforcer : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var site = Sitecore.Context.Site.GetSiteSettings<LanguageSiteSettings>();
            if (site == null)
            {
                return;
            }

            if (site.ValidLanguages != null && 
                site.ValidLanguages.Count > 0 && 
                Sitecore.Context.Language != null)
            {
                if (site.ValidLanguages.Where(a => a.Equals(Sitecore.Context.Language)).FirstOrDefault() == null)
                {
                    SendResponse("/" + Sitecore.Context.Site.Language + "/" + Sitecore.Context.Language, string.Empty, args);
                }
            }
        }

        private static void SendResponse(string redirectToUrl, string queryString, HttpRequestArgs args)
        {
            args.Context.Response.Status = "301 Moved Permanently";
            args.Context.Response.StatusCode = 301;
            args.Context.Response.AddHeader("Location", redirectToUrl + queryString);
            args.Context.Response.End();
        }
    }
}
