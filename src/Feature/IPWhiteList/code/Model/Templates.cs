
using Sitecore.Data;

namespace SF.Feature.IPWhiteList
{
  public struct Templates

  {

    public struct SiteWhiteListSettings

    {

      public static readonly ID ID = new ID("{FD057F5F-298E-48E9-8C84-5333493ED722}");

      public struct Fields

      {

        public static readonly ID WhiteListingEnabled = new ID("{7FB1A8D2-0773-449E-BBF3-1B33E4845EE8}");
        public static readonly ID RestrictedAccessPage = new ID("{A5EA6E85-6E0B-4366-A432-D119AB3C6B38}");

      }
    }

        public struct SingleIP

        {

            public static readonly ID ID = new ID("{C0335418-EFCE-4AB3-B22B-E18BB931CCAF}");

            public struct Fields

            {

                public static readonly ID IP = new ID("{F8996F06-11EE-4930-A5F0-5A932F4F759F}");

            }
        }

        public struct IPRange

        {

            public static readonly ID ID = new ID("{5B2C7897-A053-4B7F-A3D7-BAF516FBA747}");

            public struct Fields

            {

                public static readonly ID StartIP = new ID("{674D0682-A43A-481B-A889-878E6C7474A5}");
                public static readonly ID EndIP = new ID("{C1DA16E3-871F-4CB5-896B-899046E985BD}");

            }
        }

    }
}