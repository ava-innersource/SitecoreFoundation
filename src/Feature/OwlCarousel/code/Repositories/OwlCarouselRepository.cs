using SF.Feature.OwlCarousel.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.OwlCarousel.Repositories
{
    public class OwlCarouselRepository : ModelRepository, IOwlCarouselRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new OwlCarouselModel();
            FillBaseProperties(model);

            
            return model;
        }
    }
}