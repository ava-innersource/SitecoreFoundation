using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Foundation.Multisite
{
    public class MultisitePageSettings : ISiteSettings  
    {
        public Item ConfigItem { get; set; }
        public Guid SiteConfigurationId { get; set; }

        public MultisitePageSettings()
        {

        }

        public MultisitePageSettings(Guid id)
        {
            Sitecore.Diagnostics.Log.Info("ErrorHandlingSiteSettings: Guid:" + id, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);
            var dataId = new Sitecore.Data.ID(id);
            var item = db.GetItem(dataId);
            Load(item);
        }

        public MultisitePageSettings(string path)
        {
            Sitecore.Diagnostics.Log.Info("ErrorHandlingSiteSettings: path:" + path, this);

            var db = Sitecore.Context.Database ?? Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Diagnostics.Log.Info("Current DB Context:" + db.Name, this);

            var item = db.GetItem(path);
            Load(item);
        }

        public MultisitePageSettings(Item item)
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


            if (item.HasField(Templates.MultisitePageSettings.Fields.ItemNotFoundPage) && !string.IsNullOrEmpty(item.Fields[Templates.MultisitePageSettings.Fields.ItemNotFoundPage].Value))
            {
                var pageItem = db.GetItem(new Sitecore.Data.ID(item.Fields[Templates.MultisitePageSettings.Fields.ItemNotFoundPage].Value));
                if (pageItem != null)
                {
                    this.ItemNotFoundPage = pageItem.GetItemUrl();
                }
            }

            if (item.HasField(Templates.MultisitePageSettings.Fields.LayoutNotFoundPage) && !string.IsNullOrEmpty(item.Fields[Templates.MultisitePageSettings.Fields.LayoutNotFoundPage].Value))
            {
                var pageItem = db.GetItem(new Sitecore.Data.ID(item.Fields[Templates.MultisitePageSettings.Fields.LayoutNotFoundPage].Value));
                if (pageItem != null)
                {
                    this.LayoutNotFoundPage = pageItem.GetItemUrl();
                }
            }

            if (item.HasField(Templates.MultisitePageSettings.Fields.NoAccessPage) && !string.IsNullOrEmpty(item.Fields[Templates.MultisitePageSettings.Fields.NoAccessPage].Value))
            {
                var pageItem = db.GetItem(new Sitecore.Data.ID(item.Fields[Templates.MultisitePageSettings.Fields.NoAccessPage].Value));
                if (pageItem != null)
                {
                    this.NoAccessPage = pageItem.GetItemUrl();
                }
            }

            if (item.HasField(Templates.MultisitePageSettings.Fields.LoginPage) && !string.IsNullOrEmpty(item.Fields[Templates.MultisitePageSettings.Fields.LoginPage].Value))
            {
                var pageItem = db.GetItem(new Sitecore.Data.ID(item.Fields[Templates.MultisitePageSettings.Fields.LoginPage].Value));
                if (pageItem != null)
                {
                    this.LoginPage = pageItem.GetItemUrl();
                }
            }

            if (item.HasField(Templates.MultisitePageSettings.Fields.ErrorPage) && !string.IsNullOrEmpty(item.Fields[Templates.MultisitePageSettings.Fields.ErrorPage].Value))
            {
                var pageItem = db.GetItem(new Sitecore.Data.ID(item.Fields[Templates.MultisitePageSettings.Fields.ErrorPage].Value));
                if (pageItem != null)
                {
                    this.ErrorPage = pageItem.GetItemUrl();
                }
            }
        }



        #region Error Handling Properties

        public string ItemNotFoundPage { get; set; }
        public string LayoutNotFoundPage { get; set; }
        public string NoAccessPage { get; set; }
        public string LoginPage { get; set; }
        public string ErrorPage { get; set; }

        #endregion
    }
}
