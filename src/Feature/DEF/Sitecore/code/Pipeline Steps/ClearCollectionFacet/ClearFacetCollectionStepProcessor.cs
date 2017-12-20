using Sitecore.Analytics.Tracking;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Services.Core.Diagnostics;

namespace SF.DEF.Feature.SitecoreProvider
{
    [RequiredPipelineStepPlugins(typeof(ClearFacetCollectionSettings))]
    public class ClearFacetCollectionStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            Contact contact = this.GetTargetObjectAsContact(pipelineStep, pipelineContext);
            if (contact == null)
            {
                logger.Error("Target Item is not a contact. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            if (contact.ContactId == null || contact.ContactId == Guid.Empty)
            {
                logger.Error("Contact Id does not Exist. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            var settings = pipelineStep.GetPlugin<ClearFacetCollectionSettings>();
            if (settings == null)
            {
                logger.Error("Cannot access ClearFacetCollectionSettings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            //Use Reflection to get Collection
            if (!contact.Facets.Keys.Contains(settings.FacetName))
            {
                logger.Warn("Facet {0} does not exist on contact)", settings.FacetName);
                return;
            }

            var facet = contact.Facets[settings.FacetName];

            var collectionProperty = facet.GetType().GetProperty(settings.CollectionMemberName).GetValue(facet) as IElementCollection<IElement>;

            if (collectionProperty == null)
            {
                logger.Error("Member Name {0} is not a IElementCollection of IElement", settings.CollectionMemberName);
                return;
            }

            int ctnRemoved = 0; 
            while (collectionProperty.Count() > 0)
            {
                collectionProperty.Remove(0);
                ctnRemoved++;
            }

            logger.Info("Removed {0} elements.", ctnRemoved);
        }

        
        private Contact GetTargetObjectAsContact(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            if (!pipelineContext.HasSynchronizationSettings())
                return (Contact)null;
            SynchronizationSettings synchronizationSettings = pipelineContext.GetSynchronizationSettings();
            if (synchronizationSettings == null)
                return (Contact)null;
            if (synchronizationSettings.Target == null)
                return (Contact)null;
            return synchronizationSettings.Target as Contact;
        }
    }
}