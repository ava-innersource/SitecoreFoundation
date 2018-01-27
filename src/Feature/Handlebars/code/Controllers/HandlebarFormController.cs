using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SF.Feature.Handlebars.Models;
using SF.Feature.Handlebars.Repositories;
using SF.Foundation.Components;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Controllers;
using Sitecore.XA.Foundation.Mvc.Controllers;

namespace SF.Feature.Handlebars.Controllers
{
    public class HandlebarFormController : StandardController
    {
        protected readonly IHandlebarFormRepository HandlebarFormRepository;

        public HandlebarFormController(IHandlebarFormRepository repository)
        {
            this.HandlebarFormRepository = repository;
        }

        protected override object GetModel()
        {
            return HandlebarFormRepository.GetModel();
        }

        public new ActionResult Index()
        {
            var model = this.GetModel() as HandlebarFormModel;

            var modelItem = model.Item;

            if (Request.RequestType.ToUpper() == "POST")
            {
                var contentItem = Sitecore.Context.Database.GetItem(modelItem.Fields["Post Processing Data Source"].Value);

                if (contentItem != null)
                {
                    HandlebarManager.SetupContainer(contentItem);
                }

                var proccessors = (MultilistField)modelItem.Fields["Processors"];
             
                foreach(var processor in proccessors.Items)
                {
                    var processorItem = Sitecore.Context.Database.GetItem(processor);
                    var processorTypeValue = processorItem.Fields["Type"].Value;

                    try
                    {
                        
                        Type processorType = Type.GetType(processorTypeValue);
                        var formProcessor = Activator.CreateInstance(processorType) as IFormProcessor;
                        formProcessor.Process(processorItem, modelItem, Request);                        
                    }
                    catch(Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Could not instatiate and invoke type: " + processorTypeValue, ex, this);
                    }
                }

                var redirectField = (LinkField) modelItem.Fields["Redirection Url"];
                if (!string.IsNullOrEmpty(redirectField.Value))
                {
                    return Redirect(redirectField.GetFriendlyUrl());
                }
            }
            else
            {
                var contentItem = Sitecore.Context.Database.GetItem(modelItem.Fields["Content Data Source"].Value);
                if (contentItem != null)
                {
                    HandlebarManager.SetupContainer(contentItem);
                }
            }

            return View(model);
        }

        
        
    }
}