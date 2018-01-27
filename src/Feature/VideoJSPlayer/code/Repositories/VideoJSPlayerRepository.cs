using SF.Feature.VideoJSPlayer.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.VideoJSPlayer.Repositories
{
    public class VideoJSPlayerRepository : ModelRepository, IVideoJSPlayerRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new VideoJSPlayerModel();
            FillBaseProperties(model);

            var mp4 = (Sitecore.Data.Fields.LinkField)model.Item.Fields["mp4"];
            var webm = (Sitecore.Data.Fields.LinkField)model.Item.Fields["webm"];
            var ogg = (Sitecore.Data.Fields.LinkField)model.Item.Fields["ogg"];

            model.Mp4Url = mp4.GetFriendlyUrl();
            model.WebmUrl = webm.GetFriendlyUrl();
            model.OggUrl = ogg.GetFriendlyUrl();

            model.AutoPlay = ((Sitecore.Data.Fields.CheckboxField)model.Item.Fields["AutoPlay"]).Checked;
            model.Preload = model.Item.Fields["Preload"].Value;
            model.Poster = ((Sitecore.Data.Fields.LinkField)model.Item.Fields["Poster"]).GetFriendlyUrl();
            model.Loop = ((Sitecore.Data.Fields.CheckboxField)model.Item.Fields["Loop"]).Checked;

            int width = 0;
            int.TryParse(model.Item.Fields["Width"].Value, out width);
            model.Width = width;
            int height = 0;
            int.TryParse(model.Item.Fields["Height"].Value, out height);
            model.Height = height;

            model.CenterBigPlayButton = ((Sitecore.Data.Fields.CheckboxField)model.Item.Fields["CenterBigPlayButton"]).Checked;

            model.NotSupportedMessage = model.Item.Fields["NotSupportedMessage"].Value;
            model.AdditionalSetupOptions = model.Item.Fields["AdditionalSetupOptions"].Value;

            model.VideoTagCSS = @"video-js vjs-default-skin";
            if (model.CenterBigPlayButton)
            {
                model.VideoTagCSS += " vjs-big-play-centered";
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("controls ");
            if (model.AutoPlay)
            {
                sb.Append("autoplay ");
            }

            if (!string.IsNullOrEmpty(model.Preload))
            {
                sb.Append(@"preload=").Append(model.Preload).Append(@" ");
            }

            if (!string.IsNullOrEmpty(model.Poster))
            {
                sb.Append(@"poster=").Append(model.Poster).Append(@" ");
            }

            if (model.Loop)
            {
                sb.Append("loop ");
            }

            if (width > 0)
            {
                sb.Append(@"width=").Append(model.Width).Append(@" ");
            }

            if (height > 0)
            {
                sb.Append(@"height=").Append(model.Height).Append(@" ");
            }

            if (!string.IsNullOrEmpty(model.AdditionalSetupOptions))
            {
                sb.Append(@"data-setup='").Append(model.AdditionalSetupOptions).Append("' ");
            }

            model.VideoJSAttributes = sb.ToString();

            return model;
        }
    }
}