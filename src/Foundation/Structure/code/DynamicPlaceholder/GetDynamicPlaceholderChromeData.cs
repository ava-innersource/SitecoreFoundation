using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Web.UI.PageModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SF.Foundation.Structure.DynamicPlaceholder
{
    /// <summary>
    ///     Replaces the Displayname of the Placeholder rendering with the dynamic "parent"
    /// </summary>
    public class GetDynamicPlaceholderChromeData : GetChromeDataProcessor
    {
        //text that ends in a GUID
        private const string DynamicKeyRegex = @"(.+)_[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}";

        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if ("placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                var argument = args.CustomData["placeHolderKey"] as string;

                string placeholderKey = argument;
                var regex = new Regex(DynamicKeyRegex);
                Match match = regex.Match(placeholderKey);
                if (match.Success && match.Groups.Count > 0)
                {
                    // Is a Dynamic Placeholder
                    placeholderKey = match.Groups[1].Value;
                }
                else
                {
                    return;
                }

                // Handles replacing the displayname of the placeholder area to the master reference
                if (args.Item != null)
                {
                    string layout = ChromeContext.GetLayout(args.Item);
                    Item item = Client.Page.GetPlaceholderItem(placeholderKey, args.Item.Database, layout);
                    if (item != null)
                    {
                        args.ChromeData.DisplayName = item.DisplayName;
                    }

                    if ((item != null) && !string.IsNullOrEmpty(item.Appearance.ShortDescription))
                    {
                        args.ChromeData.ExpandedDisplayName = item.Appearance.ShortDescription;
                    }
                }
            }
        }
    }
}
