using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace SF.Feature.DEF.General
{
    [SupportedIds(new string[] { "{1D1784AE-52D8-4FF1-B969-FFCC62110B5C}" })]
    public class IterateAndRunPipelineAndStoreResultsStepConverter : BasePipelineStepConverter
    {
        public IterateAndRunPipelineAndStoreResultsStepConverter(IItemModelRepository repository)
          : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            this.AddPipelinesSettingsPlugin(source, pipelineStep);
            this.AddDataLocationSettingsPlugin(source, pipelineStep);
        }

        public override PipelineStep Convert(ItemModel source)
        {
            this.CanConvert(source);
            return base.Convert(source) ?? (PipelineStep)null;
        }

        private void AddPipelinesSettingsPlugin(ItemModel source, PipelineStep pipelineStep)
        {
            PipelinesSettings pipelinesSettings = new PipelinesSettings();
            IEnumerable<Pipeline> models = this.ConvertReferencesToModels<Pipeline>(source, "Pipelines");
            if (models != null)
            {
                foreach (Pipeline pipeline in models)
                    pipelinesSettings.Pipelines.Add(pipeline);
            }
            pipelineStep.Plugins.Add((IPlugin)pipelinesSettings);
        }

        private void AddDataLocationSettingsPlugin(ItemModel source, PipelineStep pipelineStep)
        {
            string stringValue = this.GetStringValue(source, "DataLocation");
            pipelineStep.Plugins.Add((IPlugin)new DataLocationSettings()
            {
                DataLocation = stringValue
            });
        }
    }
}
