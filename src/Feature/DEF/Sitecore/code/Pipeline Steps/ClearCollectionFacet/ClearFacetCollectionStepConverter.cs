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
    public class ClearFacetCollectionStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{C7A26D05-091F-43D8-871F-072CA250B2FC}");
        public ClearFacetCollectionStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddClearFacetCollectionSettings(source, pipelineStep);
        }
        private void AddClearFacetCollectionSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new ClearFacetCollectionSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, ClearFacetCollectionItemModel.EndPointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.FacetName =
                base.GetStringValue(source, ClearFacetCollectionItemModel.FacetName);

            settings.CollectionMemberName =
                base.GetStringValue(source, ClearFacetCollectionItemModel.CollectionMemberName);

            pipelineStep.AddPlugin< ClearFacetCollectionSettings>(settings);
        }
    }
}
