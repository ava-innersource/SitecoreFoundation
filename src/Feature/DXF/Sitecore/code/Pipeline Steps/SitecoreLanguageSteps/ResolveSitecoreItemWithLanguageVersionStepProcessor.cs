using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.Sc.DataAccess.Readers;
using Sitecore.DataExchange.Providers.Sc.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Providers.Sc.Extensions;

namespace SF.DXF.Feature.SitecoreProvider
{

  [RequiredPipelineStepPlugins(new Type[] { typeof(ResolveSitecoreItemSettings) })]
  [RequiredEndpointPlugins(new Type[] { typeof(ItemModelRepositorySettings) })]
  public class ResolveSitecoreItemStepProcessor : BaseResolveObjectFromRepositoryEndpointStepProcessor<object>
  {
    protected override object ResolveObject(object identifierValue, Endpoint endpoint, PipelineStep pipelineStep, PipelineContext pipelineContext)
    {
      if (identifierValue == null)
        throw new ArgumentException("The value cannot be null.", "identifierValue");
      if (endpoint == null)
        throw new ArgumentNullException("endpoint");
      if (pipelineStep == null)
        throw new ArgumentNullException("pipelineStep");
      if (pipelineContext == null)
        throw new ArgumentNullException("pipelineContext");
      ItemModelRepositorySettings repositorySettings = endpoint.GetItemModelRepositorySettings();
      if (repositorySettings == null)
        return (object)null;
      IItemModelRepository itemModelRepository = repositorySettings.ItemModelRepository;
      if (itemModelRepository == null)
        return (object)null;
      ResolveSitecoreItemSettings sitecoreItemSettings = pipelineStep.GetResolveSitecoreItemSettings();
      if (sitecoreItemSettings == null)
        return (object)null;
      ResolveObjectSettings resolveObjectSettings = pipelineStep.GetResolveObjectSettings();
      if (resolveObjectSettings == null)
        return (object)null;

      ResolveSitecoreItemWithLanguageSettings languageSettings = pipelineStep.GetPlugin<ResolveSitecoreItemWithLanguageSettings>();
      if (languageSettings == null)        
        return (ItemModel)null;

      

      ItemModel sourceAsItemModel = this.GetSourceObjectAsItemModel(pipelineStep, pipelineContext);
      if (sourceAsItemModel == null)
        return (ItemModel)null;

      var language = sourceAsItemModel[languageSettings.LanguageField].ToString();

      ILogger logger = pipelineContext.Logger;
      RepositoryObjectStatus status = RepositoryObjectStatus.DoesNotExist;
      ItemModel itemModel = this.DoSearch(identifierValue, sitecoreItemSettings, itemModelRepository, logger, pipelineContext);
      if (itemModel != null)
      {
        this.Log(new Action<string>(pipelineContext.Logger.Debug), pipelineContext, "Item was resolved.", string.Format("identifier: {0}", identifierValue), string.Format("item id: {0}", itemModel["ItemID"]));
        status = RepositoryObjectStatus.Exists;
      }

      if (itemModel == null && !resolveObjectSettings.DoNotCreateIfObjectNotResolved)
      {
        this.Log(new Action<string>(logger.Debug), pipelineContext, "Item was not resolved. Will create it.", string.Format("identifier: {0}", identifierValue));
        itemModel = this.CreateNewItem(this.GetIdentifierObject(pipelineStep, pipelineContext), itemModelRepository, sitecoreItemSettings, logger, pipelineContext, language);
        if (itemModel == null)
          this.Log(new Action<string>(logger.Error), pipelineContext, "Unable to create new item.", string.Format("identifier: {0}", identifierValue));
        else
          this.Log(new Action<string>(logger.Debug), pipelineContext, "New item was created.", string.Format("identifier: {0}", identifierValue));
      }
      this.SetRepositoryStatusSettings(status, pipelineContext);
      return (object)itemModel;
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

    protected virtual ItemModel DoSearch(object value, ResolveSitecoreItemSettings resolveItemSettings, IItemModelRepository repository, ILogger logger, PipelineContext pipelineContext)
    {
      SitecoreItemFieldReader valueReader = this.GetValueReader(resolveItemSettings.MatchingFieldValueAccessor) as SitecoreItemFieldReader;
      if (valueReader == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "The matching field value accessor is not a valid Sitecore item field reader.");
        return (ItemModel)null;
      }
      string str = this.ConvertValueForSearch(value);
      this.Log(new Action<string>(logger.Debug), pipelineContext, "Value converted for search.", string.Format("field: {0}", (object)valueReader.FieldName), string.Format("original value: {0}", value), string.Format("converted value: {0}", (object)str));
      this.Log(new Action<string>(logger.Debug), pipelineContext, "Starting search for item.", string.Format("field: {0}", (object)valueReader.FieldName), string.Format("value: {0}", (object)str));
      IEnumerable<ItemModel> source = repository.Search(new ItemSearchSettings()
      {
        SearchFilters = {
          new SearchFilter()
          {
            FieldName = valueReader.FieldName,
            Value = str
          }
        }
      });
      if (source == null)
        return (ItemModel)null;
      return source.FirstOrDefault<ItemModel>();
    }

    protected virtual string ConvertValueForSearch(object value)
    {
      if (value == null)
        return string.Empty;
      return value.ToString();
    }

    private ItemModel CreateNewItem(object identifierObject, IItemModelRepository repository, ResolveSitecoreItemSettings settings, ILogger logger, PipelineContext pipelineContext, string language)
    {
      IValueReader valueReader = this.GetValueReader(settings.ItemNameValueAccessor);
      if (valueReader == null)
        return (ItemModel)null;
      DataAccessContext context = new DataAccessContext();
      string validItemName = this.ConvertValueToValidItemName(this.ReadValue(identifierObject, valueReader, context), logger, pipelineContext);
      if (validItemName == null)
        return (ItemModel)null;
      
      
      Guid id = repository.Create(validItemName, settings.TemplateForNewItem, settings.ParentItemIdItem, language);

      return repository.Get(id, (string)null, 0);
    }

    private string ConvertValueToValidItemName(object value, ILogger logger, PipelineContext pipelineContext)
    {
      if (value == null)
        return (string)null;
      string str = value.ToString();
      SitecoreItemUtilities plugin = pipelineContext.GetPlugin<SitecoreItemUtilities>();
      if (plugin == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "No plugin is specified on the context to determine whether or not the specified value is a valid item name. The original value will be used.", string.Format("missing plugin: {0}", (object)typeof(SitecoreItemUtilities).FullName));
        return str;
      }
      if (plugin.IsItemNameValid == null)
      {
        this.Log(new Action<string>(logger.Error), pipelineContext, "No delegate is specified on the plugin that can determine whether or not the specified value is a valid item name. The original value will be used.", string.Format("plugin: {0}", (object)typeof(SitecoreItemUtilities).FullName), string.Format("delegate: {0}", (object)"IsItemNameValid"), string.Format("original value: {0}", (object)str));
        return str;
      }
      if (plugin.IsItemNameValid(str))
        return str;
      if (plugin.ProposeValidItemName != null)
        return plugin.ProposeValidItemName(str);
      logger.Error("No delegate is specified on the plugin that can propose a valid item name. The original value will be used. (plugin: {0}, delegate: {1}, original value: {2})", (object)typeof(SitecoreItemUtilities).FullName, (object)"ProposeValidItemName", (object)str);
      return str;
    }

    private IValueReader GetValueReader(IValueAccessor config)
    {
      if (config == null)
        return (IValueReader)null;
      return config.ValueReader;
    }

    private object ReadValue(object source, IValueReader reader, DataAccessContext context)
    {
      if (reader == null)
        return (object)null;
      if (!reader.CanRead(source, context).CanReadValue)
        return (object)null;
      ReadResult readResult = reader.Read(source, context);
      if (!readResult.WasValueRead)
        return (object)null;
      return readResult.ReadValue;
    }

    protected override object ConvertValueToIdentifier(object identifierValue, PipelineStep pipelineStep, PipelineContext pipelineContext)
    {
      return identifierValue;
    }

  }
    
        
  
}
