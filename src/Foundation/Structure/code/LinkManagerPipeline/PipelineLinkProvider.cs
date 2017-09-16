using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Structure
{
    public class PipelineLinkProvider : LinkProvider
    {
        public override string GetItemUrl(Sitecore.Data.Items.Item item, UrlOptions options)
        {
            var args = new LinkManagerArgs
            {
                Item = item,
                Options = options,
                LinkProvider = this
            };
            Sitecore.Pipelines.CorePipeline.Run("processCustomLinkRules", args);

            if (!string.IsNullOrEmpty(args.ReturnUrl))
            {
                return args.ReturnUrl;
            }

            return base.GetItemUrl(item, options);
        }
    }
}