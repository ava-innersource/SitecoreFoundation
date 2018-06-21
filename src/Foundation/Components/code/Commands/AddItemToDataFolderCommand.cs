using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using Sitecore.XA.Foundation.Multisite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Sitecore.XA.Foundation.SitecoreExtensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace SF.Foundation.Components.Commands
{
    [Serializable]
    public class AddItemToDataFolderCommand : WebEditCommand
    {
        /// <summary>
        /// Main execution point of the WebEditCommand
        /// </summary>
        public override void Execute(CommandContext context)
        {
            NameValueCollection parameters = new NameValueCollection();
           
            if (context.Items.Length != 1)
                return;

            parameters["ItemToUpdate"] = context.Items[0].ID.Guid.ToString();

            // Get Language
            var itemUri = ItemUri.ParseQueryString();
            var language = context.Parameters["language"];

            if (String.IsNullOrEmpty(language) && itemUri != null)
            {
                language = itemUri.Language.ToString();
            }

            parameters["language"] = language;

            // Store the custom parameters 
            parameters["templateId"] = context.Parameters["templateId"] ?? String.Empty;
            parameters["dataFolderName"] = context.Parameters["dataFolderName"] ?? String.Empty;
            parameters["fieldId"] = context.Parameters["fieldId"] ?? String.Empty;
            parameters["locationId"] = context.Parameters["locationId"] ?? String.Empty;

            Context.ClientPage.Start((object)this, "Run", parameters);
        }

        /// <summary>
        /// Handles the Postback of the Sheer Dialogs
        /// </summary>
        protected static void Run(ClientPipelineArgs args)
        {
            var language = Language.Parse(args.Parameters["language"]);
            
            // Retrival of custom parameters
            var templateId = AddItemToDataFolderCommand.GetSitecoreId(args.Parameters, "templateId");
            var fieldId = AddItemToDataFolderCommand.GetSitecoreId(args.Parameters, "fieldId");
            var itemId = AddItemToDataFolderCommand.GetSitecoreId(args.Parameters, "ItemToUpdate");
            var locationId = AddItemToDataFolderCommand.GetSitecoreId(args.Parameters, "locationId", itemId.ToString());


            var item = Context.ContentDatabase.GetItem(itemId);

            //Can Use Location ID and Note Data Folder Name if want to have explicit global Location.
            var dataFolderName = args.Parameters["dataFolderName"];
            if (!string.IsNullOrEmpty(dataFolderName))
            {
                var settingsItem = ServiceLocator.ServiceProvider.GetService<IMultisiteContext>().GetDataItem(item);
                var locationFolder = settingsItem.Children.Where(x => x.Name == dataFolderName).FirstOrDefault();
                if (locationFolder != null)
                {
                    locationId = locationFolder.ID;
                }
                else
                {
                   var queryResults = settingsItem.Axes.GetDescendant(dataFolderName);
                    if (queryResults != null)
                    {
                        locationId = queryResults.ID;
                    }
                }
                
            }

            var locationItem = Context.ContentDatabase.GetItem(locationId);

            var branch = Context.ContentDatabase.Branches[templateId.ToString(), language];
            var template = Context.ContentDatabase.Templates[templateId, language];

            if (args.IsPostBack)
            {
                if (!args.HasResult)
                {
                    return;
                }

                Item childItem = null ;

                if (branch != null)
                {
                    childItem = Context.Workflow.AddItem(args.Result, branch, locationItem);

                }
                else
                {
                    childItem = Context.Workflow.AddItem(args.Result, template, locationItem);
                }

                // Add to a MultilistField
                if (!fieldId.IsNull)
                {
                    AddItemToDataFolderCommand.AddToMultiField(item, fieldId, childItem.ID);
                }

                SheerResponse.Eval(
                    "scForm.browser.getParentWindow(scForm.browser.getFrameElement(window).ownerDocument).location.reload(true)");
            }
            else if (!locationItem.Access.CanCreate())
            {
                Context.ClientPage.ClientResponse.Alert("You do not have permission to create an item here.");
            }
            else
            {
                SheerResponse.Input(String.Format("Enter a name for the new {0}:", branch.DisplayName),
                    branch.DisplayName,
                    Settings.ItemNameValidation,
                    Translate.Text("'$Input' is not a valid name."), Settings.MaxItemNameLength);

                args.WaitForPostBack();
            }
        }

        /// <summary>
        /// Gets a Sitecore Id from a NameValueCollection
        /// </summary>
        /// <param name="collection">Collection of Parameters</param>
        /// <param name="parameter">Parameter Key</param>
        /// <param name="defaultValue">Default value if no parameter under that key found. Default
        /// value is also returned if the value cannot be parsed into a </param>
        /// <returns>Sitecore Id</returns>
        private static ID GetSitecoreId(NameValueCollection collection, string parameter, string defaultValue = null)
        {
            if (collection == null || !collection.HasKeys() || String.IsNullOrEmpty(parameter))
            {
                return null;
            }

            var value = collection[parameter] ?? String.Empty;

            if (String.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            ID output;

            ID.TryParse(value, out output);

            return output;
        }

        /// <summary>
        /// Adds a Sitecore Id to a Multilist Field
        /// <para>All parameters are required.</para>
        /// </summary>
        /// <param name="item">Context Item</param>
        /// <param name="fieldId">Multilist Field</param>
        /// <param name="value">Value to add to the multilist field</param>
        private static void AddToMultiField(Item item, ID fieldId, ID value)
        {
            if (item == null || fieldId.IsNull || value.IsNull)
            {
                return;
            }

            MultilistField multiListField = item.Fields[fieldId];

            if (multiListField == null)
            {
                return;
            }

            using (new SecurityDisabler())
            {
                using (new EditContext(item))
                {
                    multiListField.Add(value.ToString());
                }
            }
        }
    }
}