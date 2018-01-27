using SF.Feature.Handlebars.Models;
using Sitecore.ContentSearch.Linq;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Repositories
{
    public class HandlebarCollectionContainerRepository : ModelRepository, IHandlebarCollectionContainerRepository
    {
        public override IRenderingModelBase GetModel()
        {
            var model = new HandlebarCollectionContainerModel();
            FillBaseProperties(model);

            model.EnablePagination = ((Sitecore.Data.Fields.CheckboxField)model.Item.Fields["Enable Pagination"]).Checked;

            int itemsPerPage = 10;
            int.TryParse(model.Item.Fields["Items Per Page"].Value, out itemsPerPage);

            model.ItemsPerPage = itemsPerPage;


            model.QueryStringParam = model.Item.Fields["Querystring Parameter"].Value;

            int currentPage = 1;
            int.TryParse(HttpContext.Current.Request.QueryString[model.QueryStringParam], out currentPage);
            model.CurrentPage = currentPage < 1 ? 1 : currentPage;

            model.CurrentUrl = HttpContext.Current.Request.Url.PathAndQuery;
            model.CurrentUrl = model.CurrentUrl.Replace(model.QueryStringParam + "=" + model.CurrentPage, "");
            if (model.CurrentUrl.IndexOf("?") > -1)
            {
                if (!model.CurrentUrl.EndsWith("?") && !model.CurrentUrl.EndsWith("&"))
                {
                    model.CurrentUrl += "&";
                }
            }
            else
            {
                model.CurrentUrl += "?";
            }

            return model;
        }
    }
}