using SF.Foundation.Components.Repositories;
using Sitecore.XA.Foundation.RenderingVariants.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Foundation.Components.Controllers
{
    public class GenericVariantController : VariantsController
    {
        protected readonly IGenericVariantRepository GenericVariantRepository;
        public GenericVariantController(IGenericVariantRepository repository)
        {
            GenericVariantRepository = repository;
        }
        protected override object GetModel()
        {
            return GenericVariantRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = GetModel();
            return View(model);
        }

    }
}