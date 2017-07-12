using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SF.Foundation.Configuration;

namespace SF.Foundation.Multisite
{

    /// <summary>
    /// This Pipeline Component overrides the default redirection logic for Item Not Found, Layout Not Found
    /// No Access, Site Access Denied, and Need to Login and instead redirects to configured destinations
    /// specified in the Multi Site Configuration.
    /// </summary>
    public class MultiSitePageConfigurationPipeline : Sitecore.Pipelines.HttpRequest.ExecuteRequest
    {

        protected override void RedirectOnItemNotFound(string url)
        {
            Sitecore.Diagnostics.Log.Warn(
                this + " : No item for " + HttpContext.Current.Request.RawUrl + " (" + Sitecore.Context.User.Name + ")",
                this);

            if (Sitecore.Context.Site.GetMultiSiteContext() == null)
            {
                return;
            }

            var siteConfig = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
            if (siteConfig != null)
            {
                if (!string.IsNullOrEmpty(siteConfig.ItemNotFoundPage))
                {
                    var context = System.Web.HttpContext.Current;

                    try
                    {

                        // NotFound page html content
                        string domain = context.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
                        string content = Sitecore.Web.WebUtil.ExecuteWebPage(string.Concat(domain, siteConfig.ItemNotFoundPage));

                        // set Response params
                        context.Response.TrySkipIisCustomErrors = true;
                        context.Response.StatusCode = 404;
                        context.Response.ContentType = "text/html";
                        context.Response.StatusDescription = "Page not found";

                        // write out NotFound page html content
                        context.Response.Write(content);
                        //context.Response.End();

                        return;
                    }
                    catch
                    {
                        //Web Client failed, maybe auth issue, do normal redirect.
                        this.PerformRedirect(siteConfig.ItemNotFoundPage);
                        return;
                    }
                }
                
            }
            base.RedirectOnItemNotFound(url);
        }

        protected override void RedirectOnLayoutNotFound(string url)
        {
            Sitecore.Diagnostics.Log.Warn(
                this + " : No layout for " + HttpContext.Current.Request.RawUrl + " (" + Sitecore.Context.User + " - " + Sitecore.Context.Device.Name + ")",
                this);

            if (Sitecore.Context.Site.GetMultiSiteContext() == null)
            {
                return;
            }

            var siteConfig = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
            if (siteConfig != null)
            {
                if (!string.IsNullOrEmpty(siteConfig.LayoutNotFoundPage))
                {
                    this.PerformRedirect(siteConfig.LayoutNotFoundPage);
                    return;
                }
                if (!string.IsNullOrEmpty(siteConfig.ItemNotFoundPage))
                {
                    this.PerformRedirect(siteConfig.ItemNotFoundPage);
                    return;
                }
            }
            base.RedirectOnLayoutNotFound(url);
        }

        protected override void RedirectOnNoAccess(string url)
        {
            Sitecore.Diagnostics.Log.Warn(
                this + " : No access for " + HttpContext.Current.Request.RawUrl + " (" + Sitecore.Context.User.Name + ")", this);

            if (Sitecore.Context.Site.GetMultiSiteContext() == null)
            {
                return;
            }

            var siteConfig = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
            
            if (siteConfig != null)
            {
                if (!string.IsNullOrEmpty(siteConfig.NoAccessPage))
                {
                    this.PerformRedirect(siteConfig.NoAccessPage);
                    return;
                }
            }
            base.RedirectOnNoAccess(url);
        }

        protected override void RedirectOnSiteAccessDenied(string url)
        {
            Sitecore.Diagnostics.Log.Warn(
                this + " : No site access for " + HttpContext.Current.Request.RawUrl + " (" + Sitecore.Context.User.Name + ")",
                this);

            if (Sitecore.Context.Site.GetMultiSiteContext() == null)
            {
                return;
            }

            var siteConfig = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
            
            if (siteConfig != null)
            {
                if (!string.IsNullOrEmpty(siteConfig.NoAccessPage))
                {
                    this.PerformRedirect(siteConfig.NoAccessPage);
                    return;
                }
            }
            base.RedirectOnSiteAccessDenied(url);
        }

        protected override void RedirectToLoginPage(string url)
        {
            if (Sitecore.Context.Site.GetMultiSiteContext() == null)
            {
                return;
            }

            var siteConfig = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
            
            if (siteConfig != null)
            {
                if (!string.IsNullOrEmpty(siteConfig.LoginPage))
                {
                    this.PerformRedirect(siteConfig.LoginPage);
                    return;
                }
            }
            base.RedirectToLoginPage(url);
        }

        

    }
}
