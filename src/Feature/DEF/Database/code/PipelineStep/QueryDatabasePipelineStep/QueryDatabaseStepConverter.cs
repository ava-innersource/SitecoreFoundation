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

namespace SF.Feature.DEF.Database
{
    public class QueryDatabaseStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{944D1A17-3C28-446D-8F54-AB3EACCD288F}");
        public QueryDatabaseStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddEndpointSettings(source, pipelineStep);
        }
        private void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new QuerySettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, QueryDatabaseStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.Query =
                base.GetStringValue(source, QueryDatabaseStepItemModel.Query);

            pipelineStep.AddPlugin< QuerySettings>(settings);
        }
    }
}
