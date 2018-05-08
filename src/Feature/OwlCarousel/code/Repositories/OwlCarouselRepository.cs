using SF.Feature.OwlCarousel.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.OwlCarousel.Repositories
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   An owl carousel repository. </summary>
    ///
    /// <remarks>   David San Filippo, 5/7/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    public class OwlCarouselRepository : ModelRepository, IOwlCarouselRepository
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
            var model = new OwlCarouselModel();
            FillBaseProperties(model);

            
            return model;
        }
    }
}