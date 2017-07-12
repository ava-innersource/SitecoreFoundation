using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Text;
using System.Web.Mvc;

namespace SF.Foundation.Container
{

    public static class ContainerHelper
    {
        public static IDisposable ComponentContainer(this HtmlHelper htmlHelper, string componentClassName = "", string containerTag = "div")
        {
            htmlHelper.ViewContext.Writer.Write(htmlHelper.RenderContainerStart(componentClassName, containerTag));
            return new DisposableHelper(htmlHelper, containerTag);
        }


    public static string RenderContainerStart(this HtmlHelper helper, string componentClassName = "", string containerTag = "div")
        {
            var attributes = helper.GetContainerTagAttributes(componentClassName).ToHtmlString();
            return string.Format("<{0} {1}>", containerTag, attributes);
        }

        public static MvcHtmlString GetContainerTagAttributes(this HtmlHelper helper, string componentClassName)
        {
            var item = helper.GetComponentItem();

            var sb = new StringBuilder();

            var rc = RenderingContext.CurrentOrNull;
            if (rc != null)
            {
                var renderingId = rc.Rendering.Parameters["ID"];
                if (!string.IsNullOrEmpty(renderingId))
                {
                    sb.Append(@" id=""").Append(renderingId).Append(@"""");
                }
                else
                {
                    if (item != null && item.Fields["CssId"] != null && !string.IsNullOrEmpty(item.Fields["CssId"].Value))
                    {
                        sb.Append(@" id=""").Append(item.Fields["CssId"].Value).Append(@"""");
                    }
                }

                var renderingClass = rc.Rendering.Parameters["class"];
                if (!string.IsNullOrEmpty(renderingClass))
                {
                    componentClassName += " " + renderingClass;
                }
            }

            var visibilityClasses = helper.GetVisibilityParameterClasses();
            if (visibilityClasses != string.Empty)
            {
                componentClassName += " " + visibilityClasses;
            }

            if (!string.IsNullOrEmpty(componentClassName) || (item.Fields["CssClass"] != null && !string.IsNullOrEmpty(item.Fields["CssClass"].Value)))
            {
                sb.Append(@" class=""").Append(componentClassName);
                if (item.Fields["CssClass"] != null && !string.IsNullOrEmpty(item.Fields["CssClass"].Value))
                {
                    sb.Append(" ");
                    sb.Append(item.Fields["CssClass"].Value);
                }

                sb.Append(@"""");
            }

            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                if (rc != null && rc.Rendering != null)
                {
                    var renderingTitle = rc.Rendering.RenderingItem.DisplayName;
                    sb.Append(string.Format(@" data-widget-title=""{0}""", renderingTitle));
                }
            }

            return new MvcHtmlString(sb.ToString());
        }


        public static Item GetComponentItem(this HtmlHelper helper)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc != null && rc.Rendering != null)
            {
                if (!string.IsNullOrEmpty(rc.Rendering.DataSource))
                {
                    var item = Sitecore.Context.Database.GetItem(rc.Rendering.DataSource);
                    if (item != null)
                    {
                        return item;
                    }
                }
            }
            return Sitecore.Context.Item;
        }

    }
}
