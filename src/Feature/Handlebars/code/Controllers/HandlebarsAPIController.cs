using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SF.Feature.Handlebars
{
    public class HandlebarsAPIController : ServicesApiController
    {
        //Get /sitecore/api/avn/additem
        [HttpGet]
        public string AddItem()
        {
            throwError(HttpStatusCode.MethodNotAllowed, "Get Not Allowed", "Post is Supported");

            return "";
        }

        //Post: /sitecore/api/sf/additem
        [HttpPost]
        public string AddItem(AddItemDetails details)
        {
            
                if (string.IsNullOrEmpty(details.name) ||
                    string.IsNullOrEmpty(details.templateId) ||
                    string.IsNullOrEmpty(details.parentId)
                    )
                {
                    throwError(HttpStatusCode.BadRequest, "Missing Name, TemplateId or ParentId", "Missing Parameters");
                }

                var templateID = new Sitecore.Data.ID(details.templateId);
                var parentID = new Sitecore.Data.ID(details.parentId);

                var db = Sitecore.Context.Database;
                var parent = db.GetItem(parentID);

                if (parent == null)
                {
                    throwError(HttpStatusCode.BadRequest, "ParentId is invalid", "Could not find Parent");
                }

                var template = db.GetItem(templateID);

                if (template == null)
                {
                    throwError(HttpStatusCode.BadRequest, "TemplateId is invalid", "Could not find template");
                }

            try
            {

                var childItem = parent.Add(details.name, new Sitecore.Data.TemplateID(templateID));

                //change sort order to be first
                if (childItem.Access.CanWrite() && !childItem.Appearance.ReadOnly)
                {
                    childItem.Editing.BeginEdit();
                    childItem[FieldIDs.Sortorder] = "1";
                    childItem.Editing.EndEdit();
                }

                int counter = 1;
                foreach (Item child in parent.Children)
                {
                    int sortorder = counter * 100;
                    SetSortorder(child, sortorder);
                    counter++;
                }

                return "OK";
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error in Add Item", ex, this);
                throw new HttpResponseException(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private static void SetSortorder(Item item, int sortorder)
        {
            if (item.Access.CanWrite() && !item.Appearance.ReadOnly)
            {
                item.Editing.BeginEdit();
                item[FieldIDs.Sortorder] = sortorder.ToString();
                item.Editing.EndEdit();
            }
        }

        private void throwError(HttpStatusCode status, string content, string reasonPhrase)
        {
            var resp = new HttpResponseMessage(status)
            {
                Content = new StringContent(content),
                ReasonPhrase = reasonPhrase
            };
            throw new HttpResponseException(resp);
        }
    }
}
