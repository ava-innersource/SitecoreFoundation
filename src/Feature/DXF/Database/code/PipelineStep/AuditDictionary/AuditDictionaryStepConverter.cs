using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.Database
{
    public class AuditDictionaryStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{FECB92C7-27C7-4376-946E-1983853CB276}");
        public AuditDictionaryStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddAuditDictionarySettingsPlugin(source, pipelineStep);
        }
        private void AddAuditDictionarySettingsPlugin(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new AuditDictionarySettings();
            settings.Context =
                base.GetStringValue(source, AuditDictionaryStepItemModel.Context);

            pipelineStep.Plugins.Add(settings);
        }
    }
}