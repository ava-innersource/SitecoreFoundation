using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SF.Foundation.Configuration;

namespace SF.Feature.Favicon
{

    /// <summary>
    /// This Pipeline Processor intercepts requests to favicon.ico and serves what ever item is 
    /// configured for the multi site conext.
    /// </summary>
    public class FaviconPipelineProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            HttpContext currentContext = args.Context;
            if (currentContext != null)
            {
                var requestedUrl = currentContext.Request.Url.ToString().ToLower();
                if (requestedUrl.EndsWith("favicon.ico"))
                {
                    var siteSettings = Sitecore.Context.Site.GetSiteSettings<FaviconSiteSettings>();
                    if (siteSettings == null)
                    {
                        return;
                    }

                    if (siteSettings != null && siteSettings.Favicon != null && siteSettings.Favicon.MediaItem != null)
                    {
                        MediaItem mediaItem = siteSettings.Favicon.MediaItem;
                        Stream stream = mediaItem.GetMediaStream();

                        HttpResponse httpResponse = HttpContext.Current.Response;
                        httpResponse.ContentType = "image/x-icon";
                        stream.CopyTo(httpResponse.OutputStream);
                        httpResponse.End();
                        args.AbortPipeline();
                    }
                }
            }
        }
    }
}
