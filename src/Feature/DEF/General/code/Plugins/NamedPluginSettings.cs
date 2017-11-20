using Sitecore.DataExchange;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.DEF.General.Plugins
{
    public class NamedPluginSettings
    {
        public NamedPluginSettings()
        {
            Plugins = new Dictionary<string, IPlugin>();
        }

        public Dictionary<string, IPlugin> Plugins { get; set; }

        public T GetPlugin<T>(string name) where T : class, IPlugin
        {
            if (Plugins.ContainsKey(name))
            {
                return Plugins[name] as T;
            }
            return null;
        }
    }
}