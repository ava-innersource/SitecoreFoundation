using Sitecore.XA.Foundation.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Models
{
    public class HandlebarTemplateModel : RenderingModelBase
    {
        public bool HasTemplateContent { get; set; }
    }
}