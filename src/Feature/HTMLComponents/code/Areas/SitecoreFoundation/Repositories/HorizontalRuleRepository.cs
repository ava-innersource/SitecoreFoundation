using SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Repositories
{
    public class HorizontalRuleRepository : ModelRepository, IHorizontalRuleRepository
    {
        public override IRenderingModelBase GetModel()
    {
        var model = new HorizontalRuleModel();
        FillBaseProperties(model);


        return model;
    }
    }
}