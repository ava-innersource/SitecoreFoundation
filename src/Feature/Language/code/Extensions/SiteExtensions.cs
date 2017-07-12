using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Diagnostics;
using Sitecore.Sites;

namespace SF.Feature.Language
{
  public static class SiteExtensions
  {
    public static Sitecore.Globalization.Language GetFallbackLanguage(this SiteContext site)
    {
      try
      {
        //actual site is not right in page editor.
        var contextSite = Sitecore.Context.Site;

        string fallbackLanguage = contextSite.Properties["FallbackLanguage"];
        if (!string.IsNullOrEmpty(fallbackLanguage))
        {
          var culture = System.Globalization.CultureInfo.GetCultureInfo(fallbackLanguage);
          var language = Sitecore.Context.Database.Languages.Where(a => a.CultureInfo.Equals(culture)).FirstOrDefault();

          return language;
        }

        return null;

      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}