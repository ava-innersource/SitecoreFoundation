using Sitecore.Data.Items;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Resources.Media;
using Sitecore.Links;
using System.Web;
using Sitecore.Data;
using System.Text.RegularExpressions;
using Sitecore.Sites;
using System.IO;
using Sitecore.Data.Fields;
using Sitecore.Analytics;
using Sitecore.Diagnostics;
using Sitecore.Analytics.Data;
using SF.Foundation.Configuration;

namespace SF.Feature.Redirection
{
    public class RedirectHandlerPipeline : HttpRequestProcessor
    {
        /// <summary>
        /// List of extensions which should be processed by this pipeline
        /// </summary>
        private readonly string[] allowedExtensions;
               
        /// <summary>
        /// Specifies a regular expression per site by which requests that can be resolved to an
        /// item are processed anyway. Regex will be matched to the PathAndQuery part of the request URL.
        /// </summary>
        public Dictionary<string, List<string>> ProcessItemsAtUrl { get; set; }

        public RedirectHandlerPipeline()
        {
            ProcessItemsAtUrl = new Dictionary<string, List<string>>();
        }

        public RedirectHandlerPipeline(string allowedExtensions)
        {
            ProcessItemsAtUrl = new Dictionary<string, List<string>>();

            if (string.IsNullOrEmpty(allowedExtensions))
            {
                return;
            }

            // Split allowed extensions and convert to lowercase
            this.allowedExtensions = allowedExtensions.Replace(" ", "")
                .ToLower()
                .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public override void Process(HttpRequestArgs args)
        {   
            var site = Sitecore.Context.Site.GetMultiSiteContext();

            // only do redirects if no item is found or if request parameters match the configuration settings
            if (ProcessUrl() && Context.Database != null && site != null)
            {
                // Grab the actual requested path for use in both the item and pattern match sections.
                var requestedUrl = HttpContext.Current.Request.Url.ToString();
                var requestedUrlOnly = requestedUrl.IndexOf("?") > 0 ? requestedUrl.Substring(0, requestedUrl.IndexOf("?")) : requestedUrl;
                requestedUrl = requestedUrl.EndsWith("/") ? requestedUrl.Substring(0, requestedUrl.Length - 1) : requestedUrl;
                var requestedPath = HttpContext.Current.Request.Url.AbsolutePath;
                var requestedPathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
                var db = Context.Database;

                // In case allowed extensions are defined, only process pipeline when the extension of the request in included in the list 
                // of allowed extensions.
                // If the request does not contain an extension, pipeline will always be processed
                if (allowedExtensions != null && allowedExtensions.Count() > 0)
                {
                    var extensionWithDot = Path.GetExtension(requestedPath);
                    if (string.IsNullOrEmpty(extensionWithDot) == false)
                    {
                        if (allowedExtensions.Contains(extensionWithDot.Substring(1).ToLower()) == false)
                        {
                            return;
                        }
                    }
                }

                var ruleFolder = db.GetItem(new Sitecore.Data.ID(site.SiteId)).Children.Where(a => a.DisplayName == "Redirection Rules").FirstOrDefault();

                if (ruleFolder == null)
                {
                    return;
                }

                // Loop through the exact match entries to look for a match.
                foreach (Item possibleRedirect in GetRedirects(ruleFolder, "Redirect Url"))
                {
                    if (possibleRedirect.HasField("Enabled"))
                    {
                        var enabled = ((CheckboxField)possibleRedirect.Fields["Enabled"]).Checked;
                        if (!enabled)
                        {
                            continue;
                        }
                    }

                    bool Is302 = false;
                    bool IsVanity = false;

                    if (possibleRedirect.HasField("Type"))
                    {
                        string type = possibleRedirect["Type"];
                        if (type.StartsWith("302"))
                        {
                            Is302 = true;
                            IsVanity = false;
                        }
                        if (type.ToLower().StartsWith("vanity"))
                        {
                            Is302 = false;
                            IsVanity = true;
                        }

                    }

                    if (requestedUrl.Equals(possibleRedirect["Requested Url"], StringComparison.OrdinalIgnoreCase) ||
                         requestedPath.Equals(possibleRedirect["Requested Url"], StringComparison.OrdinalIgnoreCase) ||
                         requestedUrlOnly.Equals(possibleRedirect["Requested Url"], StringComparison.OrdinalIgnoreCase))
                    {

                        var redirectToItemId = possibleRedirect.Fields["Target Item"];
                        if (redirectToItemId.HasValue && !string.IsNullOrEmpty(redirectToItemId.ToString()))
                        {
                            
                            var redirectToItem = Context.Database.GetItem(ID.Parse(redirectToItemId));
                            var redirectToUrl = GetRedirectToUrl(redirectToItem);

                            if (possibleRedirect.HasField("Goal") && possibleRedirect.Fields["Goal"].HasValue)
                            {
                                var goal = ((ReferenceField)possibleRedirect.Fields["Goal"]).TargetItem;
                                var encodedUrl = HttpUtility.UrlEncode(redirectToUrl);
                                redirectToUrl = string.Format("/redirect/{0}?u={1}", goal.ID.ToGuid().ToString("N"), encodedUrl);
                            }

                            if (Is302)
                            {
                                SendTemporaryResponse(redirectToUrl, HttpContext.Current.Request.Url.Query, args);
                            }
                            else if (IsVanity)
                            {
                                SendContent(redirectToUrl, HttpContext.Current.Request.Url.Query, args);
                            }
                            else
                            {
                                //default is 301 permanent
                                SendResponse(redirectToUrl, HttpContext.Current.Request.Url.Query, args);
                            }
                        }
                    }
                }

                // Loop through the pattern match items to find a match
                foreach (Item possibleRedirectPattern in GetRedirects(ruleFolder, "Redirect Pattern"))
                {

                    if (possibleRedirectPattern.HasField("Enabled"))
                    {
                        var enabled = ((CheckboxField)possibleRedirectPattern.Fields["Enabled"]).Checked;
                        if (!enabled)
                        {
                            continue;
                        }
                    }

                    bool Is302 = false;
                    bool IsVanity = false;

                    if (possibleRedirectPattern.HasField("Type"))
                    {
                        string type = possibleRedirectPattern["Type"];
                        if (type.StartsWith("302"))
                        {
                            Is302 = true;
                            IsVanity = false;
                        }
                        if (type.ToLower().StartsWith("vanity"))
                        {
                            Is302 = false;
                            IsVanity = true;
                        }

                    }

                    var redirectPath = string.Empty;
                    if (Regex.IsMatch(requestedUrl, possibleRedirectPattern["Source Expression"], RegexOptions.IgnoreCase))
                    {
                        redirectPath = Regex.Replace(requestedUrl, possibleRedirectPattern["Source Expression"],
                                                     possibleRedirectPattern["Target Expression"], RegexOptions.IgnoreCase);
                    }
                    else if (Regex.IsMatch(requestedPathAndQuery, possibleRedirectPattern["Source Expression"], RegexOptions.IgnoreCase))
                    {
                        redirectPath = Regex.Replace(requestedPathAndQuery,
                                                     possibleRedirectPattern["Source Expression"],
                                                     possibleRedirectPattern["Target Expression"], RegexOptions.IgnoreCase);
                    }
                    if (string.IsNullOrEmpty(redirectPath)) continue;

                    if (redirectPath.ToLower().StartsWith("/sitecore"))
                    {
                        // Query portion gets in the way of getting the sitecore item.
                        var pathAndQuery = redirectPath.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                        var path = pathAndQuery[0];
                        if (LinkManager.Provider != null &&
                            LinkManager.Provider.GetDefaultUrlOptions() != null &&
                            LinkManager.Provider.GetDefaultUrlOptions().EncodeNames)
                        {
                            path = MainUtil.DecodeName(path);
                        }
                        var redirectToItem = db.GetItem(path);
                        if (redirectToItem != null)
                        {
                            var redirectToUrl = GetRedirectToUrl(redirectToItem);

                            if (possibleRedirectPattern.HasField("Goal") && possibleRedirectPattern.Fields["Goal"].HasValue)
                            {
                                var goal = ((ReferenceField)possibleRedirectPattern.Fields["Goal"]).TargetItem;

                                var encodedUrl = HttpUtility.UrlEncode(redirectToUrl);
                                redirectToUrl = string.Format("/redirect/{0}?u={1}", goal.ID.ToGuid().ToString("N"), encodedUrl);
                            }

                            var query = pathAndQuery.Length > 1 ? "?" + pathAndQuery[1] : "";

                            if (Is302)
                            {
                                SendTemporaryResponse(redirectToUrl, query, args);
                            }
                            else if (IsVanity)
                            {
                                SendContent(redirectToUrl, query, args);
                            }
                            else
                            {
                                SendResponse(redirectToUrl, query, args);
                            }
                        }
                    }
                    else
                    {
                        //validate it's a URI
                        Uri outUri = null;
                        if (Uri.TryCreate(redirectPath, UriKind.Absolute, out outUri) &&
                            (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
                        {
                            

                            var redirectToUrl = outUri.GetLeftPart(UriPartial.Path);

                            if (possibleRedirectPattern.HasField("Goal") && possibleRedirectPattern.Fields["Goal"].HasValue)
                            {
                                var goal = ((ReferenceField)possibleRedirectPattern.Fields["Goal"]).TargetItem;

                                var encodedUrl = HttpUtility.UrlEncode(redirectToUrl);
                                redirectToUrl = string.Format("/redirect/{0}?u={1}", goal.ID.ToGuid().ToString("N"), encodedUrl);
                            }

                            var query = outUri.Query;

                            if (Is302)
                            {
                                SendTemporaryResponse(redirectToUrl, query, args);
                            }
                            else if (IsVanity)
                            {
                                SendContent(redirectToUrl, query, args);
                            }
                            else
                            {
                                SendResponse(redirectToUrl, query, args);
                            }
                        }
                    }
                }
            }
        }

       


        private static IEnumerable<Item> GetRedirects(Item ruleFolder, string templateName)
        {
            var redirectRoot = ruleFolder.Paths.FullPath;
            IEnumerable<Item> ret = Context.Database.SelectItems(String.Format("fast:{0}//*[@@templatename='{1}']", redirectRoot, templateName));

            // make sure to return an empty list instead of null
            return ret ?? new Item[0];
        }

        /// <summary>
        ///  Once a match is found and we have a Sitecore Item, we can send the 301 response.
        /// </summary>
        private static void SendResponse(Item redirectToItem, string queryString, HttpRequestArgs args)
        {
            var redirectToUrl = GetRedirectToUrl(redirectToItem);
            SendResponse(redirectToUrl, queryString, args);
        }

        private static void SendResponse(string redirectToUrl, string queryString, HttpRequestArgs args)
        {
            args.Context.Response.Status = "301 Moved Permanently";
            args.Context.Response.StatusCode = 301;
            args.Context.Response.AddHeader("Location", redirectToUrl + queryString);
            args.Context.Response.End();
        }

        private static void SendTemporaryResponse(string redirectToUrl, string queryString, HttpRequestArgs args)
        {
            args.Context.Response.Status = "302 Moved Temporarily";
            args.Context.Response.StatusCode = 302;
            args.Context.Response.AddHeader("Location", redirectToUrl + queryString);
            args.Context.Response.End();
        }

        private static void SendContent(string url, string queryString, HttpRequestArgs args)
        {

            var headers = new System.Collections.Specialized.NameValueCollection();

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["Cookie"]))
                {
                    headers.Add("Cookie", HttpContext.Current.Request.Headers["Cookie"]);
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["Host"]))
                {
                    headers.Add("Host", HttpContext.Current.Request.Headers["Host"]);
                }
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["User-Agent"]))
                {
                    headers.Add("User-Agent", HttpContext.Current.Request.Headers["User-Agent"]);
                }
            }

            // may need to change to web client and pass cookies/forms auth to do request as user
            string content = Sitecore.Web.WebUtil.ExecuteWebPage(string.Concat(url, queryString), headers);
            content += "<!-- Redirected from " + url + queryString + "-->";

            // set Response params
            args.Context.Response.TrySkipIisCustomErrors = true;
            args.Context.Response.StatusCode = 200;

            // write out NotFound page html content
            args.Context.Response.Write(content);
            args.Context.Response.End();
        }

        private static string GetRedirectToUrl(Item redirectToItem)
        {
            if (redirectToItem.Paths.Path.StartsWith("/sitecore/media library/"))
            {
                //to do - manage CDN urls.
                var mediaItem = (MediaItem)redirectToItem;
                var mediaUrl = MediaManager.GetMediaUrl(mediaItem);
                var redirectToUrl = StringUtil.EnsurePrefix('/', mediaUrl);
                return redirectToUrl;
            }

            return LinkManager.GetItemUrl(redirectToItem);
        }

        /// <summary>
        /// In general, URLs should only be processed if no item can be resolved.
        /// Exceptions can be defined per site: If the current URL matches a regex in ProcessItemsAtUrl for the
        /// respective site, the request is processed.
        /// </summary>
        /// <returns>true if URL should be processed; otherwise false</returns>
        private bool ProcessUrl()
        {
            var site = Sitecore.Context.Site.GetSiteSettings<RedirectSiteSettings>();
            
            // Item could not be resolved -> process request
            if (Context.Item == null || (site != null && site.ApplyRulesForAllRequests))
            {
                return true;
            }

            // No exceptions for sites defined -> request should not be processed
            if (ProcessItemsAtUrl.Count == 0)
            {
                return false;
            }

            var siteName = Context.Site.Name.ToLower();
            if (ProcessItemsAtUrl.ContainsKey(siteName) && ProcessItemsAtUrl[siteName] != null)
            {
                // Exception for the current site are defined. Check whether PathAndQuery matches any regex.
                foreach (var regex in ProcessItemsAtUrl[siteName])
                {
                    if (Regex.IsMatch(HttpContext.Current.Request.Url.PathAndQuery, regex, RegexOptions.IgnoreCase))
                    {
                        return true;
                    }

                }
            }
            
            return false;
        }

        /// <summary>
        /// Adding regular expressions as exceptions per site.
        /// Request should be processed in case an item can be resolved and PathQuery matches a defined expression 
        /// </summary>
        /// <param name="node">Config node</param>
        public void AddItemUrl(System.Xml.XmlNode node)
        {
            var siteName = node.Attributes["site"].Value.ToLower();
            var regex = node.Attributes["regex"].Value;

            if (string.IsNullOrEmpty(siteName) == true)
            {
                return;
            }

            if (ProcessItemsAtUrl.ContainsKey(siteName) == false)
            {
                ProcessItemsAtUrl.Add(siteName, new List<string>());
            }

            ProcessItemsAtUrl[siteName].Add(regex);
        }
    }
}
