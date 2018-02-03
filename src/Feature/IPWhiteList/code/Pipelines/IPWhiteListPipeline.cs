using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SF.Foundation.Configuration;
using Sitecore.DependencyInjection;
using Sitecore.XA.Foundation.Multisite;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;

namespace SF.Feature.IPWhiteList
{
    public class IPWhiteListPipeline : HttpRequestProcessor
    {
        protected const string GlobalRulesFolderID = @"{0AF14FCD-DCEF-4F02-8585-02D1A46657EB}";

        public override void Process(HttpRequestArgs args)
        {
            var settingsItem = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetSettingsItem(Context.Database.GetItem(Context.Site.StartPath));
            if (settingsItem == null)
            {
                return;
            }

            var whiteListingSettingsItem = settingsItem.FirstChildInheritingFrom(Templates.SiteWhiteListSettings.ID);
            if (whiteListingSettingsItem == null)
            {
                return;
            }

            var whiteListingSettings = new WhiteListSettings(whiteListingSettingsItem);
            if (whiteListingSettings == null)
            {
                return;
            }

            if (whiteListingSettings.WhiteListingEnabled && (Context.Item == null || !Context.Item.ID.Guid.Equals(whiteListingSettings.RestrictedAccessPageId)))
            {
                IPAddress clientIP = null;
                if (IPAddress.TryParse(HttpContext.Current.Request.UserHostAddress, out clientIP))
                {
                    var globalFolder = Context.Database.GetItem(new Sitecore.Data.ID(GlobalRulesFolderID));
                    var siteFolder = whiteListingSettingsItem;

                    foreach (var ip in GetIPs(globalFolder, "GlobalWhiteListedIPs"))
                    {
                        if (clientIP.Equals(ip))
                        {
                            return;
                        }
                    }

                    foreach (var range in GetRanges(globalFolder, "GlobalWhiteListedRanges"))
                    {
                        if (range.IsInRange(clientIP))
                        {
                            return;
                        }
                    }

                    if (siteFolder != null)
                    {
                        foreach (var ip in GetIPs(siteFolder, whiteListingSettings.SiteConfigurationId + "WhiteListedIPs"))
                        {
                            if (clientIP.Equals(ip))
                            {
                                return;
                            }
                        }

                        foreach (var range in GetRanges(siteFolder, whiteListingSettings.SiteConfigurationId + "WhiteListedRanges"))
                        {
                            if (range.IsInRange(clientIP))
                            {
                                return;
                            }
                        }
                    }

                    var sharedSettingsItem = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetSharedSitesSettingsItem(Context.Database.GetItem(Context.Site.StartPath));
                    if (sharedSettingsItem != null)
                    {

                    }

                    //We're not White Listed, try custom page with 302 redirect
                    try
                    {
                        var restricedPageUrl = LinkManager.GetItemUrl(Context.Database.GetItem(new Sitecore.Data.ID(whiteListingSettings.RestrictedAccessPageId)));
                        HttpContext.Current.Response.Status = "302 Moved Temporarily";
                        HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.Moved;
                        HttpContext.Current.Response.AddHeader("Location", restricedPageUrl);
                        HttpContext.Current.Response.End();
                    }
                    catch
                    {
                        //use default IIS 403 instead
                        HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                        HttpContext.Current.Response.End();
                    }
                }
            }
        }

        protected List<IPRange> GetRanges(Item rootFolder, string cacheKey)
        {
            List<IPRange> ranges = HttpRuntime.Cache[cacheKey] as List<IPRange>;
            if (ranges == null)
            {
                ranges = new List<IPRange>();
                var items = rootFolder.Axes.SelectItems(String.Format("*[@@templatename='{0}']", "IP Range"));
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            IPRange range = new IPRange(IPAddress.Parse(item[Templates.IPRange.Fields.StartIP]), IPAddress.Parse(item[Templates.IPRange.Fields.EndIP]));
                            ranges.Add(range);
                        }
                        catch { }
                    }
                }
                HttpRuntime.Cache[cacheKey] = ranges;
            }
            return ranges;

        }

        protected List<IPAddress> GetIPs(Item rootFolder, string cacheKey)
        {

            List<IPAddress> ips = HttpRuntime.Cache[cacheKey] as List<IPAddress>;
            if (ips == null)
            {
                ips = new List<IPAddress>();
                var items = rootFolder.Axes.SelectItems(String.Format("*[@@templatename='{0}']", "Single IP"));
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        try
                        {
                            var ip = IPAddress.Parse(item[Templates.SingleIP.Fields.IP]);
                            ips.Add(ip);
                        }
                        catch { }
                    }
                }
                HttpRuntime.Cache[cacheKey] = ips;
                    
            }
            return ips;

        }
    }
}
