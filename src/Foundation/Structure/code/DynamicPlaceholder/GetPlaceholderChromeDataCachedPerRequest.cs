using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using Sitecore.Web.UI.PageModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Structure
{

    public class GetPlaceholderChromeDataCachedPerRequest : GetChromeDataProcessor
{
    ///The chrome type.
    public const string ChromeType = "placeholder";
    ///The key of the placeholderkey in CustomData dictionary.
    public const string PlaceholderKey = "placeHolderKey";
    ///Path to the root item default buttons.
    private const string DefaultButtonsRoot = "/sitecore/content/Applications/WebEdit/Default Placeholder Buttons";
 
    ///The process.
    ///The pipeline args.
    public override void Process(GetChromeDataArgs args)
    {
        Assert.ArgumentNotNull(args, "args");
        Assert.IsNotNull(args.ChromeData, "Chrome Data");
        if (!ChromeType.Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase)) return;
 
        var placeholderPath = args.CustomData[PlaceholderKey] as string;
        Assert.ArgumentNotNull(placeholderPath, string.Format("CustomData[\"{0}\"]", PlaceholderKey));
 
        var placeholderKey = StringUtil.GetLastPart(placeholderPath, '/', placeholderPath);
 
        args.ChromeData.DisplayName = placeholderKey;
        AddButtonsToChromeData(GetButtons(DefaultButtonsRoot), args);
 
        Item placeholderItem = null;
        var hasPlaceholderSettings = false;
        if (args.Item != null)
        {
            //this is the part modified from base implementation
            var layout = GetCachedLayout(args.Item);
 
            var placeholderRenderingsArgs = new GetPlaceholderRenderingsArgs(placeholderPath, layout, args.Item.Database)
            {
                OmitNonEditableRenderings = true
            };
            CorePipeline.Run("getPlaceholderRenderings", placeholderRenderingsArgs);
 
            hasPlaceholderSettings = placeholderRenderingsArgs.HasPlaceholderSettings;
            var stringList = new List<string>();
            if (placeholderRenderingsArgs.PlaceholderRenderings != null)
            {
                stringList = placeholderRenderingsArgs.PlaceholderRenderings.Select(i => i.ID.ToShortID().ToString()).ToList() ?? new List<string>();
            }
            args.ChromeData.Custom.Add("allowedRenderings", stringList);
            placeholderItem = Client.Page.GetPlaceholderItem(placeholderPath, args.Item.Database, layout);
            if (placeholderItem != null)
                args.ChromeData.DisplayName = HttpUtility.HtmlEncode(placeholderItem.DisplayName);
            if (placeholderItem != null && !string.IsNullOrEmpty(placeholderItem.Appearance.ShortDescription))
                args.ChromeData.ExpandedDisplayName = HttpUtility.HtmlEncode(placeholderItem.Appearance.ShortDescription);
        }
        else
            args.ChromeData.Custom.Add("allowedRenderings", new List<string>());
        var isEditable = (placeholderItem == null || placeholderItem["Editable"] == "1") && (Settings.WebEdit.PlaceholdersEditableWithoutSettings || hasPlaceholderSettings);
        args.ChromeData.Custom.Add("editable", isEditable.ToString().ToLowerInvariant());
    }
    
    ///
    /// Instead of calling ChromeContext.GetLayout(layoutItem) every time, we store it in the request so we only need to do it once
    ///
    ///
    /// 
    protected virtual string GetCachedLayout(Item layoutItem)
    {
        var layoutCacheKey = string.Format("edit-layout-item-{0}", layoutItem.ID.Guid.ToString("N"));
        var layout = Context.Items[layoutCacheKey] as string ?? ChromeContext.GetLayout(layoutItem);
        Context.Item[layoutCacheKey] = layout;
 
        return layout;
    }
}
}
