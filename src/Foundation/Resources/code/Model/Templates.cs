
using Sitecore.Data;

namespace SF.Foundation.Resources
{
  public struct Templates

  {

    public struct Design

    {

      public static readonly ID ID = new ID("{D5B9AFA4-31EB-4561-AB5A-E9DEEF9019E0}");

      public struct Fields

      {

        public static readonly ID Styles = new ID("{781E4E6C-A452-40A9-8497-C3DB175986B1}");
        public static readonly ID Scripts = new ID("{5191447C-E8BB-4BF4-8FFD-AF4535C80DD6}");
        public static readonly ID Minify = new ID("{E85A4AB5-F545-4A7A-85FC-B55FB8C9E6E6}");

      }
    }

    public struct PageResources

    {

      public static readonly ID ID = new ID("{D5B9AFA4-31EB-4561-AB5A-E9DEEF9019E0}");

      public struct Fields

      {

        public static readonly ID PageStyles = new ID("{D4D04742-63A3-4E00-8533-EB6E17ECE948}");
        public static readonly ID PageScripts = new ID("{96F5B29C-D4D5-4941-A3FE-5B52244A9A55}");
        public static readonly ID Minify = new ID("{02D34012-5DB7-43CE-A35F-514CBCC02E77}");
        public static readonly ID BodyCSSOverride = new ID("{1BDC8EFF-C488-4389-9CF9-05CF42BA1C20}");

        public static readonly ID Header = new ID("{E94186DD-AC08-423A-801A-4E1DC6EF4BB8}");
        public static readonly ID Footer = new ID("{840D3F75-C37D-4EA5-876A-3CB43E8DF730}");

      }
    }

    public struct SiteResources

    {

      public static readonly ID ID = new ID("{4D3C897E-2BAC-4A59-9EA7-C8888A28FD2D}");

      public struct Fields

      {

        public static readonly ID SiteStyles = new ID("{9FDA9F8B-F1E8-487E-85A3-BDBCBEF16A6A}");
        public static readonly ID SiteScripts = new ID("{4424082A-C9F6-469A-8363-C4D95B58F1D2}");
        public static readonly ID Minify = new ID("{D5E2AE75-9BBB-4E26-BDF2-FD3934E921BA}");
        public static readonly ID AddBodyClass = new ID("{835AA9E2-9F18-4BED-B927-F4446723B5AC}");

        public static readonly ID Header = new ID("{36D038D3-4E99-4482-9827-C3DF2A679D46}");
        public static readonly ID Footer = new ID("{970BAA00-A57D-4ADD-8827-8DB5EA1223D1}");

      }
    }

    public struct Resource

    {

      public static readonly ID ID = new ID("{E89C3408-6BC2-4C95-A395-7FE1AE6E4078}");

      public struct Fields

      {

        public static readonly ID Name = new ID("{DDA77D50-E0AF-4A9A-A7DC-17233D778EB5}");
        public static readonly ID Version = new ID("{4D7740A6-6828-4552-8BA9-A7155DBB067B}");
        public static readonly ID Notes = new ID("{27B24DA2-007E-4DEC-A78E-F6F5B8E165B5}");

        public static readonly string ContentFieldName = "Content";

      }
    }
  }
}