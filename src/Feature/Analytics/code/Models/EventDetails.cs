using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Analytics
{
    public class EventDetails
    {
        public string name { get; set; }
        public string value { get; set; }
        public string text { get; set; }

        public string isGoal { get; set; }
    }
}