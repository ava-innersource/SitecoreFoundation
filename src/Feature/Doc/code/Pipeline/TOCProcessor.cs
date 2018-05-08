using SF.Feature.Doc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Pipeline
{
    public class TOCProcessor : BaseDocProcessor
    {
        public override void Process(DocumentationArgs args)
        {
            var toc = new Section() { Content = RenderPartialView("/Views/Docs/TOC.cshtml", args.Document.Sections) };
            args.Document.Sections.Insert(1, toc);
        }
    }
}