using Sitecore.DataExchange.Plugins;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class ResolveSitecoreItemWithLanguageSettings : EndpointSettings
  {
        public ResolveSitecoreItemWithLanguageSettings()
            : base()
        {

        }

        public string LanguageField { get; set; }

    }
}
