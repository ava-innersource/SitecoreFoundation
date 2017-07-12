using Sitecore.DataExchange;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.SitecoreProvider
{

    public class ResolveSitecoreItemWithLanguageStepConverter : BaseResolveObjectFromEndpointStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{5070DD8A-6D55-4951-9E75-AE99514E811F}");

        public ResolveSitecoreItemWithLanguageStepConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            base.AddPlugins(source, pipelineStep);
            this.AddCreateSitecoreItemSettings(source, pipelineStep);
        }

        private void AddCreateSitecoreItemSettings(ItemModel source, PipelineStep step)
        {
            //The default
            ResolveSitecoreItemSettings sitecoreItemSettings = new ResolveSitecoreItemSettings()
            {
                ParentItemIdItem = this.GetGuidValue(source, "ParentForItem"),
                MatchingFieldValueAccessor = this.ConvertReferenceToModel<IValueAccessor>(source, "MatchingFieldValueAccessor"),
                TemplateForNewItem = this.GetGuidValue(source, "TemplateForNewItem"),
                ItemNameValueAccessor = this.ConvertReferenceToModel<IValueAccessor>(source, "ItemNameValueAccessor")
            };
            step.Plugins.Add((IPlugin)sitecoreItemSettings);

            //My endpoint and Language
            var settings = new ResolveSitecoreItemWithLanguageSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, ResolveSitecoreItemWithLanguageStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }

            settings.LanguageField =
                base.GetStringValue(source, ResolveSitecoreItemWithLanguageStepItemModel.LanguageField);

            step.Plugins.Add(settings);
        }
    }
}
