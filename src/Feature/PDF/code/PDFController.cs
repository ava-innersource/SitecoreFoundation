using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using SF.Foundation.Configuration;

namespace SF.Feature.PDF
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class PDFController : Controller
    {
        
        public ActionResult Generate(string id)
        {
            Guid requestedGuid = Guid.Empty;
            if (!Guid.TryParse(id, out requestedGuid))
            {
                return Content("Guid Not Formatted properly.");
            }

            var item = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(id));
            if (item == null)
            {
                return Content("Invalid ID");
            }

            var itemUrl = item.GetFullyQualifiedUrl();

            var result = new UrlAsPdf(itemUrl);

            //Set Options
            if (!string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                result.FileName = item.Name + ".pdf";
            }

            if (!string.IsNullOrEmpty(Request.QueryString["landscape"]))
            { 
                result.PageOrientation = Rotativa.Options.Orientation.Landscape;
            }
            
            string requestedSize = Request.QueryString["size"];
            if (!string.IsNullOrEmpty(requestedSize))
            {
                Rotativa.Options.Size size = Rotativa.Options.Size.Letter;
                if (Enum.TryParse<Rotativa.Options.Size>(requestedSize, out size))
                {
                    result.PageSize = size;
                }
            }

            string pageWidth = Request.QueryString["width"];
            if (!string.IsNullOrEmpty(pageWidth))
            {
                double width = 0;
                if (double.TryParse(pageWidth, out width))
                {
                    result.PageWidth = width;
                }                
            }
            string pageHeight = Request.QueryString["height"];
            if (!string.IsNullOrEmpty(pageHeight))
            {
                double height = 0;
                if (double.TryParse(pageHeight, out height))
                {
                    result.PageHeight = height;
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString["nomargin"]))
            {
                result.PageMargins.Left = 0;
                result.PageMargins.Right = 0;
                result.PageMargins.Top = 0;
                result.PageMargins.Bottom = 0;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["passauth"]))
            {
                //pass any existing cookies and auth tokens
                result.Cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);
                result.FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;                
            }

            string disableSmartShrinking = Request.QueryString["dss"];
            if (!string.IsNullOrEmpty(disableSmartShrinking))
            {
                result.CustomSwitches = "--load-error-handling ignore";
                result.CustomSwitches += " --disable-smart-shrinking";
            }

            string saveLocal = Request.QueryString["sl"];
            if (!string.IsNullOrEmpty(saveLocal))
            {
                result.SaveOnServerPath = @"C:\inetpub\wwwroot\rms\Website\App_Data\tempPDFs";
            }

            //var binary = result.BuildPdf(this.ControllerContext);
            // return File(binary, "application/pdf", result.FileName);
            return result;
        }
    }
}
