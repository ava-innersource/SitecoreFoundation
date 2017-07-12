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

namespace SF.DXF.Feature.SitecoreProvider
{
    [RequiredPipelineStepPlugins(typeof(EnrollContactInEngagementPlanSettings))]
    public class EnrollContactInEngagementPlanStepProcessor : BasePipelineStepProcessor
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

            var settings = pipelineStep.GetPlugin<EnrollContactInEngagementPlanSettings>();
            if (settings == null)
            {
                logger.Error("Cannot access Engagement Plan Settings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            string state = settings.EngagementPlanStateID;
            bool wasEnrolled = AutomationContactManager.AddContact(contact.ContactId, new Sitecore.Data.ID(state));

            if (!wasEnrolled)
            {
                logger.Warn(@"Contact Was not Enrolled (contact: {0})", contact.ContactId);
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