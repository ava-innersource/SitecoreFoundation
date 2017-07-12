
using Sitecore.Data;

namespace SF.Feature.Redirection
{
  public struct Templates

  {

    public struct SiteRedirectionSettings

    {

      public static readonly ID ID = new ID("{4B7A43CC-AFC5-4834-BB29-39D197F28AFE}");

      public struct Fields

      {

        public static readonly ID ForceHttps = new ID("{CEC4EBAE-C6EB-4B70-AFE6-50677E8579D7}");
        public static readonly ID ForceWWW = new ID("{A70592C5-786F-46B8-98DE-B560709BB81C}");
        public static readonly ID ApplyRulesForAllRequests = new ID("{D2E3925B-2913-477F-BDEE-22C7DC2FB178}");
        public static readonly ID TakeSiteOffline = new ID("{0ABF82C0-AB34-4745-A4D5-6EBFCEA06731}");
        public static readonly ID SiteOfflinePage = new ID("{628D81E5-53C2-4DA5-AE2D-0E9785AF6EB2}");
        
      }
    }
    
  }
}