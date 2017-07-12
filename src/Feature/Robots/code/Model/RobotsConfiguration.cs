using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Feature.Robots
{
    /// <summary>
    /// Represents the Robots Configuration Data Template
    /// </summary>
    public class RobotsConfiguration
    {
        public RobotsConfiguration(Guid id) : this(Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(id)))
        {

        }

        public RobotsConfiguration(string path) : this(Sitecore.Context.Database.GetItem(path))
        {

        }

        public RobotsConfiguration(Item item)
        {
            if (item.HasField(Templates.RobotsConfiguration.Fields.RobotsContent))
            {
                this.RobotsContent = item.Fields[Templates.RobotsConfiguration.Fields.RobotsContent].Value;
            }
            if (item.HasField(Templates.RobotsConfiguration.Fields.HumansContent))
            {
                this.HumansContent = item.Fields[Templates.RobotsConfiguration.Fields.HumansContent].Value;
            }
            if (item.HasField(Templates.RobotsConfiguration.Fields.DisableRobots))
            {
                this.DisableRobots = ((CheckboxField)item.Fields[Templates.RobotsConfiguration.Fields.DisableRobots]).Checked;
            }
            if (item.HasField(Templates.RobotsConfiguration.Fields.DisableHumans))
            {
                this.DisableHumans = ((CheckboxField)item.Fields[Templates.RobotsConfiguration.Fields.DisableHumans]).Checked;
            }
        }

        public string RobotsContent { get; set; }
        public string HumansContent { get; set; }
        public bool DisableRobots { get; set; }
        public bool DisableHumans { get; set; }
    }
}
