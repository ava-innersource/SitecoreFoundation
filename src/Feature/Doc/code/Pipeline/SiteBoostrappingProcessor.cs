using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class SiteBoostrappingProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var siteBoostrappingField = (MultilistField)item.Fields["Site Boostrapping Rules"];
            var rules = siteBoostrappingField.GetItems();

            if (rules != null && rules.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Site Boostrapping Rules";
                this.SectionContent = RenderPartialView("/Views/Docs/SiteRules.cshtml", rules);
            }
        }
    }
}