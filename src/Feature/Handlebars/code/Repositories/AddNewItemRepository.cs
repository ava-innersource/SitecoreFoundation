using SF.Feature.Handlebars.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Repositories
{
    public class AddNewItemRepository : ModelRepository, IAddNewItemRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new AddNewItemModel();
            FillBaseProperties(model);

            model.ButtonText = model.Rendering.Parameters["ButtonText"];
            model.Template = model.Rendering.Parameters["Template"];
            model.Parent = model.Rendering.Parameters["RootFolder"];

            return model;
        }
        
    }
}