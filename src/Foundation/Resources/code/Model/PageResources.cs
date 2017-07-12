using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public class PageResources
    {
        public PageResources() : this(Sitecore.Context.Item)
        {

        }

        public PageResources(Item item)
        {
            if (item.HasField(Templates.PageResources.Fields.PageStyles))
            {
                MultilistField field = (MultilistField)item.Fields[Templates.PageResources.Fields.PageStyles];
                if (field.TargetIDs.Length > 0)
                {
                    this.PageCSS = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = Sitecore.Context.Database.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.PageCSS.Add(new Resource(resourceItem)); 
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("PageCSS item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.PageResources.Fields.PageScripts))
            {
                MultilistField field = (MultilistField)item.Fields[Templates.PageResources.Fields.PageScripts];
                if (field.TargetIDs.Length > 0)
                {
                    this.PageScripts = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = Sitecore.Context.Database.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.PageScripts.Add(new Resource(resourceItem)); 
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("PageScripts item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.PageResources.Fields.Minify))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.PageResources.Fields.Minify];
                this.Minify = field.Checked;
            }

            if (item.HasField(Templates.PageResources.Fields.BodyCSSOverride))
            {
                this.BodyCSSClassNameOverride = item.Fields[Templates.PageResources.Fields.BodyCSSOverride].Value;
            }

            if (item.HasField(Templates.PageResources.Fields.Header))
            {
                this.Header = item.Fields[Templates.PageResources.Fields.Header].Value;
            }
            if (item.HasField(Templates.PageResources.Fields.Footer))
            {
                this.Footer = item.Fields[Templates.PageResources.Fields.Footer].Value;
            }
        }

        public List<Resource> PageCSS { get; set; }
        public List<Resource> PageScripts { get; set; }
        public bool Minify { get; set; }
        public string BodyCSSClassNameOverride { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
    }
}
