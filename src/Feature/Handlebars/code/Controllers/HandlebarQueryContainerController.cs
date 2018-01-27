using SF.Feature.Handlebars.Models;
using SF.Feature.Handlebars.Repositories;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SF.Feature.Handlebars.Controllers
{
    public class HandlebarQueryContainerController : StandardController
    {
        protected readonly IHandlebarQueryContainerRepository HandlebarQueryContainerRepository;

        public HandlebarQueryContainerController(IHandlebarQueryContainerRepository repository)
        {
            this.HandlebarQueryContainerRepository = repository;
        }

        protected override object GetModel()
        {
            return HandlebarQueryContainerRepository.GetModel();
        }

        public override ActionResult Index()
        {
            var model = this.GetModel() as HandlebarQueryContainerModel;

            var items = new List<Sitecore.Data.Items.Item>();
            var container = model.Item;

            string rootFolderId = container.Fields["Search Root"].Value;
            var rootFolder = Sitecore.Context.Database.GetItem(rootFolderId);

            string searchQuery = container.Fields["Free Text Query"].Value;

            var baseTemplateFld = (MultilistField)container.Fields["Base Template"];
            var baseTemplates = baseTemplateFld.GetItems();

            var fieldFiltersFld = (NameValueListField)container.Fields["Field Filters"];
            var fieldFilters = fieldFiltersFld.NameValues;

            var queryParam = container.Fields["Query Parameter"].Value;
            queryParam = string.IsNullOrEmpty(queryParam) ? "q" : queryParam;
            var additionalSearch = Request.QueryString[queryParam];

            var indexNameOverride = container.Fields["Index Name Override"].Value;

            var orderByField = container.Fields["Order By Field"].Value;
            bool orderByAscending = container.Fields["Order By Direction"].Value.ToLower() == "ascending";

            Sitecore.ContentSearch.ISearchIndex index = null;
            if (!string.IsNullOrEmpty(indexNameOverride))
            {
                //Overriden Index Name
                index = Sitecore.ContentSearch.ContentSearchManager.GetIndex(indexNameOverride);
            }
            else
            {
                //Use context of root Item to get index
                if (rootFolder != null)
                {
                    var tempItem = (Sitecore.ContentSearch.SitecoreIndexableItem)rootFolder;
                    index = Sitecore.ContentSearch.ContentSearchManager.GetIndex(tempItem);
                }
            }

            if (index != null)
            {
                using (var context = index.CreateSearchContext())
                {
                    var rootPath = rootFolder.Paths.Path;
                    var predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.True<Sitecore.ContentSearch.SearchTypes.SearchResultItem>();

                    //ensure item falls under root item
                    predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item.Path.StartsWith(rootPath));

                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item.Content.Contains(searchQuery));
                    }

                    if (baseTemplates.Length > 0)
                    {
                        var innerPredicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.False<SearchResultItem>();
                        foreach (var template in baseTemplates)
                        {
                            innerPredicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.Or(innerPredicate, item => item.TemplateId == template.ID);
                        }
                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, innerPredicate);
                    }

                    foreach (string key in fieldFilters.Keys)
                    {
                        string filterValue = HttpUtility.UrlDecode(fieldFilters[key]);

                        if (filterValue.StartsWith("!"))
                        {
                            filterValue = filterValue.Replace("!", "");
                            predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] != filterValue);
                        }
                        else
                        {
                            if (filterValue.StartsWith("$"))
                            {
                                var pageItem = Sitecore.Context.Item;
                                var pageIDFormat1 = pageItem.ID.Guid.ToString();
                                var pageIDFormat2 = pageIDFormat1.Replace("-", "");

                                if (filterValue == "$ID")
                                {
                                    predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] == pageIDFormat1 || item[key] == pageIDFormat2);
                                }
                                else
                                {
                                    filterValue = filterValue.Replace("$", "");
                                    if (pageItem.Fields[filterValue] != null && !string.IsNullOrEmpty(pageItem.Fields[filterValue].Value))
                                    {
                                        filterValue = pageItem.Fields[filterValue].Value;
                                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] == filterValue);
                                    }
                                }
                            }
                            else
                            {
                                if (filterValue.Length > 30 && filterValue.Length < 40)
                                {
                                    Guid possibleGuid = Guid.Empty;
                                    if (Guid.TryParse(filterValue, out possibleGuid))
                                    {
                                        //Sometimes Guid's get stored in Lucene index with and without dashes.
                                        var possibleFormat1 = possibleGuid.ToString();
                                        var possibleFormat2 = possibleFormat1.Replace("-", "");
                                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] == possibleFormat1 || item[key] == possibleFormat2);
                                    }
                                    else
                                    {
                                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] == filterValue);
                                    }
                                }
                                else
                                {
                                    predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item[key] == filterValue);
                                }
                            }
                        }

                    }

                    if (!string.IsNullOrEmpty(additionalSearch))
                    {
                        predicate = Sitecore.ContentSearch.Linq.Utilities.PredicateBuilder.And(predicate, item => item.Content.Contains(additionalSearch));
                    }

                    var query = context.GetQueryable<Sitecore.ContentSearch.SearchTypes.SearchResultItem>().Where(predicate);

                    if (!string.IsNullOrEmpty(orderByField))
                    {
                        if (orderByAscending)
                        {
                            query = query.OrderBy(item => item[orderByField]);
                        }
                        else
                        {
                            query = query.OrderByDescending(item => item[orderByField]);
                        }

                    }

                    var results = query.GetResults();
                    foreach (var result in results.Hits)
                    {
                        var item = result.Document.GetItem();
                        items.Add(item);
                    }
                }
            }


            HandlebarManager.SetupContainer(items);

            return View(model);
        }
    }
}