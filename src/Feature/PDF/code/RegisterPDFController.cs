using Sitecore.Pipelines;
using System.Web.Routing;
using System.Web.Mvc;

namespace SF.Feature.PDF
{
    public class RegisterPDFController
    {
        public void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute("pdfRoute", "pdf/{id}", new 
                {
                    controller = "PDF",
                    action = "Generate"
                });
        }
       
    }
}
