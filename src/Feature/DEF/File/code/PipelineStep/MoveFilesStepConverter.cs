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

namespace SF.DEF.Feature.File
{


    public class MoveFilesStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("[PIPELINE STEP TEMPLATE ID]");
        public MoveFilesStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddMoveFileSettings(source, pipelineStep);
        }
        private void AddMoveFileSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new MoveFileSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, MoveFilesStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.DestinationDirectory =
               base.GetStringValue(source, MoveFilesStepItemModel.DestinationDirectory);
            settings.Copy =
               base.GetBoolValue(source, MoveFilesStepItemModel.Copy);
            settings.AppendTimeStamp =
               base.GetBoolValue(source, MoveFilesStepItemModel.AppendTimeStamp);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
