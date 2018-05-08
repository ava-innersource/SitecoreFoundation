using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class TemplateProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var templateField = (MultilistField)item.Fields["Templates"];
            var items = templateField.GetItems();
            var templates = new List<TemplateItem>();
            foreach (var tempItem in items)
            {
                templates.Add(new TemplateItem(tempItem));
            }

            if (templates.Count > 0)
            {
                this.SectionName = "Templates";
                this.IncludeInTOC = true;
                this.SectionContent = RenderPartialView("/Views/Docs/Templates.cshtml", templates);
            }
        }
    }
}