using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Resources.Media;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace SF.Foundation.Resources.FileSystem
{
    public class TemplateWatcher : FileWatcher
    {
        private string masterDB;
        private string publishDB;
        private bool autoPublish;

        private string MediaFolderTemplate = "{FE5DD826-48C6-436D-B87A-7C4210C7413B}";


        public TemplateWatcher() : base("watchers/templates")
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
            var relativePath = fullPath.Substring(fullPath.ToLower().IndexOf("\\templates\\") + "\\templates\\".Length);

            var paths = relativePath.Split('\\');

            var fileName = paths[paths.Length - 1];
            var fileNameWithoutExtension = fileName.Substring(0, fileName.LastIndexOf('.'));

            var fullItemPath = getPathTo(paths, paths.Length - 1);
            fullItemPath = fullItemPath.Substring(0, fullItemPath.LastIndexOf('.'));

            using (new SecurityDisabler())
            {

                var item = getDB().GetItem(fullItemPath);
                if (item != null)
                {
                    string content = File.ReadAllText(fullPath);
                    item.Editing.BeginEdit();
                    using (new EditContext(item))
                    {
                        item.Fields["Template"].Value = content;
                    }
                    item.Editing.EndEdit();

                }
                else
                {
                    //Do nothing, don't want to add variants on the fly
                }
                
                if (item != null && autoPublish)
                {
                    PublishItem(item);
                }

            }
        }

        private Item EnsureFolders(string[] paths)
        {
            Item parent = getDB().GetItem(@"/sitecore/media library");

            //ensure folders
            for (int index = 0; index < paths.Length - 1; index++)
            {
                var folderItem = getDB().GetItem(getPathTo(paths, index));
                if (folderItem == null)
                {
                    folderItem = AddFolder(parent, paths[index], MediaFolderTemplate);
                }

                parent = folderItem;
            }

            return parent;
        }

        private void SaveMediaItem(string fullPath, string fileName, string fileNameWithoutExtension, string fullItemPath)
        {
            using (var stream = File.OpenRead(fullPath))
            {
                if (stream != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);

                        var mediaCreator = new MediaCreator();
                        var options = new MediaCreatorOptions
                        {
                            Versioned = false,
                            IncludeExtensionInItemName = false,
                            Database = getDB(),
                            Destination = fullItemPath,
                            OverwriteExisting = true,
                            AlternateText = fileNameWithoutExtension

                        };

                        using (new SecurityDisabler())
                            mediaCreator.CreateFromStream(memoryStream, fileName, options);
                    }
                }
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