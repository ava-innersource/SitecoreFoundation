using SF.Foundation.Components.Constants;
using Sitecore.Data;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.ParseVariantFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Variants
{
    public class ParseViewRenderingTemplate : ParseVariantFieldProcessor
    {
        public ParseViewRenderingTemplate()
        {
        }

        public override ID SupportedTemplateId
        {
            get
            {
                return new ID(Templates.ViewRenderingTemplate);
            }
        }

        public override void TranslateField(ParseVariantFieldArgs args)
        {
            args.TranslatedField = new ViewRenderingTemplate(args.VariantItem)
            {
                ItemName = args.VariantItem.Name,
                ViewPath = args.VariantItem[new ID(Fields.ViewPath)],
                TemplateItem = args.VariantItem
            };
        }
    }
}