using SF.Foundation.Components.Constants;
using Sitecore.Data.Items;
using Sitecore.Publishing.Pipelines.GetItemReferences;
using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Publishing
{
    public class PublishDataSourceChildItems : GetItemReferencesProcessor
    {
        public override void Process(PublishItemContext context)
        {
            base.Process(context);
        }

        protected override List<Item> GetItemReferences(PublishItemContext context)
        {
            var relatedItems = new List<Item>();
            var item = context.PublishHelper.GetSourceItem(context.ItemId);

            if (item != null && !item.Empty)
            {
                if (item.IsDerived(new Sitecore.Data.ID(Templates.BasePublishChildrenAsRelatedItems)))
                {
                    RecurseChildren(relatedItems, item);
                }
            }
                
            return relatedItems;
        }

        private List<Item> RecurseChildren(List<Item> items, Item item)
        {
            if (item.HasChildren)
            {
                foreach (Item child in item.Children)
                {
                    items.Add(child);
                    
                    //Disabled Recursive behavior, as each child will go through the processor too
                    //If you want it to recurse to children's children, ensure each item inherits
                    //from Base Template. Can re-enable if you want it to do all regardless.
                    //items.AddRange(RecurseChildren(items, child));
                }
            }
            return items;
        }
    }
}