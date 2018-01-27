using SF.Feature.Handlebars.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Repositories
{
    public class HandlebarTemplateRepository : ModelRepository, IHandlebarTemplateRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new HandlebarTemplateModel();
            FillBaseProperties(model);

            var templateContent = model.Item.Fields["Content"].Value;
            model.HasTemplateContent = !string.IsNullOrEmpty(templateContent.Trim());
            
            return model;
        }
    }
}