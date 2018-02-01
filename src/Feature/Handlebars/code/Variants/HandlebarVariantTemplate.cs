using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Variants
{
    public class HandlebarVariantTemplate : RenderingVariantFieldBase
    {
        public HandlebarVariantTemplate(Item variantItem) : base(variantItem)
        {
        }

        public string Template
        {
            get;
            set;
        }

        public Item TemplateItem { get; set; }
    }
}