using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Extensions
{
    public static class ItemExtensions
    {
        public static IEnumerable<string> GetBranchChildrenDetails(this Item item, string prefix = "")
        {
            foreach(Item child in item.Children)
            {
                yield return string.Format("{0} {1} ({2})", prefix, child.Name, child.TemplateName);

                foreach (var result in child.GetBranchChildrenDetails(prefix + child.Name + ">"))
                {
                    yield return result;
                }
            }
        }
    }
}