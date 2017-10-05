using Sitecore.Analytics;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.API
{
    public static class ServicesApiControllerExtensions
    {
        public static ITracker GetTracker(this ServicesApiController controller, bool trackRequest = false)
        {
            if (IsContextInvalid())
            {
                Tracker.StartTracking();
                if (IsContextInvalid())
                {
                    throw new ArgumentException("Context is invalid");
                }

                if (!trackRequest)
                {
                    Tracker.Current.CurrentPage.Cancel();
                }
            }

            return Tracker.Current;
        }

        private static bool IsContextInvalid()
        {
            return
                Tracker.Current == null ||
                Tracker.Current.Session == null ||
                Tracker.Current.Interaction == null ||
                !Tracker.IsActive ||
                !Tracker.Enabled;
        }
    }
}