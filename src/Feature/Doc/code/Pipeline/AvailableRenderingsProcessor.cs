using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class AvailableRenderingsProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var availableRenderingsField = (MultilistField)item.Fields["Available Renderings"];
            var availableRenderings = availableRenderingsField.GetItems();

            if (availableRenderings != null && availableRenderings.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Available Renderings";
                this.SectionContent = RenderPartialView("/Views/Docs/AvailableRenderings.cshtml", availableRenderings);
            }
        }
    }
}