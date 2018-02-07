using SF.Foundation.Components.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Repositories
{
    public class GenericVariantRepository : VariantsRepository,IGenericVariantRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new GenericVariantModel();
            FillBaseProperties(model);
            model.ContainerClass = model.Rendering.Parameters["containerClass"] ?? "generic-variant";
            return model;
        }
    }
}