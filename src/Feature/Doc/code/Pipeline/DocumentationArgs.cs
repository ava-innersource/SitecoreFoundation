using SF.Feature.Doc.Model;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Pipeline
{
    public class DocumentationArgs : PipelineArgs
    {
        public DocumentationArgs(Item item)
        {
            Item = item;
            Document = new Document();
        }

        public Item Item { get; set; }

        public Document Document { get; set; }
    }
}