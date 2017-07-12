using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.Pipelines;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.Container
{
    public class ContainerClassPipelineArgs : PipelineArgs
    {
        public ContainerClassPipelineArgs()
        {
            this.CssClasses = new List<string>();
        }

        public List<string> CssClasses { get; set; }
        public Template RenderingParametersTemplate 
        {
            get
            {
                return Sitecore.Data.Managers.TemplateManager.GetTemplate(new Sitecore.Data.ID(RenderingContext.Rendering.RenderingItem.InnerItem["Parameters Template"]), Sitecore.Context.Database);
            }
        }

        public bool GetCheckboxRenderingParameterValue(string parameterName)
        {
            if (RenderingContext == null || RenderingContext.Rendering == null || RenderingParameters == null) return false;
            return RenderingParameters[parameterName] == "1";
        }

        public Sitecore.Mvc.Presentation.RenderingContext RenderingContext
        {
            get
            {
                return Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull;
            }
        }

        private NameValueCollection renderingParameters = null;

        public NameValueCollection RenderingParameters
        {
            get
            {
                if (RenderingContext == null || RenderingContext.Rendering == null) return null;

                if (renderingParameters == null)
                {
                    var parametersAsString = RenderingContext.Rendering.Properties["Parameters"];
                    renderingParameters = HttpUtility.ParseQueryString(parametersAsString);
                }

                return renderingParameters;
            }
        }
    }
}
