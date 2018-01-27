using Sitecore.XA.Foundation.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.VideoJSPlayer.Models
{
    public class VideoJSPlayerModel : RenderingModelBase
    {
        public string Mp4Url { get; set; }
        public string WebmUrl { get; set; }
        public string OggUrl { get; set; }

        public bool AutoPlay { get; set; }
        public string Preload { get; set; }

        public string Poster { get; set; }

        public bool Loop { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool CenterBigPlayButton { get; set; }
        public string NotSupportedMessage { get; set; }
        public string AdditionalSetupOptions { get; set; }


        public string VideoId { get; set; }

        public string VideoJSAttributes { get; set; }

        public string VideoTagCSS { get; set; }
    }
}