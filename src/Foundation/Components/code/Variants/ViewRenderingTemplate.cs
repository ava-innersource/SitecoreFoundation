using Sitecore.Data.Items;
using Sitecore.XA.Foundation.RenderingVariants.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Variants
{
    public class ViewRenderingTemplate : RenderingVariantFieldBase
    {
        public ViewRenderingTemplate(Item variantItem) : base(variantItem)
        {
        }

        public string ViewPath { get; set; }

        public Item TemplateItem { get; set; }
    }
}