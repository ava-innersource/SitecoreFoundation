using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.DataExchange.Extensions;
using System.Text;

namespace SF.DXF.Feature.Database
{
    [RequiredPipelineStepPlugins(typeof(AuditDictionarySettings))]
    public class AuditDictionaryStepProcessor : BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            var logger = pipelineContext.PipelineBatchContext.Logger;
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error("Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            var settings = pipelineStep.GetPlugin<AuditDictionarySettings>();
            if (settings == null)
            {
                logger.Error("Cannot access AuditDictionarySettings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            var synchronizationSettings = pipelineContext.GetSynchronizationSettings();
            if (synchronizationSettings == null)
            {
                logger.Error("Cannot access synchronizationSettings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            Dictionary<string, string> record = settings.IsSource ? 
                synchronizationSettings.Source as Dictionary<string, string> : 
                synchronizationSettings.Target as Dictionary<string, string>;

            StringBuilder sb = new StringBuilder();
            sb.Append("Audit for Pipeline Run. Dictionary located in " + settings.Context);
            sb.Append(Environment.NewLine);
            foreach (var key in record.Keys)
            {
                sb.Append(string.Format("[{0}]:[{1}],", key, record[key]));
            }
            sb.Append(Environment.NewLine);

            logger.Info(sb.ToString());
        }
    }
}