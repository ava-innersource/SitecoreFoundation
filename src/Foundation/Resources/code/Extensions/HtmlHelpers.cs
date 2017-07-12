using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public static class HtmlHelpers
    {
        public static IHtmlString GetItemNameAndPageEditorStatus(this HtmlHelper helper)
        {
            Sitecore.Data.Items.Item currentItem = Sitecore.Context.Item;

            string itemName = null;

            if (Sitecore.Context.Site != null)
            {
                var msc = Sitecore.Context.Site.GetSiteSettings<SiteResources>();

                if (msc != null && msc.InsertItemNameAsBodyCSSClass)
                {
                    if (currentItem != null && !string.IsNullOrEmpty(currentItem.Name))
                    {
                        // get page level override if any
                        PageResources pr = new PageResources(currentItem);

                        if (pr != null && !string.IsNullOrWhiteSpace(pr.BodyCSSClassNameOverride))
                        {
                            itemName = pr.BodyCSSClassNameOverride.Trim();
                        }
                        else
                        {
                            itemName = currentItem.Name;
                        }

                        bool isPageEditorMode = Sitecore.Context.PageMode.IsExperienceEditor;

                        return new MvcHtmlString(string.Format("{0}{1}", itemName, isPageEditorMode ? " page-editor" : string.Empty));
                    }
                }
            }

            // MVC 4 will not render a corresponding attribute at all if the return value is null
            return null;
        }
    }
}