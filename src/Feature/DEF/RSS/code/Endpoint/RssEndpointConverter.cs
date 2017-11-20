using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.RSS
{
    public class RssEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{A13400B4-F0C2-47ED-8135-A697B7E72293}");
        public RssEndpointConverter(IItemModelRepository repository)
            : base(repository)
        {
            //
            //identify the template an item must be based
            //on in order for the converter to be able to
            //convert the item
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //
            //create the plugin
            var settings = new RssSettings();
            //
            //populate the plugin using values from the item
            settings.FeedUrl =
                base.GetStringValue(source, RssEndpointItemModel.FeedUrl);
            
            //add the plugin to the endpoint
            endpoint.Plugins.Add(settings);
        }
    }
}
