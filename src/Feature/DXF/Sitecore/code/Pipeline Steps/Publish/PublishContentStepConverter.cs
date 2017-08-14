using Sitecore.Data.Managers;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.SitecoreProvider
{
    public class PublishContentStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{BE97C7CF-F4FF-46B3-A197-279C534F8801}");
        public PublishContentStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddEndpointSettings(source, pipelineStep);
        }
        private void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new PublishContentSettings();
            
            settings.RootItem = base.GetStringValue(source, PublishContentItemModel.RootItem);
            
            settings.RelatedItems = base.GetBoolValue(source, PublishContentItemModel.RelatedItems);
            settings.ChildItems = base.GetBoolValue(source, PublishContentItemModel.ChildItems);
            
            settings.Languages = new Sitecore.Collections.LanguageCollection();
            var languages = base.GetStringValue(source, PublishContentItemModel.Languages).Split('|');
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
            foreach(var lang in languages)
            {
                var langItem = master.GetItem(new Sitecore.Data.ID(lang));
                settings.Languages.Add(LanguageManager.GetLanguage(langItem.Name, master));    
            }

            settings.Target = base.GetStringValue(source, PublishContentItemModel.Target);
            
            pipelineStep.Plugins.Add(settings);
        }
    }
}