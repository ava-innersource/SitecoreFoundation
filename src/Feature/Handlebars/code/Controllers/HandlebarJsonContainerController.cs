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
    public class HandlebarJsonContainerController : StandardController
    {
        protected readonly IHandlebarJsonContainerRepository HandlebarContainerJsonRepository;

        public HandlebarJsonContainerController(IHandlebarJsonContainerRepository repository)
        {
            this.HandlebarContainerJsonRepository = repository;
        }

        protected override object GetModel()
        {
            return HandlebarContainerJsonRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as HandlebarJsonContainerModel;
            HandlebarManager.SetupJsonContainer(model.JsonUrl);
            return View(model);
        }
    }
}