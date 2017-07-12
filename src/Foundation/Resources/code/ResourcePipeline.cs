using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Yahoo.Yui.Compressor;

namespace SF.Foundation.Resources
{
    public class ResourcePipeline : HttpRequestProcessor
    {
        
        public override void Process(HttpRequestArgs args)
        {
            Log.Debug("ResourcePipeline: Process() begin", this);

            HttpContext currentContext = args.Context;
            if (currentContext != null)
            {
                Log.Debug("ResourcePipeline: currentContext not null, continuing", this);

                var requestedPath = currentContext.Request.Url.AbsolutePath.ToLower();

                Log.Debug("ResourcePipeline: request URL: " + currentContext.Request.Url.ToString(), this);

                Log.Debug("ResourcePipeline: requestedPath: " + requestedPath, this);
                
                //Don't spend any time if we're not in our path.
                if (!requestedPath.StartsWith("/resources"))
                {
                    Log.Debug("ResourcePipeline: requestedPath does NOT start with /resources, exiting", this);
                    return;
                }
                
                var cacheKey = requestedPath;
                var modifiedKey = cacheKey + "modified";

                // don't use cached content if page is being previewed or is in page editor mode
                bool bypassHttpCache = Sitecore.Context.PageMode.IsPreview ||
                    Sitecore.Context.PageMode.IsExperienceEditor || 
                    Sitecore.Context.PageMode.IsSimulatedDevicePreviewing;

                Log.Info("ResourcePipeline: Bypass HTTP cache? " + bypassHttpCache.ToString(), this);

                // ignore Http cache if in preview mode
                if (!bypassHttpCache)
                {
                    //check Cache, //to do, clear cache on remote publish
                    var cached = HttpRuntime.Cache[cacheKey];
                    DateTime modifiedDate = DateTime.MinValue;
                    if (HttpRuntime.Cache[modifiedKey] != null)
                    {
                        modifiedDate = (DateTime)HttpRuntime.Cache[modifiedKey];
                    }

                    if (cached != null)
                    {
                        Log.Debug("ResourcePipeline: cache[" + cacheKey + "] found in cache", this);

                        var contentType = "text/css";
                        if (requestedPath.EndsWith(".js"))
                        {
                            contentType = "application/x-javascript";
                        }

                        SendResponse(cached.ToString(), modifiedDate, contentType);
                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/css/design"))
                {
                    string id = requestedPath.Substring(requestedPath.Length - 36, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var item = Sitecore.Context.Database.Items[new Sitecore.Data.ID(itemId)];
                    Design design = new Design(item);

                    if (design.Styles != null && design.Styles.Count > 0)
                    {
                        var output = GetContent(design.Styles);

                        ICSSProcessor processor = null;

                        //eventually move to factory implementation
                        var processorType = Sitecore.Configuration.Settings.GetSetting("SF.CSSProcessor").ToLower();
                        if (processorType == "less")
                        {
                            processor = new LessProcessor();
                        }
                        if (processorType == "sass")
                        {
                            processor = new SassProcessor();
                        }

                        if (processor != null)
                        {
                            output = processor.Process(output);
                        }

                        if (design.Minify)
                        {
                            CssCompressor compressor = new CssCompressor();
                            compressor.CompressionType = CompressionType.Standard;
                            output = compressor.Compress(output);
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "text/css");

                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/js/design"))
                {
                    string id = requestedPath.Substring(requestedPath.Length - 35, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var item = Sitecore.Context.Database.Items[new Sitecore.Data.ID(itemId)];
                    Design design = new Design(item);

                    if (design.Scripts != null && design.Scripts.Count > 0)
                    {
                        var output = GetContent(design.Scripts);

                        if (design.Minify)
                        {
                            string beforeCompression = output;

                            try
                            {
                                JavaScriptCompressor compressor = new JavaScriptCompressor();
                                compressor.CompressionType = CompressionType.Standard;
                                output = compressor.Compress(output);
                            }
                            catch (Exception ex)
                            {
                                output = string.Format("/* Error occurred when compressing JS: {0} */\n{1}", ex.Message, beforeCompression);
                            }
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "application/x-javascript");

                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/css/site"))
                {
                    Log.Debug("ResourcePipeline: requestedPath starts with /resources/css/site", this);

                    string id = requestedPath.Substring(requestedPath.Length - 36, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var site = new SiteResources(itemId);

                    Log.Debug("ResourcePipeline: site.SiteCSS is null? " + (site.SiteCSS == null).ToString(), this);
                    Log.Debug("ResourcePipeline: site.SiteCSS.Count: " + (site.SiteCSS != null ? site.SiteCSS.Count.ToString() : "-1"), this);

                    if (site.SiteCSS != null && site.SiteCSS.Count > 0)
                    {
                        Log.Debug("ResourcePipeline: SiteCSS not null and is populated", this);

                        var output = GetContent(site.SiteCSS);

                        Log.Debug("ResourcePipeline: GetContent() output length: " + output.Length.ToString(), this);

                        ICSSProcessor processor = null;

                        //eventually move to factory implementation
                        var processorType = Sitecore.Configuration.Settings.GetSetting("SF.CSSProcessor").ToLower();
                        if (processorType == "less")
                        {
                            processor = new LessProcessor();
                        }
                        if (processorType == "sass")
                        {
                            processor = new SassProcessor();
                        }

                        if (processor != null)
                        {
                            output = processor.Process(output);
                        }


                        if (site.Minify)
                        {
                            CssCompressor compressor = new CssCompressor();
                            compressor.CompressionType = CompressionType.Standard;
                            output = compressor.Compress(output);
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "text/css");

                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/js/site"))
                {
                    Log.Debug("ResourcePipeline: requestedPath starts with /resources/js/site", this);

                    string id = requestedPath.Substring(requestedPath.Length - 35, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var site = new SiteResources(itemId);

                    if (site.SiteScripts != null && site.SiteScripts.Count > 0)
                    {
                        var output = GetContent(site.SiteScripts);

                        if (site.Minify)
                        {
                            string beforeCompression = output;

                            try
                            {
                                JavaScriptCompressor compressor = new JavaScriptCompressor();
                                compressor.CompressionType = CompressionType.Standard;
                                output = compressor.Compress(output);
                            }
                            catch (Exception ex)
                            {
                                output = string.Format("/* Error occurred when compressing JS: {0} */\n{1}", ex.Message, beforeCompression);
                            }
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "application/x-javascript");

                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/css/page"))
                {
                    string id = requestedPath.Substring(requestedPath.Length - 36, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var item = Sitecore.Context.Database.Items[new Sitecore.Data.ID(itemId)];
                    PageResources page = new PageResources(item);

                    if (page.PageCSS != null && page.PageCSS.Count > 0)
                    {
                        var output = GetContent(page.PageCSS);

                        ICSSProcessor processor = null;

                        //eventually move to factory implementation
                        var processorType = Sitecore.Configuration.Settings.GetSetting("SF.CSSProcessor").ToLower();
                        if (processorType == "less")
                        {
                            processor = new LessProcessor();
                        }
                        if (processorType == "sass")
                        {
                            processor = new SassProcessor();
                        }

                        if (processor != null)
                        {
                            output = processor.Process(output);
                        }

                        if (page.Minify)
                        {
                            CssCompressor compressor = new CssCompressor();
                            compressor.CompressionType = CompressionType.Standard;
                            output = compressor.Compress(output);
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "text/css");

                        args.AbortPipeline();
                    }
                }

                if (requestedPath.StartsWith("/resources/js/page"))
                {
                    string id = requestedPath.Substring(requestedPath.Length - 35, 32);
                    Guid itemId = Guid.ParseExact(id, "N");
                    var item = Sitecore.Context.Database.Items[new Sitecore.Data.ID(itemId)];
                    PageResources page = new PageResources(item);

                    if (page.PageScripts != null && page.PageScripts.Count > 0)
                    {
                        var output = GetContent(page.PageScripts);

                        if (page.Minify)
                        {
                            string beforeCompression = output;

                            try
                            {
                                JavaScriptCompressor compressor = new JavaScriptCompressor();
                                compressor.CompressionType = CompressionType.Standard;
                                output = compressor.Compress(output);
                            }
                            catch (Exception ex)
                            {
                                output = string.Format("/* Error occurred when compressing JS: {0} */\n{1}", ex.Message, beforeCompression);
                            }
                        }

                        if (!bypassHttpCache)
                        {
                            HttpRuntime.Cache.Insert(cacheKey, output);
                            HttpRuntime.Cache.Insert(modifiedKey, DateTime.Now);
                        }

                        SendResponse(output, DateTime.Now, "application/x-javascript");

                        args.AbortPipeline();
                    }
                }
            }

            Log.Debug("ResourcePipeline: Process() end", this);
        }

        private static string GetContent(List<Resource> resources)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var resource in resources)
            {
                sb.Append(string.Format(@"
/* -------------------
BEGIN {0} 
Sitecore Resource: {1}
   -------------------*/", resource.Name, resource.ResourceId)).Append(Environment.NewLine);
                sb.Append(resource.Content).Append(Environment.NewLine);

            }

            return sb.ToString();
        }

        private static void SendResponse(string content, DateTime modifiedDate, string contentType)
        {
            HttpResponse httpResponse = HttpContext.Current.Response;
            httpResponse.ContentType = contentType;
            httpResponse.Cache.SetCacheability(HttpCacheability.Public);
            httpResponse.Cache.SetMaxAge(new TimeSpan(30, 0, 0, 0));
            httpResponse.Cache.SetLastModified(modifiedDate);
            httpResponse.Write(content);
            httpResponse.End();
        }
    }
}
