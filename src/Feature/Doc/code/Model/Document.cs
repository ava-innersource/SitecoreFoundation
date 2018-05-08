using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Doc.Model
{
    public class Document
    {
        public Document()
        {
            Sections = new List<Section>();
        }

        public List<Section> Sections { get; set; }
    }
}