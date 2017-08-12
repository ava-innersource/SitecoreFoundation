using HandlebarsDotNet;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SF.Foundation.TraceLogger;
using SF.Foundation.CommonComponents;

namespace SF.Feature.Handlebars
{
    public static class HandlebarManager
    {
        private static int MAX_LEVEL = Sitecore.Configuration.Settings.GetIntSetting("SF.HandlebarsMaxDepth", 3);

        static HandlebarManager()
        {
            //Run Pipeleine to get Helpers and Register them
            var args = new HandlebarHelpersPipelineArgs();
            Sitecore.Pipelines.CorePipeline.Run("handlebarHelpers", args);
            foreach (var helper in args.Helpers)
            {
                if (helper.Helper != null && !HandlebarsDotNet.Handlebars.Configuration.Helpers.ContainsKey(helper.Name) && !HandlebarsDotNet.Handlebars.Configuration.BlockHelpers.ContainsKey(helper.Name))
                {
                    HandlebarsDotNet.Handlebars.RegisterHelper(helper.Name, helper.Helper);
                }
                else
                {
                    if (helper.BlockHelper != null && !HandlebarsDotNet.Handlebars.Configuration.Helpers.ContainsKey(helper.Name) && !HandlebarsDotNet.Handlebars.Configuration.BlockHelpers.ContainsKey(helper.Name))
                    {
                        HandlebarsDotNet.Handlebars.RegisterHelper(helper.Name, helper.BlockHelper);
                    }
                }
            }
        }

        public static void SetupJsonContainer(string url)
        {
            TraceLogger.Current.Write("Call to SetupJsonContainer for url " + url);
            TraceLogger.Current.Write("Fetching data from endpoint");
            var wc = new System.Net.WebClient();
            var jsonText = wc.DownloadString(url);
            TraceLogger.Current.Write("Data Obtained, Deserializing object to dynamic");
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonText);
            TraceLogger.Current.Write("Data Deserialized, Object Ready, Adding to Context");
            if (HttpContext.Current.Items["HandlebarDataSource"] == null)
            {
                HttpContext.Current.Items.Add("HandlebarDataSource", obj);
            }
            else
            {
                HttpContext.Current.Items["HandlebarDataSource"] = obj;
            }
        }

        public static void SetupFacetContainer()
        {
            TraceLogger.Current.Write("Call to SetupFacetContainer");
            TraceLogger.Current.Write("Fetching FacetList from Configuration");
            //Get Facet Type from 
            XmlNodeList nodes = Factory.GetConfigNodes(@"model/entities/contact/facets/*");
            MethodInfo method = typeof(Sitecore.Analytics.Tracking.Contact).GetMethod("GetFacet");

            if (Sitecore.Analytics.Tracker.Current == null)
            {
                Sitecore.Analytics.Tracker.StartTracking();
            }

            var myDynamicItem = new ExpandoObject() as IDictionary<string, Object>;
            myDynamicItem.Add("IsPageEditorEditing", Sitecore.Context.PageMode.IsExperienceEditorEditing ? true : false);

            TraceLogger.Current.Write("Configuration obtained, Enumerating Facet config nodes");

            foreach(XmlNode node in nodes)
            {
                var facetName = Sitecore.Xml.XmlUtil.GetAttribute("name", node);
                var facetTypeName = Sitecore.Xml.XmlUtil.GetAttribute("contract", node);

                TraceLogger.Current.Write("Fetching facet " + facetName + " for contact");
                Type facetType = Type.GetType(facetTypeName);
                MethodInfo generic = method.MakeGenericMethod(facetType);

                object obj = generic.Invoke(Sitecore.Analytics.Tracker.Current.Contact, new object[] { facetName });
                if (obj != null)
                {
                    myDynamicItem.Add(facetName, obj);
                }

            }

            TraceLogger.Current.Write("Facets Ready, Adding to Context");

            if (HttpContext.Current.Items["HandlebarDataSource"] == null)
            {
                HttpContext.Current.Items.Add("HandlebarDataSource", myDynamicItem);
            }
            else
            {
                HttpContext.Current.Items["HandlebarDataSource"] = myDynamicItem;
            }
        }

        public static void SetupContainer(Item item)
        {
            TraceLogger.Current.Write("Call to SetupContainer for Item " + item != null ? item.ID.ToString() : string.Empty );
            TraceLogger.Current.Write("Converting Item to Object for Binding");

            var itemAsObj = getItemAsObject(item, 0,  MAX_LEVEL);

            TraceLogger.Current.Write("Item processed, storing in context.");

            if (HttpContext.Current.Items["HandlebarDataSource"] == null)
            {
                HttpContext.Current.Items.Add("HandlebarDataSource", itemAsObj);
            }
            else
            {
                HttpContext.Current.Items["HandlebarDataSource"] = itemAsObj;
            }
        }

        public static void SetupContainer(List<Item> items)
        {
            TraceLogger.Current.Write("Call to SetupContainer for ItemList");
            TraceLogger.Current.Write("Converting Items to Object");

            var itemAsObj = getItemListAsObject(items);

            TraceLogger.Current.Write("Items processed, storing in context.");


            if (HttpContext.Current.Items["HandlebarDataSource"] == null)
            {
                HttpContext.Current.Items.Add("HandlebarDataSource", itemAsObj);
            }
            else
            {
                HttpContext.Current.Items["HandlebarDataSource"] = itemAsObj;
            }
        }

        public static IHtmlString GetTemplatedContent(this HtmlHelper helper, Item handlebarTemplate)
        {
            TraceLogger.Current.Write("Call to HandlebarManager.GetTempaltedContent");
            TraceLogger.Current.Write("Step 1: Fetching Compiled Template");
            var template = GetCompiledTemplate(handlebarTemplate);
            var templateData = HttpContext.Current.Items["HandlebarDataSource"];
            if (templateData != null)
            {
                string output = string.Empty;
                try
                {
                    TraceLogger.Current.Write("Template Ready, Step 2: Binding to Context Data");
                    output = template(templateData);
                    TraceLogger.Current.Write("Content Bound. Output Ready");
                }
                catch (Exception ex)
                {
                    output = string.Format("Template Error: {0}", ex);
                }
                
                return new MvcHtmlString(output);
            }

            return new MvcHtmlString("<p>No Data Exists to Bind</p>");
        }

        private static object lockObject = new object();

        private static Func<object, string> GetCompiledTemplate(Item handlebarTemplate)
        {
            bool bypassHttpCache = Sitecore.Context.PageMode.IsPreview ||
                    Sitecore.Context.PageMode.IsExperienceEditor ||
                    Sitecore.Context.PageMode.IsSimulatedDevicePreviewing;

            Dictionary<Guid, Func<object, string>> compiledTemplates = null;
            Guid templateID = Guid.Empty;

            bool updateCache = false;

            //avoid competing compilations
            lock (lockObject)
            {
                if (!bypassHttpCache)
                {
                    TraceLogger.Current.Write("Getting Cache");
                    compiledTemplates = HttpRuntime.Cache["CompiledHandlebarTemplates"] as Dictionary<Guid, Func<object, string>>;
                }

                if (compiledTemplates == null)
                {
                    TraceLogger.Current.Write("Cache Empty, Creating new Cache");
                    compiledTemplates = new Dictionary<Guid, Func<object, string>>();
                    updateCache = true;
                }

                templateID = handlebarTemplate.ID.ToGuid();
                if (!compiledTemplates.ContainsKey(templateID))
                {
                    TraceLogger.Current.Write("Template not in Cache, Getting Template");
                    updateCache = true;
                    var templateContent = handlebarTemplate.Fields["Content"].Value;

                    if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
                    {
                        //force replace everything to include html
                        templateContent = templateContent.Replace("{{", "{{{");
                        templateContent = templateContent.Replace("}}", "}}}");
                        //fix html to not have 4 from the first replace
                        templateContent = templateContent.Replace("{{{{", "{{{");
                        templateContent = templateContent.Replace("}}}}", "}}}");
                    }

                    TraceLogger.Current.Write("Obtained Template, Precompiling");
                    var template = HandlebarsDotNet.Handlebars.Compile(templateContent);
                    compiledTemplates.Add(templateID, template);
                    TraceLogger.Current.Write("Template Precompiled");
                }

                if (updateCache && !bypassHttpCache)
                {
                    HttpRuntime.Cache.Insert("CompiledHandlebarTemplates", compiledTemplates);
                    TraceLogger.Current.Write("Cache Updated with Compiled Template");
                }
            }

            return compiledTemplates[templateID];
        }

        private static object getItemListAsObject(List<Item> items)
        {
            var myDynamicItem = new ExpandoObject() as IDictionary<string, Object>;

            myDynamicItem.Add("IsPageEditorEditing", Sitecore.Context.PageMode.IsExperienceEditorEditing ? true : false);

            List<object> children = new List<object>();
            
            foreach (var childItem in items)
            {
                children.Add(getItemAsObject(childItem, 0, MAX_LEVEL));
            }
            myDynamicItem.Add("Items", children);
            return myDynamicItem;
        }

        private static object getItemAsObject(Item item, int currentLevel, int maxLevel)
        {
            TraceLogger.Current.Write("Getting Item as Object Item [" + item != null ? item.ID.ToString() : string.Empty + "] Current Level:[" + currentLevel + "] Max Level:[" + maxLevel + "]");
            return new DynamicItem(item);
        }


    }
}
