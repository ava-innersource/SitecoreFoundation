using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Feature.Analytics
{
    public class TrackerDetails
    {
        public TrackerDetails()
        {
            this.events = new List<EventDetails>();
        }

        public string contactId { get; set; }

        public string userName { get; set; }
        public string name { get; set; }

        public string email { get; set; }

        public List<EventDetails> events { get; set; }
    }
}