using SF.Feature.Handlebars.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Repositories
{
    public class HandlebarContainerRepository : ModelRepository, IHandlebarContainerRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new HandlebarContainerModel();
            FillBaseProperties(model);
            return model;
        }
    }
}