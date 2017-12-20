using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Database
{

    public class DatabaseEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{16DA30B2-3777-4FC8-A1B8-AEB3380A76DA}");
        public DatabaseEndpointConverter(IItemModelRepository repository)
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
            var settings = new DatabaseSettings();
            //
            //populate the plugin using values from the item
            settings.ConnectionString =
                base.GetStringValue(source, DatabaseEndpointItemModel.ConnectionString);
            settings.ConnectionType =
                base.GetStringValue(source, DatabaseEndpointItemModel.ConnectionType);
            
            //
            //add the plugin to the endpoint
            endpoint.AddPlugin< DatabaseSettings>(settings);
        }
    }
}
