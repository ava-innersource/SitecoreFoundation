using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{
    [RequiredPipelineStepPlugins(new Type[] { typeof(PipelinesSettings), typeof(DataLocationSettings) })]
    [RequiredPipelineContextPlugins(new Type[] { typeof(IterableDataSettings) })]
    public class IterateAndRunPipelineAndStoreResultsStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error("Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})", (object)pipelineStep.Name);
            }
            else
            {
                PipelinesSettings pipelinesSettings = pipelineStep.GetPipelinesSettings();
                if (pipelinesSettings == null || !pipelinesSettings.Pipelines.Any<Pipeline>())
                {
                    logger.Error("Pipeline step processing will abort because the pipeline step has no sub-pipelines assigned. (pipeline step: {0})", (object)pipelineStep.Name);
                }
                else
                {
                    IterableDataSettings iterableDataSettings = pipelineContext.GetIterableDataSettings();
                    if (iterableDataSettings == null || iterableDataSettings.Data == null)
                        return;

                    int num = 0;

                    try
                    {
                        var childRecordSettings = new ChildRecordSettings();

                        foreach (object element in iterableDataSettings.Data)
                        {

                            if (!pipelineContext.PipelineBatchContext.Stopped)
                            {
                                PipelineContext pipelineContext1 = new PipelineContext(pipelineContext.PipelineBatchContext);
                                SynchronizationSettings synchronizationSettings = this.ResolveSynchronizationSettingsAndSetElement(pipelineStep, pipelineContext, element);
                                //instatiate Target for filling.
                                synchronizationSettings.Target = new Dictionary<string, string>();

                                pipelineContext1.AddPlugin<SynchronizationSettings>(synchronizationSettings);
                                ParentPipelineContextSettings pipelineContextSettings = new ParentPipelineContextSettings()
                                {
                                    ParentPipelineContext = pipelineContext
                                };
                                pipelineContext1.AddPlugin<ParentPipelineContextSettings>(pipelineContextSettings);
                                this.ProcessPipelines(pipelineStep, pipelinesSettings.Pipelines, pipelineContext1, logger);

                                //Now Let's get the results
                                var record = pipelineContext1.GetPlugin<SynchronizationSettings>().Target as Dictionary<string, string>;
                                if (record != null)
                                {
                                    childRecordSettings.Records.Add(record);
                                }

                            }

                            num++;
                        }

                        //Add to the context so we can do something with this collection
                        pipelineContext.AddPlugin< ChildRecordSettings>(childRecordSettings);

                        logger.Info("{0} elements were iterated. (pipeline: {1}, pipeline step: {2})", (object)num, (object)pipelineContext.CurrentPipeline.Name, (object)pipelineContext.CurrentPipelineStep.Name, (object)pipelineContext);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);
                        pipelineContext.CriticalError = true;
                    }
                }
            }
        }

        protected virtual SynchronizationSettings ResolveSynchronizationSettingsAndSetElement(PipelineStep pipelineStep, PipelineContext pipelineContext, object element)
        {
            DataLocationSettings locationSettings = pipelineStep.GetDataLocationSettings();
            SynchronizationSettings synchronizationSettings = new SynchronizationSettings();
            if (locationSettings.DataLocation == ItemIDs.PipelineContextStorageLocationParentSource)
                synchronizationSettings.Source = element;
            if (locationSettings.DataLocation == ItemIDs.PipelineContextStorageLocationParentTarget)
                synchronizationSettings.Target = element;
            return synchronizationSettings;
        }

        protected virtual void ProcessPipelines(PipelineStep pipelineStep, ICollection<Pipeline> subPipelines, PipelineContext pipelineContext, ILogger logger)
        {
            if (pipelineStep == null)
                throw new ArgumentNullException("pipelineStep");
            if (subPipelines == null)
                throw new ArgumentNullException("subPipelines");
            if (pipelineContext == null)
                throw new ArgumentNullException("pipelineContext");
            if (!subPipelines.Any<Pipeline>())
            {
                logger.Error("Pipeline step processing will abort because no pipelines are assigned to the pipeline step. (pipeline step: {0})", (object)pipelineStep.Name);
            }
            else
            {
                List<Pipeline> pipelineList = new List<Pipeline>();
                foreach (Pipeline subPipeline in (IEnumerable<Pipeline>)subPipelines)
                {
                   RunSubPipelines(pipelineContext, subPipeline, logger); 
                }
                
            }
        }

        private void RunSubPipelines(PipelineContext pipelineContext, Pipeline subPipeline, ILogger logger)
        {
            pipelineContext.CurrentPipeline = subPipeline;
            IPipelineProcessor pipelineProcessor = subPipeline.PipelineProcessor;

            if (pipelineProcessor == null)
                logger.Error("Pipeline will be skipped because it does not have a processor assigned. (pipeline step: {0}, sub-pipeline: {1})", "Iterate and Run Async", (object)subPipeline.Name);
            else
            {
                pipelineProcessor.StartProcessing(subPipeline, pipelineContext, logger);
                if (pipelineContext.CriticalError)
                {
                    logger.Error("Sub pipeline processing will not abort since it's done async");                   
                }
            }
        }

        protected virtual void OnCriticalError(Pipeline errorPipeline, IEnumerable<Pipeline> completedPipelines, PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
        }

        public bool CanProcess(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            if (pipelineStep.GetDataLocationSettings().DataLocation != null)
                return true;
            pipelineContext.PipelineBatchContext.Logger.Error("No data location is specified. (plugin: {0}, pipeline step: {1})", (object)typeof(DataLocationSettings), (object)pipelineStep.Name);
            return false;
        }
    }
}
