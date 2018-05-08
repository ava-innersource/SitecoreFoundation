using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class PickListProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var pickListField = (MultilistField)item.Fields["Pick Lists"];
            var pickLists = pickListField.GetItems();

            if (pickLists != null && pickLists.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Pick Lists";
                this.SectionContent = RenderPartialView("/Views/Docs/PickLists.cshtml", pickLists);
            }
        }
    }
}