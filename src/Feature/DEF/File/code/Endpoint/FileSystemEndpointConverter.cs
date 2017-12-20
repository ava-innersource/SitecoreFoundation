using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.File
{
    public class FileSystemEndpointConverter : BaseEndpointConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{567C60A0-A45C-4508-8332-CE727E80760B}");
        public FileSystemEndpointConverter(IItemModelRepository repository)
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
            var settings = new FileSystemSettings();
            //
            //populate the plugin using values from the item
            settings.ColumnHeadersInFirstLine =
                base.GetBoolValue(source, FileSystemEndpointItemModel.ColumnHeadersInFirstLine);
            settings.ColumnSeparator =
                base.GetStringValue(source, FileSystemEndpointItemModel.ColumnSeparator);
            settings.Path =
                base.GetStringValue(source, FileSystemEndpointItemModel.Path);
            //
            //add the plugin to the endpoint
            endpoint.AddPlugin< FileSystemSettings>(settings);
        }
    }
}
