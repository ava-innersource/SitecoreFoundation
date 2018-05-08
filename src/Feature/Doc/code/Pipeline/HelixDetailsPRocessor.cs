using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class HelixDetailsProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            this.SectionName = "Helix Details";
            this.IncludeInTOC = true;
            this.SectionContent = RenderPartialView("/Views/Docs/HelixDetails.cshtml", item);
        }
    }
}