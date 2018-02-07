using SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Models;
using SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.HTMLComponents.Areas.SitecoreFoundation.Controllers
{
    public class HorizontalRuleController : StandardController
    {
        protected readonly IHorizontalRuleRepository HorizontalRuleRepository;

        public HorizontalRuleController(IHorizontalRuleRepository repository)
        {
            this.HorizontalRuleRepository = repository;
        }

        protected override object GetModel()
        {
            return HorizontalRuleRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as HorizontalRuleModel;
            return View(model);
        }
    }
}