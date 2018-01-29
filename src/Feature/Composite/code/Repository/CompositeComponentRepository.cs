using SF.Feature.Composite.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Composite.Repositories
{
    public class CompositeComponentRepository : ModelRepository, ICompositeComponentRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new CompositeComponentModel();
            FillBaseProperties(model);


            return model;
        }
    }
}