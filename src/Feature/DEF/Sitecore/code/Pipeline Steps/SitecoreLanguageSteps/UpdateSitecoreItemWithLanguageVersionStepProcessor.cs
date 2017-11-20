using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using Sitecore.DataExchange.Repositories;
using System;
using System.Linq;
using Sitecore.DataExchange.Providers.Sc.Extensions;
using Sitecore.Services.Core.Model;
using Sitecore.DataExchange.Extensions;

namespace SF.DEF.Feature.SitecoreProvider
{

    [RequiredEndpointPlugins(typeof(ItemModelRepositorySettings))]
    public class UpdateSitecoreItemWithLanguageVersionStepProcessor : BaseReadDataStepProcessor
    {
        public UpdateSitecoreItemWithLanguageVersionStepProcessor()
        {
        }
        protected override void ReadData(
            Endpoint endpoint,
            PipelineStep pipelineStep,
            PipelineContext pipelineContext)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException("pipeline");
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException("pipelineContext");
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;

            UpdateSitecoreItemSettings settings = pipelineStep.GetPlugin<UpdateSitecoreItemSettings>();
            if (settings == null || settings.LanguageField == string.Empty)
            {
                logger.Error("Language Field is not set");
                throw new ArgumentException("Language Field");
            }
            var langField = settings.LanguageField;

            var itemModelRepository = this.GetItemModelRepository(pipelineStep, pipelineContext);
            if (itemModelRepository == null)
            {
                logger.Error("Item Model Repository is null");
                return;
            }

            //assume language is in source, and not necessarily mapped to target.
            ItemModel sourceAsItemModel = this.GetSourceObjectAsItemModel(pipelineStep, pipelineContext);
            if (sourceAsItemModel == null)
            {
                logger.Error("Cannot read Source Objec to determine language");
                return;
            }

            var language = sourceAsItemModel[langField].ToString();

            ItemModel objectAsItemModel = this.GetTargetObjectAsItemModel(pipelineStep, pipelineContext);

            if (objectAsItemModel == null)
            {
                logger.Error("Target Item Is not an Item Model.");
                return;
            }
            

            this.FixItemModel(objectAsItemModel);

            //possible enhancement, wire up Version as well (figure out how to determine latest version of item

            if (itemModelRepository.Update(objectAsItemModel.GetItemId(), objectAsItemModel, language.Trim()))
            {
                logger.Info($"UpdateSitecoreItemWithLanguageVersionStepProcessor: item update succeeded for Id:{objectAsItemModel.GetItemId()}, Language:{language}");
            }
            else
            {
                logger.Error($"UpdateSitecoreItemWithLanguageVersionStepProcessor: item update failed for Id:{objectAsItemModel.GetItemId()}, Language:{language}");
            }
        }

        private IItemModelRepository GetItemModelRepository(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            UpdateSitecoreItemSettings settings = pipelineStep.GetPlugin <UpdateSitecoreItemSettings>();
            if (settings == null)
                return null;
            Endpoint endpointTo = settings.EndpointFrom;
            if (endpointTo == null)
                return null;
            ItemModelRepositorySettings repositorySettings = endpointTo.GetItemModelRepositorySettings();
            if (repositorySettings == null)
                return null;

            return repositorySettings.ItemModelRepository;
        }

        protected virtual void FixItemModel(ItemModel itemModel)
        {
            if (itemModel == null)
                return;
            foreach (string index in itemModel.Keys.ToArray<string>())
            {
                object obj = itemModel[index];
                if (obj != null)
                    itemModel[index] = (object)obj.ToString();
            }
        }


        private ItemModel GetTargetObjectAsItemModel(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            SynchronizationSettings synchronizationSettings = pipelineContext.GetPlugin <SynchronizationSettings>();

            if (synchronizationSettings == null)
                return (ItemModel)null;
            if (synchronizationSettings.Target == null)
                return (ItemModel)null;
            return synchronizationSettings.Target as ItemModel;
        }

        private ItemModel GetSourceObjectAsItemModel(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            SynchronizationSettings synchronizationSettings = pipelineContext.GetPlugin<SynchronizationSettings>();

            if (synchronizationSettings == null)
                return (ItemModel)null;
            if (synchronizationSettings.Source == null)
                return (ItemModel)null;

            return ItemModelHelpers.ConvertToItemModel(synchronizationSettings.Source);


        }
    }
}
