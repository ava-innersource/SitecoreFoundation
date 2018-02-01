using SF.Feature.Handlebars.Constants;
using Sitecore.Data;
using Sitecore.XA.Foundation.Variants.Abstractions.Pipelines.ParseVariantFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Variants
{
    public class ParseHandlebarTemplate : ParseVariantFieldProcessor
    {
        public ParseHandlebarTemplate()
        {
        }

        public override ID SupportedTemplateId
        {
            get
            {
                return new ID(Templates.HandlebarVariantTemplate);
            }
        }

        public override void TranslateField(ParseVariantFieldArgs args)
        {
            args.TranslatedField = new HandlebarVariantTemplate(args.VariantItem)
            {
                ItemName = args.VariantItem.Name,
                Template = args.VariantItem[new ID(Fields.Template)],
                TemplateItem = args.VariantItem
            };
        }
    }
}