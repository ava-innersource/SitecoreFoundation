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

namespace SF.Feature.DEF.Excel
{
    public class ReadExcelTabStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{0FFA905B-5FC9-474F-87B5-C3E451CAF32B}");
        public ReadExcelTabStepConverter(IItemModelRepository repository)
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
            var settings = new ReadExcelTabSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, ReadExcelTabStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.Sheet =
                base.GetStringValue(source, ReadExcelTabStepItemModel.Sheet);

            settings.FirstRowHasColumnNames =
                base.GetBoolValue(source, ReadExcelTabStepItemModel.FirstRowHasColumnNames);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
