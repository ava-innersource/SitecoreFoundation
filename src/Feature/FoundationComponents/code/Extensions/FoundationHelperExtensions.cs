using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Collections;
using Sitecore.Data.Items;

namespace SF.Feature.FoundationComponents
{
  public static class FoundationHelperExtensions
  {
    public static List<Item> GetNavigableChildItems(this ChildList childList)
    {
      return childList.Where(a => a.Name.ToLower() != "content").ToList();
    }
  }
}