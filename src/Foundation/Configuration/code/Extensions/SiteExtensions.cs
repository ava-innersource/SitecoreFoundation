using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Configuration
{
    public static class SiteExtensions
    {
        private const string SiteConfigurationKey = "SF.SiteConfiguration";
        private const string SiteConfigurationKeyMS = "SiteConfiguration";

    /// <summary>
    /// Returns the Multisite context configured for the site if it is configured.
    /// </summary>
    /// <param name="site"></param>
    /// <returns></returns>
    public static MultiSiteContext GetMultiSiteContext(this SiteContext site)
    {
      try
      {
        //actual site is not right in page editor.
        var contextSite = GetContextSite();

        string siteConfiguration = contextSite.Properties[SiteConfigurationKey] != null ? contextSite.Properties[SiteConfigurationKey] : contextSite.Properties[SiteConfigurationKeyMS];
        Guid siteGuid = Guid.Empty;
        if (Guid.TryParse(siteConfiguration, out siteGuid))
        {
          return new MultiSiteContext(siteGuid);
        }
        return new MultiSiteContext(siteConfiguration);

      }
      catch (Exception)
      {
        return null;
      }
    }

    public static T GetSiteSettings<T>(this SiteContext site) where T : ISiteSettings, new()
        {
            //actual site is not right in page editor.
            var contextSite = GetContextSite();

            string siteConfiguration = contextSite.Properties[SiteConfigurationKey] != null ? contextSite.Properties[SiteConfigurationKey] : contextSite.Properties[SiteConfigurationKeyMS];
            Guid siteGuid = Guid.Empty;
            if (Guid.TryParse(siteConfiguration, out siteGuid))
            {
                //Get Site Configuration Item
                var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
                var item = db.GetItem(new Sitecore.Data.ID(siteGuid));

                if (item != null)
                {
                    // New Up Instance and load config item
                    var instance = new T();

                    instance.Load(item);
                    return instance;
                }
            }
            return default(T);
        }

        public static Sitecore.Sites.SiteContext GetContextSite()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor || Sitecore.Context.PageMode.IsPreview)
            {
                // item ID for page editor and front-end preview mode
                string id = Sitecore.Web.WebUtil.GetQueryString("sc_itemid");

                // by default, get the item assuming Presentation Preview tool (embedded preview in shell)
                var item = Sitecore.Context.Item;

                // if a query string ID was found, get the item for page editor and front-end preview mode
                if (!string.IsNullOrEmpty(id))
                {
                    item = Sitecore.Context.Database.GetItem(id);
                }

                // loop through all configured sites
                foreach (var site in Sitecore.Configuration.Factory.GetSiteInfoList())
                {
                    // get this site's home page item
                    var homePage = Sitecore.Context.Database.GetItem(site.RootPath + site.StartItem);

                    // if the item lives within this site, this is our context site
                    if (homePage != null && item != null && homePage.Axes.IsAncestorOf(item))
                    {
                        return Sitecore.Configuration.Factory.GetSite(site.Name);
                    }
                }

                // fallback and assume context site
                return Sitecore.Context.Site;
            }
            else
            {
                // standard context site resolution via hostname, virtual/physical path, and port number
                return Sitecore.Context.Site;
            }
        }
    }
}
