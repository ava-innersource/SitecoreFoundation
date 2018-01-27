using SF.Feature.OwlCarousel.Models;
using SF.Feature.OwlCarousel.Repositories;
using SF.Foundation.Components;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.OwlCarousel.Controllers
{
    public class OwlCarouselController : StandardController
    {
        protected readonly IOwlCarouselRepository CarouselRepository;
        protected readonly IOwlCarouselItemRepository CarouselItemRepository;

        public OwlCarouselController(IOwlCarouselRepository repository, IOwlCarouselItemRepository itemRepository)
        {
            this.CarouselRepository = repository;
            this.CarouselItemRepository = itemRepository;
        }

        protected override object GetModel()
        {
            return CarouselRepository.GetModel();
        }

        public OwlCarouselItemModel GetCarouselItemModel()
        {
            return CarouselItemRepository.GetModel() as OwlCarouselItemModel;
        }

        public ActionResult CarouselItem()
        {
            var model = GetCarouselItemModel();

            return View(model);
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as OwlCarouselModel;

            model.ContainerClass = Sitecore.Context.PageMode.IsExperienceEditorEditing ? "owl-carousel" : "owl-carousel-page-Editor";
            
            var sb = new System.Text.StringBuilder();
            sb.Append("{ ");
            
            if (model.Rendering.Parameters.IsRenderingParameterChecked("Display Single Item"))
            {
                sb.Append(@"""singleItem"":true,");
            }
            else
            {
                var maxItems = model.Rendering.Parameters["Max Display Items"];
                if (!string.IsNullOrEmpty(maxItems))
                {
                    sb.Append(@"""items"":").Append(maxItems).Append(",");
                }

                var desktopResolution = model.Rendering.Parameters["Desktop Resolution"];
                var desktopItems = model.Rendering.Parameters["Desktop Items"];
                if (!string.IsNullOrEmpty(desktopResolution) && !string.IsNullOrEmpty(desktopItems))
                {
                    sb.Append(@"""itemsDesktop"":[").Append(desktopResolution).Append(",").Append(desktopItems).Append("],");
                }

                string smallDesktopResolution = model.Rendering.Parameters["Small Desktop Resolution"];
                string smallDesktopItems = model.Rendering.Parameters["Small Desktop Items"];
                if (!string.IsNullOrEmpty(smallDesktopResolution) && !string.IsNullOrEmpty(smallDesktopItems))
                {
                    sb.Append(@"""itemsDesktopSmall"":[").Append(smallDesktopResolution).Append(",").Append(smallDesktopItems).Append("],");
                }

                string tabletResolution = model.Rendering.Parameters["Tablet Resolution"];
                string tabletItems = model.Rendering.Parameters["Tablet Items"];
                if (!string.IsNullOrEmpty(tabletResolution) && !string.IsNullOrEmpty(tabletItems))
                {
                    sb.Append(@"""itemsTablet"":[").Append(tabletResolution).Append(",").Append(tabletItems).Append("],");
                }

                string smallTabletResolution = model.Rendering.Parameters["Tablet Small Resolution"];
                string smallTabletItems = model.Rendering.Parameters["Tablet Small Items"];
                if (!string.IsNullOrEmpty(smallTabletResolution) && !string.IsNullOrEmpty(smallTabletItems))
                {
                    sb.Append(@"""itemsTabletSmall"":[").Append(smallTabletResolution).Append(",").Append(smallTabletItems).Append("],");
                }

                string mobileResolution = model.Rendering.Parameters["Mobile Resolution"];
                string mobileItems = model.Rendering.Parameters["Mobile Items"];
                if (!string.IsNullOrEmpty(mobileResolution) && !string.IsNullOrEmpty(mobileItems))
                {
                    sb.Append(@"""itemsMobile"":[").Append(mobileResolution).Append(",").Append(mobileItems).Append("],");
                }
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Items Scale Up"))
            {
                sb.Append(@"""itemsScaleUp"":true,");
            }

            if (!model.Rendering.Parameters.IsRenderingParameterChecked("Pagination"))
            {
                sb.Append(@"""pagination"":false,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Show Pagination Numbers"))
            {
                sb.Append(@"""paginationNumbers"":true,");
            }

            var paginationSpeed = model.Rendering.Parameters["Pagination Speed"];
            if (!string.IsNullOrEmpty(paginationSpeed))
            {
                sb.Append(@"""paginationSpeed"":").Append(paginationSpeed).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Scroll Per Page"))
            {
                sb.Append(@"""scrollPerPage"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Navigation"))
            {
                sb.Append(@"""navigation"":true,");
            }

            string prevText = model.Rendering.Parameters["Previous Text"];
            string nextText = model.Rendering.Parameters["Next Text"];
            if (!string.IsNullOrEmpty(prevText) && !string.IsNullOrEmpty(nextText))
            {
                sb.Append(@"""navigationText"":[""").Append(prevText).Append(@""",""").Append(nextText).Append(@"""],");
            }

            string autoPlay = model.Rendering.Parameters["Auto Play"];
            if (!string.IsNullOrEmpty(autoPlay))
            {
                sb.Append(@"""autoPlay"":").Append(autoPlay).Append(",");
            }

            string slideSpeed = model.Rendering.Parameters["Slide Speed"];
            if (!string.IsNullOrEmpty(slideSpeed))
            {
                sb.Append(@"""slideSpeed"":").Append(slideSpeed).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Stop on Hover"))
            {
                sb.Append(@"""stopOnHover"":true,");
            }

            if (!model.Rendering.Parameters.IsRenderingParameterChecked("Rewind Nav"))
            {
                sb.Append(@"""rewindNav"":false,");
            }

            string transitionStyle = model.Rendering.Parameters["Transition Style"];
            if (!string.IsNullOrEmpty(transitionStyle))
            {
                sb.Append(@"""transitionStyle"":""").Append(transitionStyle).Append(@""",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Auto Height"))
            {
                sb.Append(@"""autoHeight"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Add Class Active"))
            {
                sb.Append(@"""addClassActive"":true,");
            }

            if (sb.Length > 2)
            {
                //remove last comma
                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("}");

            model.DataOptions = sb.ToString();
            
            return View(model);
        }
    }
}