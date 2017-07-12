
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Shell.Applications.Dialogs.SelectItem;
using Sitecore.Shell.Applications.Dialogs.SelectItemWithThumbnail;
using Sitecore.Shell.Controls.Splitters;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;

namespace SF.Foundation.Shell.TabbedRenderingSelector
{
    [UsedImplicitly]
    public class TabbedRenderingForm : SelectItemWithThumbnailForm
    {
        protected Tabstrip Tabs;
        protected Scrollbox Renderings;
        protected VSplitterXmlControl TreeSplitter;
        protected Scrollbox TreeviewContainer;

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object)e, "e");
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            IsOpenPropertiesChecked = Registry.GetBool("/Current_User/SelectRendering/IsOpenPropertiesChecked");
            SelectRenderingOptions renderingOptions = SelectItemOptions.Parse<SelectRenderingOptions>();

            if (renderingOptions.ShowOpenProperties)
            {
                OpenPropertiesBorder.Visible = true;
                OpenProperties.Checked = IsOpenPropertiesChecked;
            }
            if (renderingOptions.ShowPlaceholderName)
            {
                PlaceholderNameBorder.Visible = true;
                PlaceholderName.Value = renderingOptions.PlaceholderName;
            }
            if (!renderingOptions.ShowTree)
            {
                TreeviewContainer.Class = string.Empty;
                TreeviewContainer.Visible = false;
                TreeSplitter.Visible = false;
                GridPanel gridPanel = TreeviewContainer.Parent as GridPanel;
                if (gridPanel != null)
                {
                    gridPanel.SetExtensibleProperty(TreeviewContainer, "class",
                                                    "scDisplayNone");
                }

                //Line of Code that does grouping
                var gruppedSublayouts = renderingOptions.Items.GroupBy(i => i.Parent.Name);

                //Add new tab for each folder
                foreach (IGrouping<string, Item> gruppedSublayout in gruppedSublayouts)
                {
                    var newTab = new Tab();
                    newTab.Header = gruppedSublayout.Key;
                    var newScrollbox = new Scrollbox();
                    newScrollbox.Class = "scScrollbox scFixSize scFixSize4";
                    newScrollbox.Background = "white";
                    newScrollbox.Padding = "0px";
                    newScrollbox.Width = new Unit(100, UnitType.Percentage);
                    newScrollbox.Height = new Unit(100, UnitType.Percentage);
                    newScrollbox.InnerHtml = RenderPreviews(gruppedSublayout);
                    newTab.Controls.Add(newScrollbox);
                    Tabs.Controls.Add(newTab);
                }

                gridPanel = Renderings.Parent as GridPanel;
                if (gridPanel != null)
                {
                    gridPanel.SetExtensibleProperty(Renderings, "class",
                                                    "scDisplayNone");
                }
            }
            else
            {
                var gridPanel = Renderings.Parent as GridPanel;
                if (gridPanel != null)
                {
                    gridPanel.SetExtensibleProperty(Tabs, "class",
                                                    "scDisplayNone");
                }
            }
            //end of new code
            SetOpenPropertiesState(renderingOptions.SelectedItem);
        }

        /// <summary>
        /// Handles click on a non-rendering preview
        /// </summary>
        /// <param name="item">The non-rendering item.</param>
        protected override void OnItemClick(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            ItemCollection children = DataContext.GetChildren(item);
            if (children != null && children.Count > 0)
            {
                Treeview.SetSelectedItem(item);
                Renderings.InnerHtml = RenderPreviews(children);
            }
            else
                SheerResponse.Alert("Please select a rendering item", new string[0]);
            SetOpenPropertiesState(item);
        }

        /// <summary>
        /// Handles the Treeview click event.
        /// </summary>
        [UsedImplicitly]
        protected void Treeview_Click()
        {
            Item selectionItem = Treeview.GetSelectionItem();
            if (selectionItem != null)
            {
                SelectedItemId = string.Empty;
                ItemCollection children = DataContext.GetChildren(selectionItem);
                Renderings.InnerHtml = children == null || children.Count <= 0 ? RenderEmptyPreview(selectionItem) : RenderPreviews(children);
            }
            SetOpenPropertiesState(selectionItem);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is open properties checked.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// <c>true</c> if this instance is open properties checked; otherwise, <c>false</c>.
        /// 
        /// </value>
        [UsedImplicitly]
        protected bool IsOpenPropertiesChecked
        {
            get
            {
                return (string)ServerProperties["IsChecked"] == "1";
            }
            set
            {
                ServerProperties["IsChecked"] = value ? (object)"1" : (object)"0";
            }
        }

        /// <summary>
        /// Gets or sets the open properties.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The open properties.
        /// </value>
        [UsedImplicitly]
        protected Checkbox OpenProperties { get; set; }

        /// <summary>
        /// Gets or sets the open properties border.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The open properties border.
        /// </value>
        [UsedImplicitly]
        protected Border OpenPropertiesBorder { get; set; }

        /// <summary>
        /// Gets or sets the name of the placeholder.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The name of the placeholder.
        /// </value>
        [UsedImplicitly]
        protected Edit PlaceholderName { get; set; }

        /// <summary>
        /// Gets or sets the placeholder name border.
        /// 
        /// </summary>
        /// 
        /// <value>
        /// The placeholder name border.
        /// </value>
        [UsedImplicitly]
        protected Border PlaceholderNameBorder { get; set; }

        /// <summary>
        /// Gets the filter.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>
        /// The filter.
        /// </returns>
        protected override string GetFilter(SelectItemOptions options)
        {
            Assert.ArgumentNotNull((object)options, "options");
            if (options.IncludeTemplatesForDisplay.Count == 0 && options.ExcludeTemplatesForDisplay.Count == 0)
                return string.Empty;
            string list1 = SelectItemForm.GetList(options.IncludeTemplatesForDisplay);
            string list2 = SelectItemForm.GetList(options.ExcludeTemplatesForDisplay);
            if (options.IncludeTemplatesForDisplay.Count > 0 && options.ExcludeTemplatesForDisplay.Count > 0)
                return string.Format("(contains('{0}', ',' + @@templateid + ',') or contains('{0}', ',' + @@templatekey + ',')) and  not (contains('{1}', ',' + @@templateid + ',') or contains('{1}', ',' + @@templatekey + ','))", (object)list1, (object)list2);
            if (options.IncludeTemplatesForDisplay.Count > 0)
                return string.Format("(contains('{0}', ',' + @@templateid + ',') or contains('{0}', ',' + @@templatekey + ','))", (object)list1);
            else
                return "not (contains('{ExcludeList}', ',' + @@templateid + ',') or contains('{0}', ',' + @@templatekey + ',') or @@name='Placeholder Settings' or @@name='Devices' or @@name='Layouts' or @@id='{B4A0FB13-9758-427C-A7EB-1A406C045192}')".Replace("{ExcludeList}", list2);
        }

        /// <summary>
        /// Defines if item is rendering
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>
        /// <c>true</c> of item is a rendering item, and <c>false</c> otherwise
        /// </returns>
        protected bool IsItemRendering(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            ID templateId = ID.Parse("{D1592226-3898-4CE2-B190-090FD5F84A4C}");
            Template template = TemplateManager.GetTemplate(item);
            if (template == null)
                return false;
            else
                return template.DescendsFrom(templateId);
        }

        /// <summary>
        /// Defines if the item can be selected in the dialog
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>
        /// <c>true</c> if selectable; otherwise, <c>false</c>
        /// </returns>
        protected override bool IsItemSelectable(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            return IsItemRendering(item);
        }

        /// <summary>
        /// Handles a click on the OK button.
        /// </summary>
        /// <param name="sender"/><param name="args"/>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        ///             the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        /// <contract><requires name="sender" condition="not null"/><requires name="args" condition="not null"/></contract>
        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull((object)args, "args");
            if (!string.IsNullOrEmpty(SelectedItemId))
            {
                SetDialogResult(ShortID.Parse(SelectedItemId).ToID().ToString());
            }
            else
            {
                Item selectionItem = Treeview.GetSelectionItem();
                if (selectionItem != null && IsItemRendering(selectionItem))
                    SetDialogResult(selectionItem.ID.ToString());
                else
                    SheerResponse.Alert("Please select a rendering item", new string[0]);
            }
        }

        /// <summary>
        /// Handles click on rendering preview
        /// </summary>
        /// <param name="item">The rendering item.</param>
        protected override void OnSelectableItemClick(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            SetOpenPropertiesState(item);
        }

        /// <summary>
        /// Sets the dialog result.
        /// </summary>
        /// <param name="selectedRenderingId">The selected rendering id</param>
        protected void SetDialogResult(string selectedRenderingId)
        {
            Assert.ArgumentNotNull((object)selectedRenderingId, "selectedRenderingId");
            if (!OpenProperties.Disabled)
                Registry.SetBool("/Current_User/SelectRendering/IsOpenPropertiesChecked", IsOpenPropertiesChecked);
            SheerResponse.SetDialogValue(selectedRenderingId + "," + WebUtil.GetFormValue("PlaceholderName").Replace(",", "-c-") + "," + (OpenProperties.Checked ? "1" : "0"));
            SheerResponse.CloseWindow();
        }

        /// <summary>
        /// Sets the dialog result.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        protected override void SetDialogResult(Item selectedItem)
        {
            Assert.ArgumentNotNull((object)selectedItem, "selectedItem");
            SetDialogResult(selectedItem.ID.ToString());
        }

        /// <summary>
        /// Renders empty preview
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>
        /// Previews markup
        /// </returns>
        private string RenderEmptyPreview(Item item)
        {
            HtmlTextWriter output = new HtmlTextWriter((TextWriter)new StringWriter());
            output.Write("<table class='scEmptyPreview'>");
            output.Write("<tbody>");
            output.Write("<tr>");
            output.Write("<td>");
            if (item == null)
                output.Write(Translate.Text("None available."));
            else if (IsItemRendering(item))
            {
                output.Write("<div class='scImageContainer'>");
                output.Write("<span style='height:100%; width:1px; display:inline-block;'></span>");
                string str = item.Appearance.Icon;
                int num1 = 48;
                int num2 = 48;
                if (!string.IsNullOrEmpty(item.Appearance.Thumbnail) && item.Appearance.Thumbnail != Settings.DefaultThumbnail)
                {
                    string thumbnailSrc = UIUtil.GetThumbnailSrc(item, 128, 128);
                    if (!string.IsNullOrEmpty(thumbnailSrc))
                    {
                        str = thumbnailSrc;
                        num1 = 128;
                        num2 = 128;
                    }
                }
                new ImageBuilder()
                {
                    Align = "absmiddle",
                    Src = str,
                    Width = num2,
                    Height = num1
                }.Render(output);
                output.Write("</div>");
                output.Write("<span class='scDisplayName'>");
                output.Write(item.DisplayName);
                output.Write("</span>");
            }
            else
                output.Write(Translate.Text("Please select a rendering item"));
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("</tbody>");
            output.Write("</table>");
            return output.InnerWriter.ToString();
        }

        /// <summary>
        /// Renders previews
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns>
        /// Previews markup
        /// </returns>
        private string RenderPreviews(IEnumerable<Item> items)
        {
            Assert.ArgumentNotNull((object)items, "items");
            HtmlTextWriter output = new HtmlTextWriter((TextWriter)new StringWriter());
            bool flag = false;
            foreach (Item obj in items)
            {
                RenderItemPreview(obj, output);
                flag = true;
            }
            if (!flag)
                return RenderEmptyPreview((Item)null);
            else
                return output.InnerWriter.ToString();
        }

        /// <summary>
        /// Renders previews
        /// </summary>
        /// <param name="items">The items</param>
        /// <returns>
        /// Previews markup
        /// </returns>
        private string RenderPreviews(ItemCollection items)
        {
            Assert.ArgumentNotNull((object)items, "items");
            HtmlTextWriter output = new HtmlTextWriter((TextWriter)new StringWriter());
            foreach (Item obj in (CollectionBase)items)
                RenderItemPreview(obj, output);
            return output.InnerWriter.ToString();
        }

        /// <summary>
        /// Renders the help.
        /// </summary>
        /// <param name="item">The item.</param>
        private void SetOpenPropertiesState(Item item)
        {
            if (item == null || !IsItemRendering(item))
            {
                OpenProperties.Disabled = true;
                OpenProperties.Checked = false;
            }
            else
            {
                switch (item["Open Properties After Add"])
                {
                    case "-":
                    case "":
                        OpenProperties.Disabled = false;
                        OpenProperties.Checked = IsOpenPropertiesChecked;
                        break;
                    case "0":
                        if (!OpenProperties.Disabled)
                            IsOpenPropertiesChecked = OpenProperties.Checked;
                        OpenProperties.Disabled = true;
                        OpenProperties.Checked = false;
                        break;
                    case "1":
                        if (!OpenProperties.Disabled)
                            IsOpenPropertiesChecked = OpenProperties.Checked;
                        OpenProperties.Disabled = true;
                        OpenProperties.Checked = true;
                        break;
                }
            }
        }

    }

}
