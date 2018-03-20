using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using SF.Foundation.Components;

namespace SF.Feature.Handlebars
{
    public class DynamicItem : DynamicObject
    {
        public Item Item { get; set; }

        public object Model { get; set; }

        public DynamicItem(Item item)
        {
            Item = item;
            Item.Fields.ReadAll();
        }

        public bool IsPageEditorEditing
        {
            get
            {
                return Sitecore.Context.PageMode.IsExperienceEditorEditing;
            }
        }

        public bool IsExperienceEditorEditing
        {
            get
            {
                return Sitecore.Context.PageMode.IsExperienceEditorEditing;
            }
        }

        public string ItemUrl
        {
            get
            {
                return Item.GetItemUrl();
            }
        }
        
        public string ItemId
        {
            get
            {
                return Item.ID.ToString();
            }
        }

        public IEnumerable<DynamicItem> Children
        {
            get
            {
                foreach(Item child in Item.Children)
                {
                    yield return new DynamicItem(child);
                }
            }
        }

        public DynamicItem ContextItem
        {
            get
            {
                return new DynamicItem(Sitecore.Context.Item);
            }
        }

        public int Children_Count
        {
            get
            {
                return Item.Children.Count;
            }
        }

        public bool HasLayout
        {
            get
            {
                return Item.HasLayout();
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var names = new List<string>();
            names.AddRange(base.GetDynamicMemberNames());

            foreach (Field field in Item.Fields)
            {
                if (!field.IsStandardTemplateField())
                {
                    names.Add(field.Name);
                    names.Add(field.Name + "_Value");

                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.MultilistField)
                    {
                        names.Add(field.Name + "_Count");
                    }
                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.LinkField)
                    {
                        names.Add(field.Name + "_Url");
                        names.Add(field.Name + "_Text");
                    }
                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.ImageField)
                    {
                        names.Add(field.Name + "_Src");
                        names.Add(field.Name + "_Alt");
                    }
                    if (Item.Fields[field.ID].Type.ToLower() == "date" ||
                        Item.Fields[field.ID].Type.ToLower() == "datetime")
                    {
                        names.Add(field.Name + "_ShortDate");
                        names.Add(field.Name + "_LongDate");
                    }
                }
            }

            return names;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var binderNameArr = binder.Name.Split('_');
            var binderName = binderNameArr[0];
            var binderProperty = binderNameArr.Length == 2 ? binderNameArr[1] : string.Empty;

            foreach (Sitecore.Data.Fields.Field field in Item.Fields)
            {
                var fieldValue = Item.Fields[field.ID].Value.ToString();
                var propName = field.Name.Replace(" ", string.Empty);

                //Removed Fields will return with no field name, so checking for field name
                //Also checking for duplicate field values, which also cause issues.
                if (propName == binderName)
                {
                    #region Value
                    if (binderProperty == "Value")
                    {
                        result = field.Value;
                        return true;
                    }
                    #endregion
                    if (binderProperty == "Field")
                    {
                        result = field;
                        return true;
                    }
                    #region Multilist
                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.MultilistField)
                    {
                        var multiField = (Sitecore.Data.Fields.MultilistField)Item.Fields[field.ID];
                        List<DynamicItem> items = new List<DynamicItem>();
                        foreach (var item in multiField.GetItems())
                        {
                            items.Add(new DynamicItem(item));
                        }

                        if (binderProperty == "Count")
                        {
                            result = items.Count;
                            return true;
                        }

                        result = items;
                        return true;
                    }
                    #endregion
                    #region LinkField
                    if (Item.Fields[field.ID].Type.ToLower() == "droplink" ||
                        Item.Fields[field.ID].Type.ToLower() == "droptree" ||
                        Item.Fields[field.ID].Type.ToLower() == "grouped droplink")
                    {

                        Guid referenceID = Guid.Empty;
                        if (Guid.TryParse(Item.Fields[field.ID].Value, out referenceID))
                        {
                            var referencedItem = Item.Database.GetItem(new Sitecore.Data.ID(referenceID));
                            var referenceObj = new DynamicItem(referencedItem);
                            result = referenceObj;
                            return true;
                        }
                    }
                    #endregion
                    #region Field Renderer
                    if (string.IsNullOrEmpty(binderProperty))
                    {

                        string renderedValue = Sitecore.Web.UI.WebControls.FieldRenderer.Render(field.Item, field.Name);
                        result = renderedValue;
                        return true;
                    }
                    #endregion
                    #region Binder Properties

                    #region Url
                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.LinkField)
                    {
                        var linkField = (Sitecore.Data.Fields.LinkField)Item.Fields[field.ID];
                        if (binderProperty == "Url")
                        {
                            var linkUrl = linkField.GetFriendlyUrl();
                            result = linkUrl;
                            return true;
                        }

                        if (binderProperty == "Text")
                        {
                            result = linkField.Text;
                            return true;
                        }

                    }
                    #endregion
                    #region Image

                    if (Sitecore.Data.Fields.FieldTypeManager.GetField(field) is Sitecore.Data.Fields.ImageField)
                    {
                        var imgField = (Sitecore.Data.Fields.ImageField)Item.Fields[field.ID];
                        if (imgField.MediaItem != null)
                        {
                            if (binderProperty == "Src")
                            {
                                string imgUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);
                                result = imgUrl;
                                return true;
                            }
                            if (binderProperty == "Alt")
                            {
                                string altText = imgField.Alt;
                                result = altText;
                                return true;
                            }
                        }
                    }
                    #endregion
                    #region Date
                    if (Item.Fields[field.ID].Type.ToLower() == "date" ||
                        Item.Fields[field.ID].Type.ToLower() == "datetime")
                    {
                        var dateField = (Sitecore.Data.Fields.DateField)Item.Fields[field.ID];
                        if (dateField.DateTime != DateTime.MinValue && dateField.DateTime != DateTime.MaxValue)
                        {
                            if (binderProperty == "ShortDate")
                            {
                                result = dateField.DateTime.ToShortDateString();
                                return true;
                            }

                            if (binderProperty == "LongDate")
                            {
                                result = dateField.DateTime.ToLongDateString();
                                return true;
                            }

                        }
                    }
                    #endregion

                    #endregion

                }
            }

            result = null;
            return false;

        }
    }
}