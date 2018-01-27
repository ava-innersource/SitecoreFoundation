using Sitecore.XA.Foundation.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Handlebars.Models
{
    public class HandlebarQueryContainerModel : RenderingModelBase
    {
        public bool EnablePagination { get; set; }

        public int ItemsPerPage { get; set; }

        public string QueryStringParam { get; set; }

        public int CurrentPage { get; set; }

        public int NumPages { get; set; }

        public string CurrentUrl { get; set; }

        public int NumItems { get; set; }
    }
}