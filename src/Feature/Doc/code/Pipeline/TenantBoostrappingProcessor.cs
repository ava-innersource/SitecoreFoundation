using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Pipeline
{
    public class TenantBoostrappingProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var boostrappingField = (MultilistField)item.Fields["Tenant Boostrapping Rules"];
            var rules = boostrappingField.GetItems();

            if (rules != null && rules.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Tenant Boostrapping Rules";
                this.SectionContent = RenderPartialView("/Views/Docs/TenantRules.cshtml", rules);
            }
        }
    }
}