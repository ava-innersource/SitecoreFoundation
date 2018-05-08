using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class RendererProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var renderingField = (MultilistField)item.Fields["Renderings"];
            var renderingItems = renderingField.GetItems();

            if (renderingField != null && renderingItems.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Renderings";
                this.SectionContent = RenderPartialView("/Views/Docs/Renderings.cshtml", renderingItems);
            }
        }
    }
}