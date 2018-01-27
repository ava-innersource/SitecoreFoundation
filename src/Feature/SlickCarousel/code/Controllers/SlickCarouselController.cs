using SF.Feature.SlickCarousel.Models;
using SF.Feature.SlickCarousel.Repositories;
using SF.Foundation.Components;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.SlickCarousel.Controllers
{
    public class SlickCarouselController : StandardController
    {
        protected readonly ISlickCarouselRepository CarouselRepository;
        protected readonly ISlickCarouselItemRepository CarouselItemRepository;

        public SlickCarouselController(ISlickCarouselRepository repository, ISlickCarouselItemRepository itemRepository)
        {
            this.CarouselRepository = repository;
            this.CarouselItemRepository = itemRepository;
        }

        protected override object GetModel()
        {
            return CarouselRepository.GetModel();
        }

        public SlickCarouselItemModel GetCarouselItemModel()
        {
            return CarouselItemRepository.GetModel() as SlickCarouselItemModel;
        }

        public ActionResult CarouselItem()
        {
            var model = GetCarouselItemModel();

            return View(model);
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as SlickCarouselModel;

            model.ContainerClass = Sitecore.Context.PageMode.IsExperienceEditorEditing ? "slick-carousel" : "slick-carousel-page-Editor";

            var sb = new System.Text.StringBuilder();
            sb.Append("{");

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Display Single Item"))
            {
                sb.Append(@"""slidesToShow"":1,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Infinite"))
            {
                sb.Append(@"""infinite"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Accessibility"))
            {
                sb.Append(@"""accessibility"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("AdaptiveHeight"))
            {
                sb.Append(@"""adaptiveHeight"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Auto Play"))
            {
                sb.Append(@"""autoplay"":true,");
            }

            var speed = model.Rendering.Parameters["Auto Play Speed"];
            if (!String.IsNullOrEmpty(speed))
            {
                sb.Append(@"""autoplaySpeed"":").Append(speed).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Arrows"))
            {
                sb.Append(@"""arrows"":true,");
            }

            var navForClass = model.Rendering.Parameters["Navigation For Class"];
            if (!String.IsNullOrEmpty(navForClass))
            {
                sb.Append(@"""asNavFor"":").Append(navForClass).Append(",");
            }

            var appendArrows = model.Rendering.Parameters["Append Arrows"];
            if (!String.IsNullOrEmpty(appendArrows))
            {
                sb.Append(@"""appendArrows"":").Append(appendArrows).Append(",");
            }

            var appendDots = model.Rendering.Parameters["Append Dots"];
            if (!String.IsNullOrEmpty(appendDots))
            {
                sb.Append(@"""appendDots"":").Append(appendDots).Append(",");
            }

            var prevArrow = model.Rendering.Parameters["Previous Arrow"];
            if (!String.IsNullOrEmpty(prevArrow))
            {
                sb.Append(@"""prevArrow"":").Append(prevArrow).Append(",");
            }

            var nextArrow = model.Rendering.Parameters["Next Arrow"];
            if (!String.IsNullOrEmpty(nextArrow))
            {
                sb.Append(@"""nextArrow"":").Append(nextArrow).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Center Mode"))
            {
                sb.Append(@"""centerMode"":true,");
            }

            var centerPadding = model.Rendering.Parameters["Center Padding"];
            if (!String.IsNullOrEmpty(centerPadding))
            {
                sb.Append(@"""centerPadding"":").Append(centerPadding).Append(",");
            }

            var cssEase = model.Rendering.Parameters["CSS Ease"];
            if (!String.IsNullOrEmpty(cssEase))
            {
                sb.Append(@"""cssEase"":").Append(cssEase).Append(",");
            }

            var customPaging = model.Rendering.Parameters["Custom Paging"];
            if (!String.IsNullOrEmpty(customPaging))
            {
                sb.Append(@"""cssEase"":").Append(customPaging).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Show Dots"))
            {
                sb.Append(@"""dots"":true,");
            }

            var dotsClass = model.Rendering.Parameters["Dots Class"];
            if (!String.IsNullOrEmpty(dotsClass))
            {
                sb.Append(@"""dotsClass"":").Append(dotsClass).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Draggable"))
            {
                sb.Append(@"""draggable"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Fade"))
            {
                sb.Append(@"""fade"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Focus on Select"))
            {
                sb.Append(@"""focusOnSelect"":true,");
            }

            var easing = model.Rendering.Parameters["Easing"];
            if (!String.IsNullOrEmpty(easing))
            {
                sb.Append(@"""easing"":").Append(easing).Append(",");
            }

            var edgeFriction = model.Rendering.Parameters["Edge Friction"];
            if (!String.IsNullOrEmpty(edgeFriction))
            {
                sb.Append(@"""edgeFriction"":").Append(edgeFriction).Append(",");
            }

            var initialSlide = model.Rendering.Parameters["Initial Slide"];
            if (!String.IsNullOrEmpty(initialSlide))
            {
                sb.Append(@"""initialSlide"":").Append(initialSlide).Append(",");
            }

            var lazyLoad = model.Rendering.Parameters["Lazy Load"];
            if (!String.IsNullOrEmpty(lazyLoad))
            {
                sb.Append(@"""lazyLoad"":").Append(lazyLoad).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Mobile First"))
            {
                sb.Append(@"""Mobile First"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Pause on Focus"))
            {
                sb.Append(@"""pauseOnFocus"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Pause on Hover"))
            {
                sb.Append(@"""pauseOnHover"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Pause on Dots Hover"))
            {
                sb.Append(@"""pauseOnDotsHover"":true,");
            }

            var respondTo = model.Rendering.Parameters["Respond To"];
            if (!String.IsNullOrEmpty(respondTo))
            {
                sb.Append(@"""respondTo"":").Append(respondTo).Append(",");
            }

            var responsive = model.Rendering.Parameters["Responsive"];
            if (!String.IsNullOrEmpty(responsive))
            {
                sb.Append(@"""responsive"":").Append(responsive).Append(",");
            }

            var rows = model.Rendering.Parameters["Rows"];
            if (!String.IsNullOrEmpty(rows))
            {
                sb.Append(@"""rows"":").Append(rows).Append(",");
            }

            var slide = model.Rendering.Parameters["Slide"];
            if (!String.IsNullOrEmpty(slide))
            {
                sb.Append(@"""slide"":").Append(slide).Append(",");
            }

            var slidesPerRow = model.Rendering.Parameters["Slides per Row"];
            if (!String.IsNullOrEmpty(slidesPerRow))
            {
                sb.Append(@"""slidesPerRow"":").Append(slidesPerRow).Append(",");
            }

            var slidesToShow = model.Rendering.Parameters["Slides To Show"];
            if (!String.IsNullOrEmpty(slidesToShow))
            {
                sb.Append(@"""slidesToShow"":").Append(slidesToShow).Append(",");
            }

            var slidesToScroll = model.Rendering.Parameters["Slides To Scroll"];
            if (!String.IsNullOrEmpty(slidesToScroll))
            {
                sb.Append(@"""slidesToScroll"":").Append(slidesToScroll).Append(",");
            }

            var animationSpeed = model.Rendering.Parameters["Animation Speed"];
            if (!String.IsNullOrEmpty(animationSpeed))
            {
                sb.Append(@"""speed"":").Append(animationSpeed).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Swipe"))
            {
                sb.Append(@"""swipe"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Swipe To Slide"))
            {
                sb.Append(@"""swipeToSlide"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Touch Move"))
            {
                sb.Append(@"""touchMove"":true,");
            }

            var touchThreshold = model.Rendering.Parameters["Touch Threshold"];
            if (!String.IsNullOrEmpty(touchThreshold))
            {
                sb.Append(@"""touchThreshold"":").Append(touchThreshold).Append(",");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("CSS"))
            {
                sb.Append(@"""useCSS"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Transform"))
            {
                sb.Append(@"""useTransform"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Variable Width"))
            {
                sb.Append(@"""variableWidth"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Vertical"))
            {
                sb.Append(@"""vertical"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Vertical Swipe"))
            {
                sb.Append(@"""verticalSwiping"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Vertical Swipe"))
            {
                sb.Append(@"""verticalSwiping"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Right To Left"))
            {
                sb.Append(@"""rtl"":true,");
            }

            if (model.Rendering.Parameters.IsRenderingParameterChecked("Wait For Animation"))
            {
                sb.Append(@"""waitForAnimate"":true,");
            }

            var zIndex = model.Rendering.Parameters["zIndex"];
            if (!String.IsNullOrEmpty(zIndex))
            {
                sb.Append(@"""zIndex"":").Append(zIndex).Append(",");
            }


            if (sb.Length > 2)
            {
                //removes last comma
                sb.Length -= 1;
            }

            sb.Append("}");
            model.DataOptions = sb.ToString();
            
            return View(model);
        }
    }
}