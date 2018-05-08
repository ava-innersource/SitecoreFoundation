using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class PlaceholderProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var placeholderField = (MultilistField)item.Fields["Placeholders"];
            var placeholders = placeholderField.GetItems();

            if (placeholders != null && placeholders.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Placeholders";
                this.SectionContent = RenderPartialView("/Views/Docs/Placeholders.cshtml", placeholders);
            }
        }
    }
}