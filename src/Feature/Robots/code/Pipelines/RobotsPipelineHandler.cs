using Sitecore.Pipelines.HttpRequest;
using SF.Foundation.Configuration;
using System.Web;

namespace SF.Feature.Robots
{
    /// <summary>
    /// This Pipeline Processor intercepts requests to robots.txt and humans.txt and serves what ever item is 
    /// configured for the multi site conext.
    /// </summary>
    public class RobotsPipelineHandler : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            HttpContext currentContext = args.Context;
            if (currentContext != null)
            {
                var requestedUrl = currentContext.Request.Url.ToString().ToLower();
                if (requestedUrl.EndsWith("robots.txt"))
                {
                    var robotsSiteSettings = Sitecore.Context.Site.GetSiteSettings<RobotsSiteSettings>();
                    if (robotsSiteSettings == null)
                    {
                        return;
                    }
                    var robotsConfig = robotsSiteSettings.RobotsConfiguration;

                    if (!robotsConfig.DisableRobots)
                    {
                        HttpResponse httpResponse = HttpContext.Current.Response;
                        httpResponse.ContentType = "text/plain";
                        httpResponse.Write(robotsConfig.RobotsContent);
                        httpResponse.End();
                        args.AbortPipeline();
                    }
                }
                if (requestedUrl.EndsWith("humans.txt"))
                {
                    
                    var robotsSiteSettings = Sitecore.Context.Site.GetSiteSettings<RobotsSiteSettings>();
            
                    var robotsConfig = robotsSiteSettings.RobotsConfiguration;

                    if (!robotsConfig.DisableHumans)
                    {
                        HttpResponse httpResponse = HttpContext.Current.Response;
                        httpResponse.ContentType = "text/plain";
                        httpResponse.Write(robotsConfig.HumansContent);
                        httpResponse.End();
                        args.AbortPipeline();
                    }
                }
            }
        }
    }
}
