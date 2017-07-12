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
    public class UpdateSitecoreItemWithLanguageVersionStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{934237C5-AE4B-4319-8EB9-D0D95F16E47C}");
        public UpdateSitecoreItemWithLanguageVersionStepConverter(IItemModelRepository repository)
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
            var settings = new UpdateSitecoreItemSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, UpdateSitecoreItemWithLanguageVersionStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.LanguageField =
                base.GetStringValue(source, UpdateSitecoreItemWithLanguageVersionStepItemModel.LanguageField);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
