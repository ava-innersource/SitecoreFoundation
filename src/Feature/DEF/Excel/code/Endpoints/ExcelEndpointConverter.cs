using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.Excel
{

    public class ExcelEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{7A70017E-BB19-4D4F-87CF-47B31AE8DA96}");
        public ExcelEndpointConverter(IItemModelRepository repository)
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
            var settings = new ExcelSettings();
            //
            //populate the plugin using values from the item
            settings.FileLocation =
                base.GetStringValue(source, ExcelEndpointItemModel.FileLocation );
            
            //
            //add the plugin to the endpoint
            endpoint.AddPlugin< ExcelSettings>(settings);
        }
    }
}
