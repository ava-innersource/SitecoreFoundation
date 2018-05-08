using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class BranchTemplateProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var templateField = (MultilistField)item.Fields["Branch Templates"];
            var items = templateField.GetItems();
            var templates = new List<BranchItem>();
            foreach (var tempItem in items)
            {
                templates.Add(new BranchItem(tempItem));
            }

            if (templates.Count > 0)
            {
                this.SectionName = "Branch Templates";
                this.IncludeInTOC = true;
                this.SectionContent = RenderPartialView("/Views/Docs/BranchTemplates.cshtml", templates);
            }
        }
    }
}