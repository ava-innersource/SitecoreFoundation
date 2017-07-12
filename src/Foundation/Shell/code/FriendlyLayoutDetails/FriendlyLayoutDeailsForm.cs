using Sitecore;
using Sitecore.Data.Events;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Layouts;
using Sitecore.Shell.Applications.Dialogs;
using Sitecore.Shell.Applications.Dialogs.LayoutDetails;
using Sitecore.Shell.Applications.Layouts.DeviceEditor;
using Sitecore.Shell.Framework;
using Sitecore.Shell.Web.UI;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Xml;
using Sitecore.Xml.Patch;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SF.Foundation.Shell
{

    public class FriendlyLayoutDeailsForm : DialogForm
{
    // Fields
    protected Border FinalLayoutNoVersionWarningPanel;
    protected Border FinalLayoutPanel;
    protected Tab FinalLayoutTab;
    protected Border LayoutPanel;
    protected Tab SharedLayoutTab;
    protected Tabstrip Tabs;
    protected Literal WarningTitle;

    // Methods
    protected void CopyDevice(string deviceID)
    {
        Assert.ArgumentNotNullOrEmpty(deviceID, "deviceID");
        NameValueCollection parameters = new NameValueCollection();
        parameters.Add("deviceid", deviceID);
        Context.ClientPage.Start(this, "CopyDevicePipeline", parameters);
    }

    private void CopyDevice(XmlNode sourceDevice, ListString devices)
    {
        Assert.ArgumentNotNull(sourceDevice, "sourceDevice");
        Assert.ArgumentNotNull(devices, "devices");
        XmlDocument doc = XmlUtil.LoadXml(this.GetActiveLayout());
        CopyDevices(doc, devices, sourceDevice);
        this.SetActiveLayout(doc.OuterXml);
    }

    private void CopyDevice(XmlNode sourceDevice, ListString devices, Item item)
    {
        Assert.ArgumentNotNull(sourceDevice, "sourceDevice");
        Assert.ArgumentNotNull(devices, "devices");
        Assert.ArgumentNotNull(item, "item");
        Field layoutField = this.GetLayoutField(item);
        LayoutField field2 = layoutField;
        XmlDocument data = field2.Data;
        CopyDevices(data, devices, sourceDevice);
        item.Editing.BeginEdit();
        layoutField.Value = data.OuterXml;
        item.Editing.EndEdit();
    }

    protected void CopyDevicePipeline(ClientPipelineArgs args)
    {
        Assert.ArgumentNotNull(args, "args");
        if (args.IsPostBack)
        {
            if (!string.IsNullOrEmpty(args.Result) && (args.Result != "undefined"))
            {
                string[] strArray = args.Result.Split(new char[] { '^' });
                string str = StringUtil.GetString(new string[] { args.Parameters["deviceid"] });
                ListString devices = new ListString(strArray[0]);
                string itemPath = strArray[1];
                XmlNode sourceDevice = this.GetDoc().SelectSingleNode("/r/d[@id='" + str + "']");
                if (itemPath == WebUtil.GetQueryString("id"))
                {
                    if (sourceDevice != null)
                    {
                        this.CopyDevice(sourceDevice, devices);
                    }
                }
                else if (sourceDevice != null)
                {
                    Item itemNotNull = Client.GetItemNotNull(itemPath);
                    this.CopyDevice(sourceDevice, devices, itemNotNull);
                }
                this.Refresh();
            }
        }
        else
        {
            XmlDocument doc = this.GetDoc();
            WebUtil.SetSessionValue("SC_DEVICEEDITOR", doc.OuterXml);
            var urlString = new UrlString(UIUtil.GetUri("control:CopyDeviceTo"));
            urlString.Add("de", StringUtil.GetString(new string[] { args.Parameters["deviceid"] }));
            urlString.Add("fo", WebUtil.GetQueryString("id"));

            SheerResponse.ShowModalDialog(urlString.ToString(), "1200px", "700px", string.Empty, true);
            args.WaitForPostBack();
        }
    }

    private static void CopyDevices(XmlDocument doc, ListString devices, XmlNode sourceDevice)
    {
        Assert.ArgumentNotNull(doc, "doc");
        Assert.ArgumentNotNull(devices, "devices");
        Assert.ArgumentNotNull(sourceDevice, "sourceDevice");
        XmlNode node = doc.ImportNode(sourceDevice, true);
        foreach (string str in devices)
        {
            if (doc.DocumentElement != null)
            {
                XmlNode node2 = doc.DocumentElement.SelectSingleNode("d[@id='" + str + "']");
                if (node2 != null)
                {
                    XmlUtil.RemoveNode(node2);
                }
                node2 = node.CloneNode(true);
                XmlUtil.SetAttribute("id", str, node2);
                doc.DocumentElement.AppendChild(node2);
            }
        }
    }

    protected void EditPlaceholder(string deviceID, string uniqueID)
    {
        Assert.ArgumentNotNull(deviceID, "deviceID");
        Assert.ArgumentNotNullOrEmpty(uniqueID, "uniqueID");
        NameValueCollection parameters = new NameValueCollection();
        parameters.Add("deviceid", deviceID);
        parameters.Add("uniqueid", uniqueID);
        Context.ClientPage.Start(this, "EditPlaceholderPipeline", parameters);
    }

    protected void EditPlaceholderPipeline(ClientPipelineArgs args)
    {
        Assert.ArgumentNotNull(args, "args");
        LayoutDefinition definition = LayoutDefinition.Parse(this.GetDoc().OuterXml);
        PlaceholderDefinition placeholder = definition.GetDevice(args.Parameters["deviceid"]).GetPlaceholder(args.Parameters["uniqueid"]);
        if (placeholder != null)
        {
            if (args.IsPostBack)
            {
                if (!string.IsNullOrEmpty(args.Result) && (args.Result != "undefined"))
                {
                    string str;
                    Item item = SelectPlaceholderSettingsOptions.ParseDialogResult(args.Result, Client.ContentDatabase, out str);
                    if (item != null)
                    {
                        placeholder.MetaDataItemId = item.Paths.FullPath;
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        placeholder.Key = str;
                    }
                    this.SetActiveLayout(definition.ToXml());
                    this.Refresh();
                }
            }
            else
            {
                Item item2 = string.IsNullOrEmpty(placeholder.MetaDataItemId) ? null : Client.ContentDatabase.GetItem(placeholder.MetaDataItemId);
                SelectPlaceholderSettingsOptions options = new SelectPlaceholderSettingsOptions {
                    TemplateForCreating = null,
                    PlaceholderKey = placeholder.Key,
                    CurrentSettingsItem = item2,
                    SelectedItem = item2,
                    IsPlaceholderKeyEditable = true
                };
                SheerResponse.ShowModalDialog(options.ToUrlString().ToString(), "460px", "460px", string.Empty, true);
                args.WaitForPostBack();
            }
        }
    }

    protected void EditRendering(string deviceID, string index)
    {
        Assert.ArgumentNotNull(deviceID, "deviceID");
        Assert.ArgumentNotNull(index, "index");
        NameValueCollection parameters = new NameValueCollection();
        parameters.Add("deviceid", deviceID);
        parameters.Add("index", index);
        Context.ClientPage.Start(this, "EditRenderingPipeline", parameters);
    }

    protected void EditRenderingPipeline(ClientPipelineArgs args)
    {
        Assert.ArgumentNotNull(args, "args");
        RenderingParameters parameters = new RenderingParameters {
            Args = args,
            DeviceId = StringUtil.GetString(new string[] { args.Parameters["deviceid"] }),
            SelectedIndex = MainUtil.GetInt(StringUtil.GetString(new string[] { args.Parameters["index"] }), 0),
            Item = UIUtil.GetItemFromQueryString(Client.ContentDatabase)
        };
        if (!args.IsPostBack)
        {
            XmlDocument doc = this.GetDoc();
            WebUtil.SetSessionValue("SC_DEVICEEDITOR", doc.OuterXml);
        }
        if (parameters.Show())
        {
            XmlDocument document2 = XmlUtil.LoadXml(WebUtil.GetSessionString("SC_DEVICEEDITOR"));
            WebUtil.SetSessionValue("SC_DEVICEEDITOR", null);
            this.SetActiveLayout(GetLayoutValue(document2));
            this.Refresh();
        }
    }

    protected string GetActiveLayout()
    {
        if (this.ActiveTab == TabType.Final)
        {
            return this.FinalLayout;
        }
        return this.Layout;
    }

    private static Item GetCurrentItem()
    {
        string queryString = WebUtil.GetQueryString("id");
        Language language = Language.Parse(WebUtil.GetQueryString("la"));
        Sitecore.Data.Version version = Sitecore.Data.Version.Parse(WebUtil.GetQueryString("vs"));
        return Client.ContentDatabase.GetItem(queryString, language, version);
    }

    protected string GetDialogResult()
    {
        LayoutDetailsDialogResult result = new LayoutDetailsDialogResult {
            Layout = this.Layout,
            FinalLayout = this.FinalLayout,
            VersionCreated = this.VersionCreated
        };
        return result.ToString();
    }

    private XmlDocument GetDoc()
    {
        XmlDocument document = new XmlDocument();
        string activeLayout = this.GetActiveLayout();
        if (activeLayout.Length > 0)
        {
            document.LoadXml(activeLayout);
            return document;
        }
        document.LoadXml("<r/>");
        return document;
    }

    private Field GetLayoutField(Item item)
    {
        if (this.ActiveTab == TabType.Final)
        {
            return item.Fields[FieldIDs.FinalLayoutField];
        }
        return item.Fields[FieldIDs.LayoutField];
    }

    private static string GetLayoutValue(XmlDocument doc)
    {
        Assert.ArgumentNotNull(doc, "doc");
        XmlNodeList list = doc.SelectNodes("/r/d");
        if ((list != null) && (list.Count != 0))
        {
            foreach (XmlNode node in list)
            {
                if ((node.ChildNodes.Count > 0) || (XmlUtil.GetAttribute("l", node).Length > 0))
                {
                    return doc.OuterXml;
                }
            }
        }
        return string.Empty;
    }

    public override void HandleMessage(Message message)
    {
        Assert.ArgumentNotNull(message, "message");
        if (message.Name == "item:addversion")
        {
            Item currentItem = GetCurrentItem();
            Dispatcher.Dispatch(message, currentItem);
        }
        else
        {
            base.HandleMessage(message);
        }
    }

    private void ItemSavedNotification(object sender, ItemSavedEventArgs args)
    {
        this.VersionCreated = true;
        this.ToggleVisibilityOfControlsOnFinalLayoutTab(args.Item);
        SheerResponse.SetDialogValue(this.GetDialogResult());
    }

    protected override void OnLoad(EventArgs e)
    {
        Assert.CanRunApplication("Content Editor/Ribbons/Chunks/Layout");
        Assert.ArgumentNotNull(e, "e");
        base.OnLoad(e);
        this.Tabs.OnChange += (sender, args) => this.Refresh();
        if (!Context.ClientPage.IsEvent)
        {
            Item currentItem = GetCurrentItem();
            Assert.IsNotNull(currentItem, "Item not found");
            this.Layout = LayoutField.GetFieldValue(currentItem.Fields[FieldIDs.LayoutField]);
            Field field = currentItem.Fields[FieldIDs.FinalLayoutField];
            if (currentItem.Name != "__Standard Values")
            {
                this.LayoutDelta = field.GetValue(false, false) ?? field.GetInheritedValue(false);
            }
            else
            {
                this.LayoutDelta = field.GetStandardValue();
            }
            this.ToggleVisibilityOfControlsOnFinalLayoutTab(currentItem);
            this.Refresh();
        }
        SiteContext site = Context.Site;
        if (site != null)
        {
            site.Notifications.ItemSaved += new ItemSavedDelegate(this.ItemSavedNotification);
        }
    }

    protected override void OnOK(object sender, EventArgs args)
    {
        Assert.ArgumentNotNull(sender, "sender");
        Assert.ArgumentNotNull(args, "args");
        SheerResponse.SetDialogValue(this.GetDialogResult());
        base.OnOK(sender, args);
    }

    protected void OpenDevice(string deviceID)
    {
        Assert.ArgumentNotNullOrEmpty(deviceID, "deviceID");
        NameValueCollection parameters = new NameValueCollection();
        parameters.Add("deviceid", deviceID);
        Context.ClientPage.Start(this, "OpenDevicePipeline", parameters);
    }

    protected void OpenDevicePipeline(ClientPipelineArgs args)
    {
        Assert.ArgumentNotNull(args, "args");
        if (args.IsPostBack)
        {
            if (!string.IsNullOrEmpty(args.Result) && (args.Result != "undefined"))
            {
                XmlDocument doc = XmlUtil.LoadXml(WebUtil.GetSessionString("SC_DEVICEEDITOR"));
                WebUtil.SetSessionValue("SC_DEVICEEDITOR", null);
                if (doc != null)
                {
                    this.SetActiveLayout(GetLayoutValue(doc));
                }
                else
                {
                    this.SetActiveLayout(string.Empty);
                }
                this.Refresh();
            }
        }
        else
        {
            XmlDocument document2 = this.GetDoc();
            WebUtil.SetSessionValue("SC_DEVICEEDITOR", document2.OuterXml);
            UrlString str = new UrlString(UIUtil.GetUri("control:DeviceEditor"));
            str.Append("de", StringUtil.GetString(new string[] { args.Parameters["deviceid"] }));
            str.Append("id", WebUtil.GetQueryString("id"));
            str.Append("vs", WebUtil.GetQueryString("vs"));
            str.Append("la", WebUtil.GetQueryString("la"));
            ModalDialogOptions options = new ModalDialogOptions(str.ToString()) {
                Response = true,
                Width = "700"
            };
            Context.ClientPage.ClientResponse.ShowModalDialog(options);
            args.WaitForPostBack();
        }
    }

    private void Refresh()
    {
        string activeLayout = this.GetActiveLayout();
        Control renderingContainer = (this.ActiveTab == TabType.Final) ? this.FinalLayoutPanel : this.LayoutPanel;
        this.RenderLayoutGridBuilder(activeLayout, renderingContainer);
    }

    private void RenderLayoutGridBuilder(string layoutValue, Control renderingContainer)
    {
        string str = renderingContainer.ID + "LayoutGrid";
        FriendlyLayoutGridBuilder builder = new FriendlyLayoutGridBuilder
        {
            ID = str,
            Value = layoutValue,
            EditRenderingClick = "EditRendering(\"$Device\", \"$Index\")",
            EditPlaceholderClick = "EditPlaceholder(\"$Device\", \"$UniqueID\")",
            OpenDeviceClick = "OpenDevice(\"$Device\")",
            CopyToClick = "CopyDevice(\"$Device\")"
        };
        renderingContainer.Controls.Clear();
        builder.BuildGrid(renderingContainer);
        if (Context.ClientPage.IsEvent)
        {
            SheerResponse.SetOuterHtml(renderingContainer.ID, renderingContainer);
            SheerResponse.Eval("if (!scForm.browser.isIE) { scForm.browser.initializeFixsizeElements(); }");
        }
    }

    protected void SetActiveLayout(string value)
    {
        if (this.ActiveTab == TabType.Final)
        {
            this.FinalLayout = value;
        }
        else
        {
            this.Layout = value;
        }
    }

    protected void ToggleVisibilityOfControlsOnFinalLayoutTab(Item item)
    {
        bool flag = item.Versions.Count > 0;
        this.FinalLayoutPanel.Visible = flag;
        this.FinalLayoutNoVersionWarningPanel.Visible = !flag;
        if (!flag)
        {
            this.WarningTitle.Text = string.Format(Translate.Text("The current item does not have a version in \"{0}\"."), item.Language.GetDisplayName());
        }
    }

    // Properties
    private TabType ActiveTab
    {
        get
        {
            switch (this.Tabs.Active)
            {
                case 0:
                    return TabType.Shared;

                case 1:
                    return TabType.Final;
            }
            return TabType.Unknown;
        }
    }

    public string FinalLayout
    {
        get
        {
            string layoutDelta = this.LayoutDelta;
            if (string.IsNullOrWhiteSpace(this.Layout) || string.IsNullOrWhiteSpace(layoutDelta))
            {
                return this.Layout;
            }
            if (XmlPatchUtils.IsXmlPatch(layoutDelta))
            {
                return XmlDeltas.ApplyDelta(this.Layout, layoutDelta);
            }
            return layoutDelta;
        }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            if (!string.IsNullOrWhiteSpace(this.Layout))
            {
                this.LayoutDelta = XmlDeltas.GetDelta(value, this.Layout);
            }
            else
            {
                this.LayoutDelta = value;
            }
        }
    }

    public string Layout
    {
        get 
        {
            return StringUtil.GetString(base.ServerProperties["Layout"]);
        }
        set
        {
            Assert.ArgumentNotNull(value, "value");
            base.ServerProperties["Layout"] = value;
        }
    }

    protected string LayoutDelta
    {
        get {
            return StringUtil.GetString(base.ServerProperties["LayoutDelta"]);
        }
        set
        {
            base.ServerProperties["LayoutDelta"] = value;
        }
    }

    protected bool VersionCreated
    {
        get
        {
            return MainUtil.GetBool(base.ServerProperties["VersionCreated"], false);
        }
        set
        {
            base.ServerProperties["VersionCreated"] = value;
        }
    }

    // Nested Types
    private enum TabType
    {
        Shared,
        Final,
        Unknown
    }
}

}
