using SF.Foundation.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SF.Foundation.Multisite
{
    public partial class ErrorHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Sitecore.Context.Site != null)
                {
                    var settings = Sitecore.Context.Site.GetSiteSettings<MultisitePageSettings>();
                    if (settings != null)
                    {
                        if (!string.IsNullOrEmpty(settings.ErrorPage))
                        {
                            Response.Redirect(settings.ErrorPage);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                //Can Try to log it
                Sitecore.Diagnostics.Log.Error("Could not render error page", ex, this);
            }
        }
    }
}