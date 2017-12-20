using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DEF.Feature.SitecoreProvider
{
    public class PublishContentStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            var settings = pipelineStep.GetPlugin<PublishContentSettings>();
            if (settings == null)
            {
                logger.Error(
                    "No Publish Content Settings Detected. " +
                    "(pipeline step: {0})",
                    pipelineStep.Name);
                return;
            }

            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
            Sitecore.Data.Database target = Sitecore.Configuration.Factory.GetDatabase(settings.Target);

            var rootItem = master.GetItem(new Sitecore.Data.ID(settings.RootItem));

            logger.Info("About to Start Publish");

            Sitecore.Publishing.PublishManager.PublishItem(rootItem, 
                new Sitecore.Data.Database[] { target }, 
                settings.Languages.ToArray(), 
                settings.ChildItems, 
                true, //always smart publish
                settings.RelatedItems);

            logger.Info("Publish Started for {0}", rootItem.Name);

        }
    }
}