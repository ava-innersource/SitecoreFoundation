using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class DictionaryItemProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var dictionaryField = (MultilistField)item.Fields["Dictionary Items"];
            var entries = dictionaryField.GetItems();

            if (entries != null && entries.Count() > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Dictionary Entries";
                this.SectionContent = RenderPartialView("/Views/Docs/Dictionary.cshtml", entries);
            }
        }
    }
}