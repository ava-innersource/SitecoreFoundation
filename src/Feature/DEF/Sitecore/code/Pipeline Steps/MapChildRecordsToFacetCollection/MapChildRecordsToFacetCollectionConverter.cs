using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class MapChildRecordsToFacetCollectionConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{43C68B95-70E2-4BA2-A772-50CD4384BC7E}");
        public MapChildRecordsToFacetCollectionConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddMapChildRecordsToFacetCollectionSettings(source, pipelineStep);
        }
        private void AddMapChildRecordsToFacetCollectionSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new MapChildRecordsToFacetCollectionSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, MapChildRecordsToFacetCollectionItemModel.EndPointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.FacetName =
                base.GetStringValue(source, MapChildRecordsToFacetCollectionItemModel.FacetName);

            settings.CollectionMemberName =
                base.GetStringValue(source, MapChildRecordsToFacetCollectionItemModel.CollectionMemberName);

            settings.RemoveChildRecordsWhenComplete =
                base.GetBoolValue(source, MapChildRecordsToFacetCollectionItemModel.RemoveChildRecordsWhenComplete);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
