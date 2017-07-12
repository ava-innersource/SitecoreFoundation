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

namespace SF.DXF.Feature.SitecoreProvider
{
    public class EnrollContactInEngagementPlanStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{CBC11FDE-BB35-4E23-B12D-EE093E147D29}");
        public EnrollContactInEngagementPlanStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddUpdateSitecoreItemSettings(source, pipelineStep);
        }
        private void AddUpdateSitecoreItemSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new EnrollContactInEngagementPlanSettings();
            var endpointTo = base.ConvertReferenceToModel<Endpoint>(source, EnrollContactInEngagementPlanItemModel.EndpointTo);
            if (endpointTo != null)
            {
                settings.EndpointTo = endpointTo;
            }

            settings.EngagementPlanStateID =
                base.GetStringValue(source, EnrollContactInEngagementPlanItemModel.EngagementPlanStateID);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
