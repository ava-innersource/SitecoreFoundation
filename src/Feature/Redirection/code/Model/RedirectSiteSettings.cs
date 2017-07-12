using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using SF.Foundation.Configuration;

namespace SF.Feature.Redirection
{
    public class RedirectSiteSettings : ISiteSettings
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public RedirectSiteSettings()
        {

        }

        public RedirectSiteSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("RedirectSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public RedirectSiteSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("RedirectSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public RedirectSiteSettings(Item item)
        {
            Load(item);
        }

        public void Load(Item item)
        {
            var db = item.Database;

            this.ConfigItem = item;
            if (item != null)
            {
                this.SiteConfigurationId = item.ID.Guid;
            }

            if (item.HasField(Templates.SiteRedirectionSettings.Fields.ForceHttps))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteRedirectionSettings.Fields.ForceHttps];
                this.ForceHtps = field.Checked;
            }

            if (item.HasField(Templates.SiteRedirectionSettings.Fields.ForceWWW))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteRedirectionSettings.Fields.ForceWWW];
                this.ForceWWW = field.Checked;
            }

            if (item.HasField(Templates.SiteRedirectionSettings.Fields.ApplyRulesForAllRequests))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteRedirectionSettings.Fields.ApplyRulesForAllRequests];
                this.ApplyRulesForAllRequests = field.Checked;
            }

            if (item.HasField(Templates.SiteRedirectionSettings.Fields.TakeSiteOffline))
            {
                CheckboxField field = (CheckboxField)item.Fields[Templates.SiteRedirectionSettings.Fields.TakeSiteOffline];
                this.TakeSiteOffline = field.Checked;
            }

            if (item.HasField(Templates.SiteRedirectionSettings.Fields.SiteOfflinePage))
            {
                this.SiteOfflinePage = item.Fields[Templates.SiteRedirectionSettings.Fields.SiteOfflinePage].Value;                
            }
        }

        #region Redirect Settings

        public bool ForceHtps { get; set; }
        public bool ForceWWW { get; set; }

        public bool ApplyRulesForAllRequests { get; set; }

        public bool TakeSiteOffline { get; set; }

        public string SiteOfflinePage { get; set; }
        #endregion
    }
}
