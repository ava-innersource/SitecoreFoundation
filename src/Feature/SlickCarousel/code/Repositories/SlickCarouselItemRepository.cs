using SF.Feature.SlickCarousel.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.SlickCarousel.Repositories
{
    public class SlickCarouselItemRepository : ModelRepository, ISlickCarouselItemRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new SlickCarouselItemModel();
            FillBaseProperties(model);


            return model;
        }
    }
}