
using Sitecore.Data;

namespace SF.Feature.SiteMap
{
  public struct Templates

  {

    public struct PageSiteMapSettings

    {

      public static readonly ID ID = new ID("{4F2D3618-2C74-432D-85B9-9CE7606C9776}");

      public struct Fields

      {

        public static readonly ID OmitFromSitemap = new ID("{9EA7F271-EDF2-438C-90A2-A812CB43CCFE}");
        public static readonly ID ChangeFrequency = new ID("{6FCB12A4-E71E-4635-8DB8-C93EC0A52605}");
        public static readonly ID Priority = new ID("{CB9BCB4F-301B-45DD-9563-7627540B6B24}");
        public static readonly ID Images = new ID("{2BF4B4A8-E779-4723-8CE1-F060E646B14F}");
        
      }
    }

    public struct PageVideoSiteMapSettings

    {

      public static readonly ID ID = new ID("{D1647A03-8D13-4152-A00E-13129F1BE1A6}");

      public struct Fields

      {

        public static readonly ID HasVideo = new ID("{EEEA1A2D-6675-45C5-91A4-20358738835C}");
        public static readonly ID VideoTitle = new ID("{1EE409EC-C3DE-49D9-AF1B-230772D235B3}");
        public static readonly ID VideoDescription = new ID("{EA02418B-D802-446D-BDBD-662B2E6D3019}");
        public static readonly ID VideoCategory = new ID("{C8BF1666-FAB8-4057-B324-3D672AED5367}");
        public static readonly ID ContentLocation = new ID("{27C2150D-AABE-4370-AE34-2041225EA5A0}");
        public static readonly ID PlayerLocation = new ID("{0A7CDF98-EF82-407F-9637-87C3FB940E19}");
        public static readonly ID ThumbnailLocation = new ID("{5BCF8A7E-D7CC-46B8-A8A6-0D17FA12088B}");
        public static readonly ID GalleryLocation = new ID("{9DA50506-CBC9-4B77-947C-8162E5B398CC}");
        public static readonly ID Duration = new ID("{EA77040E-86A7-402A-AF9B-AC92176C732C}");
        public static readonly ID Rating = new ID("{106E19CA-FB74-42FB-B228-4A9CCA68092A}");
        public static readonly ID ViewCount = new ID("{685BDFF7-F868-4A1D-8C33-7AFC2996E797}");
        public static readonly ID PublicationDate = new ID("{4932F3C2-9499-4AF6-9E81-03CD16DD6301}");
        public static readonly ID ExpirationDate = new ID("{C2697675-50F0-449B-9334-467D65DC64E6}");
        public static readonly ID FamilyFriendly = new ID("{3A3CB7DE-3A5C-4AAD-A1E3-6C5EAA2192CF}");
        public static readonly ID VideoRestriction = new ID("{F26C3603-3AD5-4F4C-A292-8AE07AA46560}");
        public static readonly ID Price = new ID("{81C32694-9B8A-4DB2-B995-F4F125CC5A99}");
        public static readonly ID Currency = new ID("{9E1E426C-D9A8-4254-AC82-D4FC90E45DD3}");
        public static readonly ID RequiresSubscription = new ID("{59992627-4CB4-4C51-BA4E-618F49DD598B}");
        public static readonly ID Uploader = new ID("{C830E429-F35B-4E6C-8440-E605ED2C8683}");
        public static readonly ID Platform = new ID("{96F20A10-EC4E-4C4B-8C55-A916BB028366}");
        public static readonly ID LiveStream = new ID("{7285D234-843B-4D66-9177-F1513682A49B}");
        public static readonly ID Tags = new ID("{27C2F961-2866-40C3-96A7-925D5EA8EDC9}");
      }
    }

    public struct SiteSiteMapSettings

    {

      public static readonly ID ID = new ID("{A11BEA58-AE51-4E9F-A9FC-BFF3E90B1F0A}");

      public struct Fields

      {

        public static readonly ID SitemapRootId = new ID("{D46F9367-ED25-47AF-819A-5A21FF68DD73}");
        public static readonly ID SitemapDefaultChangeFrequency = new ID("{E616798A-F440-4A9A-A670-E4D3F2C551F1}");
        public static readonly ID SitemapDefaultPriority = new ID("{585F0A9A-0036-4317-85D8-EBE95CB3E350}");

      }
    }
  }
}