using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Feature.Robots
{
    public class RobotsSiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public RobotsSiteSettings()
        {

        }

        public RobotsSiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public RobotsSiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("MultiSiteContext: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public RobotsSiteSettings(Item item)
        {
            Load(item);
        }

        public void Load(Item item)
        {
            var db = item.Database;
            this.ConfigItem = item;
            if (item != null)
            {
                this.SiteConfigurationId = item.ID.Guid;
            }

            this.RobotsId = Guid.Empty;
            if (item.HasField(Templates.SiteRobotSettings.Fields.RobotsId) && !string.IsNullOrEmpty(item.Fields[Templates.SiteRobotSettings.Fields.RobotsId].Value))
            {
                this.RobotsId = new Guid(item.Fields[Templates.SiteRobotSettings.Fields.RobotsId].Value);
            }

        }

        public Guid RobotsId { get; set; }

        public RobotsConfiguration RobotsConfiguration
        {
            get
            {
                if (this.RobotsId != Guid.Empty)
                {
                    return new RobotsConfiguration(this.RobotsId);
                }
                return null;
            }
        }
    }

    
}
