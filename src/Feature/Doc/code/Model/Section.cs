using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Model
{
    public class Section
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public bool IncludeInTOC { get; set; }
    }
}