using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.DXF.Feature.SitecoreProvider
{
    public static class ItemModelHelpers
    {
        public static ItemModel ConvertToItemModel(object source)
        {
            var model = source as ItemModel;

            //See if it's a regular string dictionary
            if (model == null)
            {
                var dict = source as Dictionary<string, string>;
                if (dict != null)
                {
                    model = new ItemModel();
                    foreach (var key in dict.Keys)
                    {
                        model.Add(key, dict[key]);
                    }
                }
            }

            //Item model is a FieldDictionary which is a Dictionary
            //so source is an array for file one, may need code for other source types.
            if (model == null)
            {
                if (source.GetType().IsArray)
                {
                    model = new ItemModel();
                    int indexer = 1;
                    foreach (object item in source as System.Collections.IEnumerable)
                    {
                        model.Add(indexer.ToString(), item);
                        indexer++;
                    }
                }
            }

            return model;
        }
    }
}