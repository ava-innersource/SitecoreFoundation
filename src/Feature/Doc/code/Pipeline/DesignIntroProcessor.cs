using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class DesignIntroProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            this.SectionName = "Design Overview";
            this.IncludeInTOC = true;
            this.SectionContent = RenderPartialView("/Views/Docs/DesignIntro.cshtml", item);
        }
    }
}