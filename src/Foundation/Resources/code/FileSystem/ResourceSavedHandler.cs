using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.IO;
using SF.Foundation.Configuration;

namespace SF.Foundation.Resources
{
    public class ResourceSavedHandler
    {
        private const string CSS_Template = @"{26D57461-25FE-407A-BF86-31DBDB513707}";
        private const string Less_Template = @"{2F3DFBFD-2586-4A6F-9D37-0F46E6D393B8}";
        private const string Sass_Template = @"{629C48AC-74B3-4E6B-BBC5-B52604549151}";
        private const string JS_Template = @"{722EC325-CC44-4687-ADBD-4EA415502F88}";
        
        public void OnItemSaved(object sender, EventArgs args)
        {
            Item savedItem = Event.ExtractParameter(args, 0) as Item;

            if (savedItem.TemplateID == new Sitecore.Data.ID(CSS_Template) ||
                savedItem.TemplateID == new Sitecore.Data.ID(Less_Template) ||
                savedItem.TemplateID == new Sitecore.Data.ID(Sass_Template) ||
                savedItem.TemplateID == new Sitecore.Data.ID(JS_Template)
                )
            {
                var relativePath = savedItem.Paths.Path.Replace("/sitecore/content/", "");
                if (relativePath.ToLower().StartsWith("global"))
                {
                    var webRoot = System.Web.Hosting.HostingEnvironment.MapPath("/");
                    var neededPath = webRoot + "/" + relativePath;

                    if (savedItem.TemplateID == new Sitecore.Data.ID(CSS_Template))
                    {
                        neededPath += ".css";
                    }
                    if (savedItem.TemplateID == new Sitecore.Data.ID(Less_Template))
                    {
                        neededPath += ".less";
                    }
                    if (savedItem.TemplateID == new Sitecore.Data.ID(Sass_Template))
                    {
                        neededPath += ".scss";
                    }
                    if (savedItem.TemplateID == new Sitecore.Data.ID(JS_Template))
                    {
                        neededPath += ".js";
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(neededPath));

                    if (savedItem.HasField("Content"))
                    {
                        var contents = savedItem.Fields["Content"].Value;
                        bool writeContents = true;
                        if (File.Exists(neededPath))
                        {
                            var existingContents = File.ReadAllText(neededPath);
                            if (existingContents == contents)
                            {
                                writeContents = false;
                            }
                        }

                        if (writeContents)
                        {
                            File.WriteAllText(neededPath, contents);
                        }
                    }
                }

            }
        }
    }
}
