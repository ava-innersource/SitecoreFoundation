using SF.Feature.Handlebars.Models;
using SF.Feature.Handlebars.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.Handlebars.Controllers
{
    public class HandlebarContainerController : StandardController
    {
        protected readonly IHandlebarContainerRepository HandlebarContainerRepository;

        public HandlebarContainerController(IHandlebarContainerRepository repository)
        {
            this.HandlebarContainerRepository = repository;
        }

        protected override object GetModel()
        {
            return HandlebarContainerRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as HandlebarContainerModel;
            HandlebarManager.SetupContainer(model.Item);
            return View(model);
        }
    }
}