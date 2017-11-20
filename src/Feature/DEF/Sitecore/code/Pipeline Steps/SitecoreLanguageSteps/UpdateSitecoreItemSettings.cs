using Sitecore.DataExchange.Plugins;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class UpdateSitecoreItemSettings : EndpointSettings
  {
        public UpdateSitecoreItemSettings() : base()
        {

        }

        public string LanguageField { get; set; }
    }
}
