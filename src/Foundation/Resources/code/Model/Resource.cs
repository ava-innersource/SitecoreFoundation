using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public class Resource
    {
        public Resource() : this(Sitecore.Context.Item)
        {

        }

        public Resource(Item item)
        {
            if (item != null)
            {
                this.ResourceId = item.ID.Guid;
            }

            

            if (item.HasField(Templates.Resource.Fields.Name))
            {
                this.Name = item.Fields[Templates.Resource.Fields.Name].Value;
            }

            if (item.HasField(Templates.Resource.Fields.ContentFieldName))
            {
                this.Content = item.Fields[Templates.Resource.Fields.ContentFieldName].Value;
            }
            else
            {
                Sitecore.Diagnostics.Log.Warn("Warning, item has no content field:  " + this.ResourceId.ToString(), this);
                this.Content = "\\* No Content Item Field Found *\\";

                //attempt reload
                item.Fields.ReadAll();

                if (item.HasField(Templates.Resource.Fields.ContentFieldName))
                {
                    this.Content = item.Fields[Templates.Resource.Fields.ContentFieldName].Value;
                    Sitecore.Diagnostics.Log.Warn("After reloading Item, field now exists with value", this);
                }

            }
        }

        public Guid ResourceId {get;set;}
        public string Name { get; set; }
        public string Content { get; set; }
        
    }
}
