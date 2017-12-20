using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Services.Core.Diagnostics;

namespace SF.Feature.DEF.General
{
    [RequiredPipelineStepPlugins(typeof(GroupIterableDataSettings))]
    public class GroupIterableDataStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        { 
            var settings = pipelineStep.GetPlugin<GroupIterableDataSettings>();
            if (settings == null)
            {
                logger.Error("Cannot access ClearFacetCollectionSettings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            IterableDataSettings iterableDataSettings = pipelineContext.GetIterableDataSettings();
            if (iterableDataSettings == null || iterableDataSettings.Data == null)
                return;

            var groupedData = new GroupedDataSettings();

            foreach (object element in iterableDataSettings.Data)
            {
                var record = element as Dictionary<string, string>;
                if (record != null)
                {
                    if (record.ContainsKey(settings.GroupFieldKey))
                    {
                        var key = record[settings.GroupFieldKey];
                        if (!groupedData.Data.ContainsKey(key))
                        {
                            groupedData.Data.Add(key, new List<Dictionary<string, string>>());
                        }
                        groupedData.Data[key].Add(record);
                    }
                }
            }

            pipelineContext.AddPlugin<GroupedDataSettings>(groupedData);

            if (settings.RemoveIterableSettingsPlugin)
            {
                // This should be checked if another pipeline step is going to add settings.
                pipelineContext.RemovePlugin(iterableDataSettings.GetType());
            }
        }
        
    }
}