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
using System.Web;
using Sitecore.Analytics.Automation;
using Sitecore.Analytics.Model.Framework;
using SF.Feature.DEF.General;

namespace SF.DEF.Feature.SitecoreProvider
{
    [RequiredPipelineStepPlugins(typeof(MapChildRecordsToFacetCollectionSettings))]
    public class MapChildRecordsToFacetCollectionProcessor : BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            var logger = pipelineContext.PipelineBatchContext.Logger;
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error("Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

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

            var settings = pipelineStep.GetPlugin<MapChildRecordsToFacetCollectionSettings>();
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

            var childRecordSettings = pipelineContext.GetPlugin<ChildRecordSettings>();
            if (childRecordSettings == null)
            {
                logger.Error("No Child Records Available to Process");
                return;
            }

            foreach(var record in childRecordSettings.Records)
            {
                var newEntry = collectionProperty.Create();
                foreach(var key in record.Keys)
                {
                    try
                    {
                        newEntry.GetType().GetProperty(key).SetValue(newEntry, record[key]);
                    }
                    catch(Exception ex)
                    {
                        logger.Error("Could not Set property {0} on facet {1} {2}. exception details: {2}", key, settings.FacetName, settings.CollectionMemberName, ex);
                        return;
                    }
                }
            }

            if (settings.RemoveChildRecordsWhenComplete)
            {
                pipelineContext.Plugins.Remove(childRecordSettings);
            }
            
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