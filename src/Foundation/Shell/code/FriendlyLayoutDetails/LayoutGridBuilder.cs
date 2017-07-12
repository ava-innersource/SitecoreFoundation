using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Layouts;
using Sitecore.Pipelines.RenderLayoutGridRendering;
using Sitecore.Reflection;
using Sitecore.Resources;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XmlControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace SF.Foundation.Shell
{


    public class FriendlyLayoutGridBuilder
    {
    // Fields
    private string @class;
    private string copyToClick;
    private string editPlaceholderClick;
    private string editRenderingClick;
    private string id;
    private string openDeviceClick;
    private string value;

    // Methods
    private void BuildDevice(GridPanel grid, LayoutDefinition layout, DeviceItem deviceItem)
    {
        XmlControl webControl;
        Assert.ArgumentNotNull(grid, "grid");
        Assert.ArgumentNotNull(deviceItem, "deviceItem");
        if (!string.IsNullOrEmpty(this.OpenDeviceClick))
        {
            webControl = Sitecore.Resources.Resource.GetWebControl("LayoutFieldDevice") as XmlControl;
        }
        else
        {
            webControl = Sitecore.Resources.Resource.GetWebControl("LayoutFieldDeviceReadOnly") as XmlControl;
        }
        Assert.IsNotNull(webControl, typeof(XmlControl));
        grid.Controls.Add(webControl);
        string str = StringUtil.GetString(new string[] { this.OpenDeviceClick }).Replace("$Device", deviceItem.ID.ToString());
        string str2 = StringUtil.GetString(new string[] { this.CopyToClick }).Replace("$Device", deviceItem.ID.ToString());
        ReflectionUtil.SetProperty(webControl, "DeviceName", deviceItem.DisplayName);
        ReflectionUtil.SetProperty(webControl, "DeviceIcon", deviceItem.InnerItem.Appearance.Icon);
        ReflectionUtil.SetProperty(webControl, "DblClick", str);
        ReflectionUtil.SetProperty(webControl, "Copy", str2);
        string str3 = "<span style=\"color:#999999\">" + Translate.Text("[No layout specified]") + "</span>";
        int index = 0;
        int num2 = 0;
        Control parent = webControl["ControlsPane"] as Control;
        Control control3 = webControl["PlaceholdersPane"] as Control;
        if (layout != null)
        {
            DeviceDefinition device = layout.GetDevice(deviceItem.ID.ToString());
            string str4 = device.Layout;
            if (!string.IsNullOrEmpty(str4))
            {
                Item item = Client.ContentDatabase.GetItem(str4);
                if (item != null)
                {
                    str3 = Images.GetImage(item.Appearance.Icon, 0x10, 0x10, "absmiddle", "0px 4px 0px 0px") + item.DisplayName;
                }
            }
            ArrayList renderings = device.Renderings;
            if ((renderings != null) && (renderings.Count > 0))
            {
                Border control = new Border();
                Context.ClientPage.AddControl(parent, control);
                foreach (RenderingDefinition definition2 in renderings)
                {
                    int conditionsCount = 0;
                    if ((definition2.Rules != null) && !definition2.Rules.IsEmpty)
                    {
                        conditionsCount = definition2.Rules.Elements("rule").Count<XElement>();
                    }
                    this.BuildRendering(control, device, definition2, index, conditionsCount);
                    index++;
                }
            }
            ArrayList placeholders = device.Placeholders;
            if ((placeholders != null) && (placeholders.Count > 0))
            {
                Border border2 = new Border();
                Context.ClientPage.AddControl(control3, border2);
                foreach (PlaceholderDefinition definition3 in placeholders)
                {
                    this.BuildPlaceholder(border2, device, definition3);
                    num2++;
                }
            }
        }
        ReflectionUtil.SetProperty(webControl, "LayoutName", str3);
        if (index == 0)
        {
            Context.ClientPage.AddControl(parent, new System.Web.UI.LiteralControl("<span style=\"color:#999999\">" + Translate.Text("[No renderings specified.]") + "</span>"));
        }
        if (num2 == 0)
        {
            Context.ClientPage.AddControl(control3, new System.Web.UI.LiteralControl("<span style=\"color:#999999\">" + Translate.Text("[No placeholder settings were specified]") + "</span>"));
        }
    }

    public void BuildGrid(Control parent)
    {
        Assert.ArgumentNotNull(parent, "parent");
        GridPanel child = new GridPanel();
        parent.Controls.Add(child);
        child.RenderAs = RenderAs.Literal;
        child.Width = Unit.Parse("100%");
        child.Attributes["Class"] = this.Class;
        child.Attributes["CellSpacing"] = "2";
        child.Attributes["id"] = this.ID;
        LayoutDefinition layout = null;
        string xml = StringUtil.GetString(new string[] { this.Value });
        if (xml.Length > 0)
        {
            layout = LayoutDefinition.Parse(xml);
        }
        foreach (DeviceItem item in Client.ContentDatabase.Resources.Devices.GetAll())
        {
            this.BuildDevice(child, layout, item);
        }
    }

    private void BuildPlaceholder(Border border, DeviceDefinition deviceDefinition, PlaceholderDefinition placeholderDefinition)
    {
        string str3;
        Assert.ArgumentNotNull(border, "border");
        Assert.ArgumentNotNull(deviceDefinition, "deviceDefinition");
        Assert.ArgumentNotNull(placeholderDefinition, "placeholderDefinition");
        string metaDataItemId = placeholderDefinition.MetaDataItemId;
        Border child = new Border();
        border.Controls.Add(child);
        string str2 = StringUtil.GetString(new string[] { this.EditPlaceholderClick }).Replace("$Device", deviceDefinition.ID).Replace("$UniqueID", placeholderDefinition.UniqueId);
        Assert.IsNotNull(metaDataItemId, "placeholder id");
        Item item = Client.ContentDatabase.GetItem(metaDataItemId);
        if (item != null)
        {
            string displayName = item.DisplayName;
            str3 = Images.GetImage(item.Appearance.Icon, 0x10, 0x10, "absmiddle", "0px 4px 0px 0px") + displayName;
        }
        else
        {
            str3 = Images.GetImage("Imaging/16x16/layer_blend.png", 0x10, 0x10, "absmiddle", "0px 4px 0px 0px") + placeholderDefinition.Key;
        }
        if (!string.IsNullOrEmpty(str2))
        {
            child.RollOver = true;
            child.Class = "scRollOver";
            child.Click = str2;
        }
        Sitecore.Web.UI.HtmlControls.Literal literal = new Sitecore.Web.UI.HtmlControls.Literal("<div style=\"padding:2\">" + str3 + "</div>");
        child.Controls.Add(literal);
    }

    private void BuildRendering(Border border, DeviceDefinition deviceDefinition, RenderingDefinition renderingDefinition, int index, int conditionsCount)
    {
        Assert.ArgumentNotNull(border, "border");
        Assert.ArgumentNotNull(deviceDefinition, "deviceDefinition");
        Assert.ArgumentNotNull(renderingDefinition, "renderingDefinition");
        string itemID = renderingDefinition.ItemID;
        if (itemID != null)
        {
            Item item = Client.ContentDatabase.GetItem(itemID);
            if (item != null)
            {
                string displayName = item.DisplayName;
                string icon = item.Appearance.Icon;
                string str4 = string.Empty;
                string initialMarkup = Images.GetImage(icon, 0x10, 0x10, "absmiddle", "0px 4px 0px 0px") + displayName;
                if ((str4.Length > 0) && (str4 != "content"))
                {
                    string str7 = initialMarkup;
                    initialMarkup = str7 + " " + Translate.Text("in") + " " + str4 + ".";
                }
                if (conditionsCount > 1)
                {
                    initialMarkup = initialMarkup + @"<span class=""{((conditionsCount > 9) ? ""scConditionContainer scLongConditionContainer"" : ""scConditionContainer"")}"">{conditionsCount}</span>";
                }
                initialMarkup = RenderLayoutGridRenderingPipeline.Run(renderingDefinition, initialMarkup);
                Border child = new Border();
                border.Controls.Add(child);
                string str6 = StringUtil.GetString(new string[] { this.EditRenderingClick }).Replace("$Device", deviceDefinition.ID).Replace("$Index", index.ToString());
                if (!string.IsNullOrEmpty(str6))
                {
                    child.RollOver = true;
                    child.Class = "scRollOver";
                    child.Click = str6;
                }

                if (!string.IsNullOrEmpty(renderingDefinition.Datasource))
                {
                    var ds = Client.ContentDatabase.GetItem(renderingDefinition.Datasource);
                    if (ds != null)
                    {
                        initialMarkup += " - " + ds.Name;
                    }
                }
                else
                {
                    initialMarkup += " - No Data Source";
                }

                if (!string.IsNullOrEmpty(renderingDefinition.Placeholder))
                {
                    var nesting = renderingDefinition.Placeholder.Split('/');
                    StringBuilder sb = new StringBuilder();
                    foreach (var nest in nesting)
                    {
                        if (nest.IndexOf('_') > -1)
                        {
                            var keys = nest.Split('_');
                            sb.Append(keys[0]);
                        }
                        else 
                        {
                            sb.Append(nest);
                        }

                        sb.Append(" > ");
                    }
                    var friendlyPlaceholders = sb.ToString();
                    if (friendlyPlaceholders.IndexOf(" > ") > -1 )
                    {
                        friendlyPlaceholders = friendlyPlaceholders.Substring(0, friendlyPlaceholders.Length - 2);
                    }

                    initialMarkup += string.Format(@"<br /><span class=""friendlyPlaceholder"">{0}</a>", friendlyPlaceholders);


                }
                

                Sitecore.Web.UI.HtmlControls.Literal literal = new Sitecore.Web.UI.HtmlControls.Literal("<div class='scRendering' style='padding:2px;position:relative'>" + initialMarkup + "</div>");
                child.Controls.Add(literal);
            }
        }
    }

    // Properties
    public string Class
    {
        get 
        {
            return StringUtil.GetString(new string[] { this.@class });
        }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            this.@class = value;
        }
    }

    public string CopyToClick
    {
        get 
        {
            return StringUtil.GetString(new string[] { this.copyToClick });
        }
        set
        {
            Assert.ArgumentNotNullOrEmpty(value, "value");
            this.copyToClick = value;
        }
    }

    public string EditPlaceholderClick
    {
        get
        {
            return StringUtil.GetString(new string[] { this.editPlaceholderClick });
        }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            this.editPlaceholderClick = value;
        }
    }

    public string EditRenderingClick
    {
        get { return StringUtil.GetString(new string[] { this.editRenderingClick }); }

        set
        {
            Assert.ArgumentNotNull(value, "value");
            this.editRenderingClick = value;
        }
    }

    public string ID
    {
        get { return StringUtil.GetString(new string[] { this.id });}
        set
        {
            Assert.ArgumentNotNullOrEmpty(value, "value");
            this.id = value;
        }
    }

    public string OpenDeviceClick
    {
        get { return StringUtil.GetString(new string[] { this.openDeviceClick }); }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            this.openDeviceClick = value;
        }
    }

    public string Value
    {
        get { return StringUtil.GetString(new string[] { this.value }); }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            this.value = value;
        }
    }
}

 

}
