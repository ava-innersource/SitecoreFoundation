using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace SF.Feature.LoginSample
{
    public static class ControllerExtensions
    {
        public static Item GetItem(this System.Web.Mvc.Controller controller)
        {
            var rc = RenderingContext.CurrentOrNull;
            if (rc != null && rc.Rendering != null)
            {
                if (!string.IsNullOrEmpty(rc.Rendering.DataSource))
                {
                    return Sitecore.Context.Database.GetItem(rc.Rendering.DataSource);
                }
            }
            return Sitecore.Context.Item;
        }


    }
}
