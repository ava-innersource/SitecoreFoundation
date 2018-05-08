using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Pipeline
{
    public class VariantProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var variantField = (MultilistField)item.Fields["Variants"];
            var variants = variantField.GetItems();

            if (variants != null && variants.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Variants";
                this.SectionContent = RenderPartialView("/Views/Docs/Variants.cshtml", variants);
            }
        }
    }
}