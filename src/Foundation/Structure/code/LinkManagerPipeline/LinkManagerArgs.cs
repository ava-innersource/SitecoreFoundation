using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Structure
{
    public class LinkManagerArgs : PipelineArgs
    {
        public Item Item { get; set; }

        public UrlOptions Options { get; set; }

        public string ReturnUrl { get; set; }

        public PipelineLinkProvider LinkProvider { get; set; }

    }
}