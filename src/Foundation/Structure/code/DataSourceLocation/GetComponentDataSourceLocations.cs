using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Pipelines.GetRenderingDatasource;
using Sitecore.SecurityModel;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Structure.DataSourceLocation
{
    public class GetComponentDataSourceLocations
    {
        protected string PageContentFolderTemplate = @"{6C0B22DF-5248-48E8-8A59-85669E61DB94}";
        protected string SiteContentFolderTemplate = @"{30E27B19-38CD-48B9-B222-25274B3DF1B6}";
        protected string SiteCollectionContentFolderTemplate = @"{EE3C0087-50B0-481E-9EC6-DD746E6A318F}";
        protected string GlobalContentFolderTemplate = @"{F650187B-11A3-4353-AFC6-7B3C962CDC04}";
        

        protected string PageContentSubFolderTemplate = @"{52085DDE-D6B5-41AD-AB1A-8D3DC19480A0}";
        protected string SiteContentSubFolderTemplate = @"{57AB960E-72EA-4DDB-BE73-9C994BF1BC24}";
        protected string SiteCollectionContentSubFolderTemplate = @"{A53E7D14-49E1-4271-AF55-47F65AEEC5CA}";
        protected string GlobalContentSubFolderTemplate = @"{A050492F-BDBA-4F31-8FC7-48AAD1EDA0A5}";

        protected string SiteFolderTemplate = @"{B2DDE027-E744-4791-84EC-D788188E466F}";
        protected string SiteCollectionFolderTemplate = @"{538019D0-A1C1-4689-B328-00B503B000A7}";

        protected string KeyPath = "*/";

        public void Process(GetRenderingDatasourceArgs args)
        {
            
            foreach (var location in
                new ListString(args.RenderingItem["Datasource Location"]))
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    break;
                }

                if (!location.StartsWith(KeyPath))
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(args.ContextItemPath))
                {
                    break;
                }

                //Ok we're valid, let's start.
                Item contextItem = args.ContentDatabase.Items[args.ContextItemPath];
                string targetFolderName = location.Substring(KeyPath.Length);

                //Let's not create folders under standard values.
                if (!args.ContextItemPath.StartsWith("Templates"))
                {
                    //Step 1: Look for Local Content folder.
                    string pageContentFolderPath = string.Format("{0}/Content", args.ContextItemPath);
                    Item pageContentFolder = args.ContentDatabase.GetItem(pageContentFolderPath);
                    if (pageContentFolder == null)
                    {
                        pageContentFolder = AddFolder(contextItem, "Content", PageContentFolderTemplate);
                    }

                    //Step 2: Look for Component Folder under it.
                    string componentFolderPath = string.Format("{0}/Content/{1}", args.ContextItemPath, targetFolderName);
                    Item componentFolder = args.ContentDatabase.GetItem(componentFolderPath);
                    if (componentFolder == null)
                    {
                        componentFolder = AddFolder(pageContentFolder, targetFolderName, PageContentSubFolderTemplate);
                    }

                    args.DatasourceRoots.Add(componentFolder);

                    //Step 3: Query Ancestors for Shared Site Folder
                    string query = string.Format(@"ancestor-or-self::*[@@templateid='{0}']/Shared Content", SiteFolderTemplate);
                    Item siteSharedContent = contextItem.Axes.SelectSingleItem(query);
                    if (siteSharedContent != null)
                    {
                        query = string.Format(@"*[@@name='{0}']", targetFolderName);
                        Item siteComponentFolder = siteSharedContent.Axes.SelectSingleItem(query);
                        if (siteComponentFolder == null)
                        {
                            siteComponentFolder = AddFolder(siteSharedContent, targetFolderName, SiteContentSubFolderTemplate);
                        }
                        args.DatasourceRoots.Add(siteComponentFolder);
                    }

                    //Step 4: Get Site Collection Shared Content
                    query = string.Format(@"ancestor-or-self::*[@@templateid='{0}']/Shared Content", SiteCollectionFolderTemplate);
                    Item siteCollectionSharedContent = contextItem.Axes.SelectSingleItem(query);
                    if (siteCollectionSharedContent != null)
                    {
                        query = string.Format(@"*[@@name='{0}']", targetFolderName);
                        Item siteComponentFolder = siteCollectionSharedContent.Axes.SelectSingleItem(query);
                        if (siteComponentFolder == null)
                        {
                            siteComponentFolder = AddFolder(siteCollectionSharedContent, targetFolderName, SiteCollectionContentSubFolderTemplate);
                        }
                        args.DatasourceRoots.Add(siteComponentFolder);
                    }
                }

                //Step 5: Get Global Location
                string globalSharedComponentsPath = @"/Sitecore/Content/Global/Shared Content";
                Item globalSharedComponentsFolder = args.ContentDatabase.GetItem(globalSharedComponentsPath);
                if (globalSharedComponentsFolder != null)
                {
                    string globalComponentPath = string.Format( @"/Sitecore/Content/Global/Shared Content/{0}", targetFolderName);
                    Item globalComponentFolder = args.ContentDatabase.GetItem(globalComponentPath);
                    if (globalComponentFolder == null)
                    {
                        globalComponentFolder = AddFolder(globalSharedComponentsFolder, targetFolderName, GlobalContentSubFolderTemplate);
                    }
                    args.DatasourceRoots.Add(globalComponentFolder);
                }
            }

        }

        private Item AddFolder(Item parent, string folderName, string templateID)
        {
            using (new SecurityDisabler())
            {
                return parent.Add(folderName, new TemplateID(new ID(templateID)));
            }
        }
    }
}
