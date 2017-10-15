using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    public class UserSettingsConfiguration
    {
        public static string DefaultArea
        {
            get
            {
                return Sitecore.Configuration.Settings.GetSetting("SF.Facets.DefaultArea");
            }
        }
    }
}