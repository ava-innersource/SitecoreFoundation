
using Sitecore.Data;

namespace SF.Feature.MetaTags
{
  public struct Templates

  {

    public struct SiteMetaTagSettings

    {

      public static readonly ID ID = new ID("{6694869E-72F2-408F-9848-90A23C45ECE7}");

      public struct Fields

      {

        public static readonly ID TitlePrefix = new ID("{0C77A26C-1C00-427F-AA18-BCF83DF753E6}");
        public static readonly ID TitleSuffix = new ID("{27075040-BB6D-48BF-8C6E-8FF59EEC975A}");
        public static readonly ID MetaDescPrefix = new ID("{BBA037A1-1A20-4470-808C-A672078D1560}");
        public static readonly ID MetaDescSuffix = new ID("{C9B3A065-09EF-4295-977F-B570C89E86FB}");
        public static readonly ID MetaKeywordsPrefix = new ID("{DC576ADF-C8AB-4AD7-95C1-8320120705C2}");
        public static readonly ID MetaKeywordsSuffix = new ID("{6ADE9CEC-6CFB-40E7-9992-08A432EF1B0F}");
        public static readonly ID BaseCanoncialUrl = new ID("{89F9196F-3581-4575-9C2A-042F513F9412}");

      }
    }

    public struct PageMetaTagSettings

    {

      public static readonly ID ID = new ID("{7DB1AE6C-09AD-434B-BF4A-2B160B2FF927}");

      public struct Fields

      {

        public static readonly ID TitleTag = new ID("{2F085078-5A11-485E-8260-68B49BDCDCA9}");
        public static readonly ID MetaDescription = new ID("{D3159BC4-46B3-4530-AF05-C19EA2E9E5EB}");
        public static readonly ID MetaKeywords = new ID("{1A1A4146-B7B5-4F05-BF2E-4271314E5579}");
        public static readonly ID CanoncialUrl = new ID("{76D2825D-E8D5-4B84-AA30-B02E5110AF16}");
        public static readonly ID DisableGlobalSettings = new ID("{C95201DC-84FA-4BB2-9ABF-372645056007}");

      }
    }

  }
}