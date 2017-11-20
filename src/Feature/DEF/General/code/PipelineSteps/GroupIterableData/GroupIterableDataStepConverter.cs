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

namespace SF.Feature.DEF.General
{
    public class GroupIterableDataStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{ACC98669-2C07-462B-B05C-1DFE9F7453AD}");
        public GroupIterableDataStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddGroupIterableDataSettings(source, pipelineStep);
        }
        private void AddGroupIterableDataSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new GroupIterableDataSettings();


            settings.GroupFieldKey =
                base.GetStringValue(source, GroupIterableDataItemModel.GroupFieldKey);

            settings.RemoveIterableSettingsPlugin = 
                base.GetBoolValue(source, GroupIterableDataItemModel.RemoveIterableSettingsPlugin);

            pipelineStep.Plugins.Add(settings);
        }
    }
}
