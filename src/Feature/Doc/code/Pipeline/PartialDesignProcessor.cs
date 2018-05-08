using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Pipeline
{
    public class PartialDesignProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var partialsField = (MultilistField)item.Fields["Partials"];
            var partials = partialsField.GetItems();

            if (partials != null && partials.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Partials";
                this.SectionContent = RenderPartialView("/Views/Docs/Partials.cshtml", partials);
            }
        }
    }
}