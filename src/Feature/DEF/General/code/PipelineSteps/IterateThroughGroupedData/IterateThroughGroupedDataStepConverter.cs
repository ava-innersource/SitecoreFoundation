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
    [SupportedIds(new string[] { "{702A57B2-FFA9-41D8-966E-B423BB4F8596}", "{B86BD26A-F186-42F0-8DFE-8CB0C75D361D}", "{E592E70C-BF82-41E5-A9DB-B02ED7275A99}"})]
    public class IterateThroughGroupedDataStepConverter : BasePipelineStepConverter
    {
        public IterateThroughGroupedDataStepConverter(IItemModelRepository repository)
          : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            this.AddPipelinesSettingsPlugin(source, pipelineStep);
            this.AddDataLocationSettingsPlugin(source, pipelineStep);
            this.AddIterateThroughGroupedDataSettingsPlugin(source, pipelineStep);
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

        private void AddIterateThroughGroupedDataSettingsPlugin(ItemModel source, PipelineStep pipelineStep)
        {
            string groupFieldKey = this.GetStringValue(source, IterateThroughGroupedDataItemModel.GroupFieldKey);
            pipelineStep.Plugins.Add((IPlugin)new IterateThroughGroupedDataSettings()
            {
                 GroupFieldKey = groupFieldKey
            });
        }
    }
}
