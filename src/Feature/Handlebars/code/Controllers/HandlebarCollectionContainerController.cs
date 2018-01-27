using SF.Feature.Handlebars.Models;
using SF.Feature.Handlebars.Repositories;
using Sitecore.ContentSearch.Linq;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.Handlebars.Controllers
{
    public class HandlebarCollectionContainerController : StandardController
    {
        protected readonly IHandlebarCollectionContainerRepository HandlebarCollectionContainerRepository;

        public HandlebarCollectionContainerController(IHandlebarCollectionContainerRepository repository)
        {
            this.HandlebarCollectionContainerRepository = repository;
        }

        protected override object GetModel()
        {
            return HandlebarCollectionContainerRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as HandlebarCollectionContainerModel;

            var items = new List<Sitecore.Data.Items.Item>();
            var container = model.Item;

            string rootFolderId = container.Fields["Root Folder"].Value;
            if (!string.IsNullOrEmpty(rootFolderId))
            {
                var rootFolder = Sitecore.Context.Database.GetItem(rootFolderId);
                if (Sitecore.Buckets.Managers.BucketManager.IsBucket(rootFolder))
                {
                    var bucketPath = rootFolder.Paths.Path;
                    //we got a bucket, use search API to get all children
                    var tempItem = (Sitecore.ContentSearch.SitecoreIndexableItem)rootFolder;
                    var index = Sitecore.ContentSearch.ContentSearchManager.GetIndex(tempItem);
                    using (var context = index.CreateSearchContext())
                    {
                        var predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.True<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();
                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item.Path.StartsWith(bucketPath));

                        var query = context.GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Where(predicate);
                        var results = query.GetResults();
                        foreach (var result in results.Hits)
                        {
                            var item = result.Document.GetItem();
                            items.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var child in rootFolder.GetChildren().Cast<Sitecore.Data.Items.Item>().ToList())
                    {
                        items.Add(child);
                    }
                }
            }

            var additionalItems = (Sitecore.Data.Fields.MultilistField)container.Fields["Additional Items"];
            foreach (var item in additionalItems.GetItems())
            {
                items.Add(item);
            }

            model.NumItems = items.Count;
            model.NumPages = Convert.ToInt32(Math.Ceiling((double)((double)items.Count / (double)model.ItemsPerPage)));

            if (model.EnablePagination)
            {
                int skipItems = (model.CurrentPage - 1) * model.ItemsPerPage;
                items = items.Skip(skipItems).Take(model.ItemsPerPage).ToList();
            }

           
            HandlebarManager.SetupContainer(items);

            return View(model);
        }
    }
}