using SF.Feature.VideoJSPlayer.Models;
using SF.Feature.VideoJSPlayer.Repositories;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.VideoJSPlayer.Controllers
{
    public class VideoJSPlayerController : StandardController
    {
        protected readonly IVideoJSPlayerRepository VideoJSPlayerRepository;

        public VideoJSPlayerController(IVideoJSPlayerRepository repository)
        {
            this.VideoJSPlayerRepository = repository;
        }

        protected override object GetModel()
        {
            return VideoJSPlayerRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as VideoJSPlayerModel;

            model.VideoId = "r" + model.Rendering.UniqueId.Guid.ToString("N");

            return View(model);
        }
    }
}