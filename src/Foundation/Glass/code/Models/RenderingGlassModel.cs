using Sitecore.XA.Foundation.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.GlassBootstrap.Models
{
    public class RenderingGlassModel<T> : RenderingModelBase where T : class
    {
        public T GlassModel { get; set; }

    }
}