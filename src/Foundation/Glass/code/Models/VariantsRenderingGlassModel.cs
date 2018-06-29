using Sitecore.XA.Foundation.Mvc.Models;
using Sitecore.XA.Foundation.Variants.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.GlassBootstrap.Models
{
    public class VariantsRenderingGlassModel<T> : VariantsRenderingModel where T : class
    {
        public T GlassModel { get; set; }

    }
}