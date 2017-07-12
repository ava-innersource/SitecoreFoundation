using Sitecore.Data.Items;
using Sitecore.Pipelines.GetRenderingDatasource;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Structure.DataSourceLocation
{

    public class GetQueryableDataSourceLocations
    {
        public void Process(GetRenderingDatasourceArgs args)
        {
            foreach (var location in
                new ListString(args.RenderingItem["Datasource Location"]))
            {
                if (location.StartsWith("query:"))
                {
                    Item contextItem = args.ContentDatabase.Items[args.ContextItemPath];
                    if (contextItem != null)
                    {
                        string query = location.Substring("query:".Length);
                        Item queryItem = contextItem.Axes.SelectSingleItem(query);
                        if (queryItem != null)
                        {
                            args.DatasourceRoots.Add(queryItem);
                        }
                    }
                }
            }
        }
    }
}
