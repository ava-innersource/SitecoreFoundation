using Sitecore.DataExchange.Plugins;

namespace SF.DXF.Feature.SitecoreProvider
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
