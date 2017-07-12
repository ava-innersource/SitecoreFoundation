using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public class SiteResources : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public SiteResources()
        {

        }

        public SiteResources(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("SiteResources: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public SiteResources(string path)
        {
            Sitecore.Diagnostics.Log.Info("SiteResources: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public SiteResources(Item item)
        {
            Load(item);
        }

        public void Load(Item item)
        {
            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");
            this.ConfigItem = item;
            if (item != null)
            {
                this.SiteConfigurationId = item.ID.Guid;
            }

            if (item.HasField(Templates.SiteResources.Fields.SiteStyles))
            {
                Sitecore.Diagnostics.Log.Debug("SiteCSS found!", this);

                MultilistField field = (MultilistField)item.Fields[Templates.SiteResources.Fields.SiteStyles];

                Sitecore.Diagnostics.Log.Debug("Fields length: " + field.Count.ToString(), this);

                if (field.TargetIDs.Length > 0)
                {
                    this.SiteCSS = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = db.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.SiteCSS.Add(new Resource(resourceItem));
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("SiteCSS item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.SiteResources.Fields.SiteScripts))
            {
                MultilistField field = (MultilistField)item.Fields[Templates.SiteResources.Fields.SiteScripts];
                if (field.TargetIDs.Length > 0)
                {
                    this.SiteScripts = new List<Resource>();
                    foreach (var id in field.TargetIDs)
                    {
                        var resourceItem = db.Items[id];
                        // in case referenced item was deleted and warning dialog ignored, causing broken link
                        if (resourceItem != null)
                        {
                            this.SiteScripts.Add(new Resource(resourceItem));
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Warn("SiteScripts item " + id.ToString() + " not found in database, skipping", this);
                        }
                    }
                }
            }

            if (item.HasField(Templates.SiteResources.Fields.Minify))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteResources.Fields.Minify];
                this.Minify = field.Checked;
            }

            if (item.HasField(Templates.SiteResources.Fields.AddBodyClass))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteResources.Fields.AddBodyClass];
                this.InsertItemNameAsBodyCSSClass = field.Checked;
            }

            if (item.HasField(Templates.SiteResources.Fields.Header))
            {
                this.Header = item.Fields[Templates.SiteResources.Fields.Header].Value;
            }

            if (item.HasField(Templates.SiteResources.Fields.Footer))
            {
                this.Footer = item.Fields[Templates.SiteResources.Fields.Footer].Value;
            }
        }

        #region Site Resources

        public List<Resource> SiteCSS { get; set; }
        public List<Resource> SiteScripts { get; set; }
        public bool Minify { get; set; }
        public bool InsertItemNameAsBodyCSSClass { get; set; }

        #endregion

        #region Header and Footer Injection

        public string Header { get; set; }
        public string Footer { get; set; }
        #endregion
    }
}
