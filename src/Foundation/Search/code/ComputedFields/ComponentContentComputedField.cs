using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SF.Foundation.Search
{
  public class ComponentContentComputedField : IComputedIndexField
  {

    public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
    {
      var item = (Item)(indexable as SitecoreIndexableItem);
      if (item == null) return null;

      // optimization to reduce indexing time
      // by skipping this logic for items in the Core database
      if (System.String.Compare(
        item.Database.Name,
        "core",
        System.StringComparison.OrdinalIgnoreCase) == 0)
      {
        return null;
      }


      if (item.Paths.IsMediaItem)
      {
        //skip media root and folder items
        if (item.TemplateID != Sitecore.TemplateIDs.MediaFolder
          && item.ID != Sitecore.ItemIDs.MediaLibraryRoot)
          return null;
      }

      //From Sitecore 8.0 onwards we need to look in Final Renderings for latest content versions
      LayoutField layoutField = item.Fields["__Final Renderings"];
      RenderingReference[] renderings = layoutField.GetReferences(GetDefaultDevice(item.Database));

      if (renderings == null) return null;

      StringBuilder sb = new StringBuilder();

      foreach (var renderingReference in renderings)
      {
        var datasourceId = renderingReference.Settings.DataSource;

        //Check if datasource has been set or we get null reference errors
        if (!string.IsNullOrWhiteSpace(datasourceId))
        {
          var referencedItem = item.Database.GetItem(datasourceId, item.Language);
          foreach (Field field in referencedItem.Fields)
          {
            if (!field.IsStandardTemplateField())
            {
              if (field.Type == "Single-Line Text" || field.Type == "Rich Text")
              {
                sb.Append(" ").Append(StripTags(field.Value));
              }
            }
          }
        }
      }

      return sb.ToString().Trim();
    }

    public string StripTags(string input)
    {
      return WebUtility.HtmlDecode(Regex.Replace(input, "<[^>]*(>|$)", string.Empty));
    }
    public DeviceItem GetDefaultDevice(Database db)
    {
      return db.Resources.Devices.GetAll().First(d => d.Name.ToLower() == "default");
    }

    public string FieldName
    {
      get;
      set;
    }

    public string ReturnType
    {
      get;
      set;
    }
  }
}
