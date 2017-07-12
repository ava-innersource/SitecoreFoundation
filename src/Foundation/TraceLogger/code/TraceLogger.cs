using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SF.Foundation.TraceLogger
{
    public class TraceLogger
    {
        private const string contextKey = "TraceLogger";

        #region ASP.Net Context Singleton Pattern

        // Note: Constructor is 'protected' 
        protected TraceLogger()
        {
            this.StartTimer();
        }
        private bool used = false;

        public static TraceLogger Current
        {
            get
            {
                TraceLogger instance = null;
                instance = HttpContext.Current.Items[contextKey] as TraceLogger;
                if (instance == null)
                {
                    instance = new TraceLogger();
                    HttpContext.Current.Items.Add(contextKey, instance);
                }
                return instance;
            }
        }

        #endregion

        private StringBuilder sb;

        public DateTime LastTime {get;set;}
        public DateTime StartTime {get;set;}
        

        public void StartTimer()
        {
            this.StartTime = DateTime.Now;
            this.LastTime = DateTime.Now;

            sb = new StringBuilder();
            sb.Append("Performance Data for: ");
            sb.Append(HttpContext.Current.Request.Url.ToString());
            sb.Append("\nTime\t\tSince First\tSince Last\tComments\n");
            sb.Append(this.StartTime.ToLongTimeString());
            sb.Append("\t0.0000000\t0.0000000\tTimer Initialized\n");
        }

        public double MilliSecondsSinceStart()
        {
            TimeSpan span = DateTime.Now.Subtract(this.StartTime);
            return span.TotalMilliseconds / 1000;
        }

        public double MilliSecondsSinceLast()
        {
            TimeSpan span = DateTime.Now.Subtract(this.LastTime);
            this.LastTime = DateTime.Now;
            return span.TotalMilliseconds / 1000;
        }

        public void Write(string message)
        {
            used = true;

            sb.Append(DateTime.Now.ToLongTimeString());
            sb.Append("\t");
            sb.Append(String.Format("{0:0.0000000}", MilliSecondsSinceStart()));
            sb.Append("\t");
            sb.Append(String.Format("{0:0.0000000}", MilliSecondsSinceLast()));
            sb.Append("\t");
            sb.Append(message);
            sb.Append("\n");
        }

        public void Flush()
        {
            if (used)
            {
                Sitecore.Diagnostics.Log.Info(sb.ToString(), this);
            }
        }
    }
}
