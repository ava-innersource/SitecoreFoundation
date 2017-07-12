using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public class Design
    {
        public Design() : this(Sitecore.Context.Item)
        {

        }

        public Design(Item item)
        {
            if (item.HasField(Templates.Design.Fields.Styles))
            {
                MultilistField field = (MultilistField)item.Fields[Templates.Design.Fields.Styles];
                if (field.TargetIDs.Length > 0)
                {
                    this.Styles = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = Sitecore.Context.Database.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.Styles.Add(new Resource(resourceItem)); 
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("Styles item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.Design.Fields.Scripts))
            {
                MultilistField field = (MultilistField)item.Fields[Templates.Design.Fields.Scripts];
                if (field.TargetIDs.Length > 0)
                {
                    this.Scripts = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = Sitecore.Context.Database.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.Scripts.Add(new Resource(resourceItem)); 
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("Scripts item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.Design.Fields.Minify))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.Design.Fields.Minify];
                this.Minify = field.Checked;
            }

            
        }

        public List<Resource> Styles { get; set; }
        public List<Resource> Scripts { get; set; }
        public bool Minify { get; set; }
 
    }
}
