using SF.Feature.Handlebars.Models;
using SF.Foundation.Components;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Repositories
{
    public class HandlebarJsonContainerRepository : ModelRepository, IHandlebarJsonContainerRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new HandlebarJsonContainerModel();
            FillBaseProperties(model);
            
            var JsonUrlField = (Sitecore.Data.Fields.LinkField)model.Item.Fields["Url"];
            model.JsonUrl = JsonUrlField.LinkUrl();

            return model;
        }
    }
}