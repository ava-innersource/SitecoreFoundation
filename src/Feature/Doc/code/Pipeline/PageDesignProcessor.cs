using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class PageDesignProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var pageDesignField = (MultilistField)item.Fields["Page Designs"];
            var pageDesigns = pageDesignField.GetItems();

            if (pageDesigns != null && pageDesigns.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Page Designs";
                this.SectionContent = RenderPartialView("/Views/Docs/PageDesigns.cshtml", pageDesigns);
            }
        }
    }
}