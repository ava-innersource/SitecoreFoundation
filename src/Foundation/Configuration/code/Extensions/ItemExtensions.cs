using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Configuration
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Uses the Link Manager to return the relative url to the item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetItemUrl(this Item item)
        {
            return LinkManager.GetItemUrl(item);
        }

        /// <summary>
        /// Uses the Link Manager to return the relative url and combines with the current host
        /// to create a fully qualified url.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetFullyQualifiedUrl(this Item item)
        {
            Uri uri = new Uri(System.Web.HttpContext.Current.Request.Url, LinkManager.GetItemUrl(item));
            return uri.ToString();
        }

        /// <summary>
        /// Checks whether the field exists on the Item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool HasField(this Item item, string fieldName)
        {
            return item.Fields[fieldName] != null;
        }
    /// <summary>
    /// Checks whetehr the field exists on the Item by ID
    /// </summary>
    /// <param name="item"></param>
    /// <param name="fieldID"></param>
    /// <returns></returns>
    public static bool HasField(this Item item, ID fieldID)
    {
      return item.Fields[fieldID] != null;
    }

    /// <summary>
    /// If the Item is a media item, uses the media manager to get the media url
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string GetMediaUrl(this Item item)
        {
            MediaItem mediaItem = null;
            try
            {
                mediaItem = (MediaItem)item;
            }
            catch
            {
                return string.Empty;
            }
            return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem));
        }

        /// <summary>
        /// If the Item is a media item, uses the media manager to get the media url
        /// using MediaUrlOptions AlwaysIncludeServerUrl set to true.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetFullyQualifiedMediaUrl(this Item item)
        {
            MediaItem mediaItem = null;
            try
            {
                mediaItem = (MediaItem)item;
            }
            catch
            {
                return string.Empty;
            }
            var options = new Sitecore.Resources.Media.MediaUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            return Sitecore.Resources.Media.MediaManager.GetMediaUrl(mediaItem, options);
        }


        public static bool IsDerived(this Template template, ID templateId)
        {
            return template.ID == templateId || template.GetBaseTemplates().Any(baseTemplate => IsDerived(baseTemplate, templateId));
        }


        public static bool HasLayout(this Item item)
        {
            LayoutItem layoutItem = item.Visualization.GetLayout(Sitecore.Context.Device);
            return layoutItem != null;
        }

        public static bool IsDerived( this Item item, ID templateId)
        {
            return TemplateManager.GetTemplate(item).IsDerived(templateId);
        }

        public static bool IsCurrentItem(this Item item)
        {
            return Sitecore.Context.Item.ID == item.ID;
        }

        public static bool AreChildrenCurrentItem(this Item item)
        {
            bool rtnVal = false;

            foreach (Item child in item.Children)
            {
                if (child.ID == Sitecore.Context.Item.ID)
                {
                    rtnVal = true;
                    return rtnVal;
                }

                if (child.HasChildren)
                {
                    rtnVal = child.AreChildrenCurrentItem();
                }

                if (rtnVal)
                {
                    return rtnVal;
                }
            }

            return rtnVal;
        }



        public static List<Item> GetBreadcrumbTrail(this Item item)
        {
            List<Item> items = new List<Item>();
            var sc = SiteExtensions.GetContextSite();
            
            if (sc != null)
            {
                var startItem = Sitecore.Context.Database.GetItem(sc.StartPath);
                if (startItem != null)
                {
                    Item currentItem = item;
                    items.Add(currentItem);
                    while (!currentItem.ID.Equals(startItem.ID))
                    {
                        currentItem = currentItem.Parent;
                        if (currentItem == null)
                        {
                            return new List<Item>();
                        }

                        items.Insert(0, currentItem);
                    }
                }
            }

            return items;
        }

        public static String LinkUrl(this Sitecore.Data.Fields.LinkField lf)
        {
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return lf.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return lf.Url;
            }
        }

        public static bool IsStandardTemplateField(this Sitecore.Data.Fields.Field field)
        {
            Sitecore.Data.Templates.Template template = Sitecore.Data.Managers.TemplateManager.GetTemplate(
              Sitecore.Configuration.Settings.DefaultBaseTemplate,
              field.Database);
            Sitecore.Diagnostics.Assert.IsNotNull(template, "template");
            return template.ContainsField(field.ID);
        }
    }
}
