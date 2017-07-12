
using Sitecore.Data;

namespace SF.Feature.Robots
{
  public struct Templates

  {

    public struct SiteRobotSettings

    {

      public static readonly ID ID = new ID("{40D88EAD-C4BC-4F44-B6FF-4B09DE7DCB58}");

      public struct Fields

      {

        public static readonly ID RobotsId = new ID("{82984827-4CFE-45A2-8D05-C063CA0D0047}");
      }
    }

    public struct RobotsConfiguration

    {

      public static readonly ID ID = new ID("{1F1B4108-8325-44C2-89FB-AB6746A666B4}");

      public struct Fields

      {

        public static readonly ID RobotsContent = new ID("{E6CFEC4F-CC7E-475D-ADCF-788B5B369A4F}");
        public static readonly ID DisableRobots = new ID("{58040060-9443-4042-B003-0A4E24C4455E}");
        public static readonly ID HumansContent = new ID("{9064EB8F-1B58-4B6D-AC67-8981A7EA3A59}");
        public static readonly ID DisableHumans = new ID("{ABB52027-CAE9-46A0-A072-1851738B8345}");

      }
    }

  }
}