using Sitecore.Analytics.Tracking;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Extensions;
using System;
using Sitecore.Xdb.MarketingAutomation.OperationsClient;
using Sitecore.Xdb.MarketingAutomation.Core.Requests;
using Sitecore.Xdb.MarketingAutomation.Core.Results;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace SF.DEF.Feature.SitecoreProvider
{
    [RequiredPipelineStepPlugins(typeof(EnrollContactInEngagementPlanSettings))]
    public class EnrollContactInEngagementPlanStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            var log = pipelineContext.PipelineBatchContext.Logger;

            Contact contact = this.GetTargetObjectAsContact(pipelineStep, pipelineContext);
            if (contact == null)
            {
                log.Error("Target Item is not a contact. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            if (contact.ContactId == null || contact.ContactId == Guid.Empty)
            {
                log.Error("Contact Id does not Exist. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            var settings = pipelineStep.GetPlugin<EnrollContactInEngagementPlanSettings>();
            if (settings == null)
            {
                log.Error("Cannot access Engagement Plan Settings. (pipeline step: {0})", (object)pipelineStep.Name);
                return;
            }

            
            try
            {
                var planId = Guid.Parse(settings.EngagementPlanStateID);

                var operationsClient = ServiceLocator.ServiceProvider.GetService<IAutomationOperationsClient>();
                
                var request = new EnrollmentRequest(contact.ContactId, planId); // Contact ID, Plan ID

                request.Priority = 1; // Optional

                //To do Map Activity and Custom Values
                //request.ActivityId = Guid.Parse("{C5B87651-EE70-4684-BDD9-0B464B79476D}"); // Optional
                //request.CustomValues.Add("test", "test"); // Optional

                BatchEnrollmentRequestResult result = operationsClient.EnrollInPlanDirect(new[] { request });

                if (!result.Success)
                {
                    log.Warn(@"Contact Was not Enrolled (contact: {0})", contact.ContactId);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error Enrolling Contact in Plan", ex);
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