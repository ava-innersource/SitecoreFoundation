using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Resources
{
    public class ResourceWatcher : FileWatcher
    {
        private string masterDB;
        private string publishDB;
        private bool autoPublish;

        public ResourceWatcher() : base("watchers/resources")
        {
            masterDB = Sitecore.Configuration.Settings.GetSetting("SF.ResourceFileManager.MasterDB");
            publishDB = Sitecore.Configuration.Settings.GetSetting("SF.ResourceFileManager.PublishDB");
            autoPublish = bool.Parse(Sitecore.Configuration.Settings.GetSetting("SF.ResourceFileManager.AutoPublish"));
        }

        private Sitecore.Data.Database getDB()
        {
            return Sitecore.Data.Database.GetDatabase(masterDB);
        }

        private string getPathTo(string[] paths, int index)
        {
            return "/sitecore/content/" + string.Join("/", paths.Take(index + 1));
        }

        private Item AddFolder(Item parent, string folderName, string templateID)
        {
            using (new SecurityDisabler())
            {
                return parent.Add(folderName, new TemplateID(new ID(templateID)));
            }
        }

        protected override void Created(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return;
            }

            //remove path to root. assume mapped to glboal folder
            var relativePath = fullPath.Substring(fullPath.IndexOf("\\global\\") + 1);

            var paths = relativePath.Split('\\');

            var fullItemPath = getPathTo(paths, paths.Length - 1);
            fullItemPath = fullItemPath.Substring(0, fullItemPath.LastIndexOf('.'));

            var item = getDB().GetItem(fullItemPath);
            if (item != null)
            {
                string content = File.ReadAllText(fullPath);
                string itemContent = item.Fields["Content"].Value;

                if (content != itemContent)
                {
                    using (new SecurityDisabler())
                    {
                        item.Editing.BeginEdit();
                        using (new EditContext(item))
                        {
                            item.Fields["Content"].Value = content;
                        }
                        item.Editing.EndEdit();
                    }
                }

            }
            else
            {
                Item parent = getDB().GetItem(@"/sitecore/content/");

                //ensure folders
                for (int index = 0; index < paths.Length - 1; index++)
                {
                    var folderItem = getDB().GetItem(getPathTo(paths, index));
                    if (folderItem == null)
                    {
                        folderItem = AddFolder(parent, paths[index], parent.TemplateID.ToString());
                    }

                    parent = folderItem;
                }

                var fileName = paths[paths.Length - 1];
                var extension = fileName.Split('.')[1];
                var template = "";
                switch (extension.ToLower())
                {
                    case "css":
                        template = @"{26D57461-25FE-407A-BF86-31DBDB513707}";
                        break;
                    case "less":
                        template = @"{2F3DFBFD-2586-4A6F-9D37-0F46E6D393B8}";
                        break;
                    case "scss":
                        template = @"{629C48AC-74B3-4E6B-BBC5-B52604549151}";
                        break;
                    case "js":
                        template = "{722EC325-CC44-4687-ADBD-4EA415502F88}";
                        break;
                }

                if (string.IsNullOrEmpty(template))
                {
                    return;
                }

                //create item
                string content = File.ReadAllText(fullPath);
                    
                using (new SecurityDisabler())
                {                    
                    item = parent.Add(fileName.Split('.')[0], new TemplateID(new ID(template)));
                    item.Editing.BeginEdit();
                    using (new EditContext(item))
                    {
                        item.Fields["Content"].Value = content;
                    }
                    item.Editing.EndEdit();
                }
            }

            if (item != null && autoPublish)
            {
                PublishItem(item);
            }
        }

        private void PublishItem(Item item)
        {
            Sitecore.Publishing.PublishOptions publishOptions =
                new Sitecore.Publishing.PublishOptions(item.Database,
                                                       Database.GetDatabase(publishDB),
                                                       Sitecore.Publishing.PublishMode.SingleItem,
                                                       item.Language,
                                                       System.DateTime.Now);  // Create a publisher with the publishoptions

            Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

            // Choose where to publish from
            publisher.Options.RootItem = item;

            // Publish children as well?
            publisher.Options.Deep = true;

            // Do the publish!
            publisher.Publish();
        }

        protected override void Deleted(string filePath)
        {
            return;
        }

        protected override void Renamed(string filePath, string oldFilePath)
        {
            return;
        }

        
    }
}
