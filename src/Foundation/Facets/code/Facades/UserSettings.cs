using Sitecore.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets.Facades
{
    public class UserSettings
    {
        private static UserSettings _Settings = new UserSettings();

        public static UserSettings Settings
        {
            get
            {
                return _Settings;
            }
        }

        public string this[string key]
        {
            get
            {
                return this[key, UserSettingsConfiguration.DefaultArea];
            }
            set
            {
                this[key, UserSettingsConfiguration.DefaultArea] = value;
            }
        }

        public string this[string key, string area]
        {
            get
            {
                var settings = Tracker.Current.Contact.GetFacet<IUserSettings>(FacetNames.UserSettings);
                if (settings != null)
                {
                    if (!settings.Entries.Keys.Contains(area))
                    {
                        return string.Empty;
                    }

                    var areaSettings = settings.Entries[area];
                    if (areaSettings != null)
                    {
                        if (!areaSettings.Entries.Keys.Contains(key))
                        {
                            return string.Empty;
                        }

                        var setting = areaSettings.Entries[key];
                        if (setting != null)
                        {
                            return setting.Value;
                        }
                    }
                }
                return string.Empty;
            }
            set
            {
                var settings = Tracker.Current.Contact.GetFacet<IUserSettings>(FacetNames.UserSettings);
                if (settings != null)
                {
                    if (!settings.Entries.Keys.Contains(area))
                    {
                        settings.Entries.Create(area);
                    }
                    var areaSettings = settings.Entries[area];
                    if (!areaSettings.Entries.Keys.Contains(key))
                    {
                        areaSettings.Entries.Create(key);
                    }
                    areaSettings.Entries[key].Value = value;
                }
            }
        }


        public bool ContainsArea(string area)
        {
            var settings = Tracker.Current.Contact.GetFacet<IUserSettings>(FacetNames.UserSettings);
            if (settings != null)
            {
                if (!settings.Entries.Keys.Contains(area))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(string key, string area)
        {
            var settings = Tracker.Current.Contact.GetFacet<IUserSettings>(FacetNames.UserSettings);
            if (settings != null)
            {
                if (!settings.Entries.Keys.Contains(area))
                {
                    if (settings.Entries[area].Entries.Keys.Contains(key))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            return ContainsKey(key, UserSettingsConfiguration.DefaultArea);
        }


        }
}