using SF.Feature.OwlCarousel.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.OwlCarousel.Repositories
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An owl carousel item repository. </summary>
    ///
    /// <remarks>   David San Filippo, 5/7/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class OwlCarouselItemRepository : ModelRepository, IOwlCarouselItemRepository
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the model. </summary>
        ///
        /// <remarks>   David San Filippo, 5/7/2018. </remarks>
        ///
        /// <returns>   The model. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public override IRenderingModelBase GetModel()
        {
            var model = new OwlCarouselItemModel();
            FillBaseProperties(model);


            return model;
        }
    }
}