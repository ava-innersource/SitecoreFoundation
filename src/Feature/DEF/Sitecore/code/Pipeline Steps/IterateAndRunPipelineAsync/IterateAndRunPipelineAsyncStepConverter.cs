using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System.Collections.Generic;

namespace SF.DEF.Feature.SitecoreProvider
{
    [SupportedIds(new string[] { "{EE1ED7B7-AECA-435A-BADA-07F4E03683A2}" })]
    public class IterateAndRunPipelineAsyncStepConverter : BasePipelineStepConverter
    {
        public IterateAndRunPipelineAsyncStepConverter(IItemModelRepository repository)
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
