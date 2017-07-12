using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SF.Feature.Redirection
{
    public class GoalRedirectController : Controller
    {
        public ActionResult Redirect(string goal, string u)
        {
            
            var url = System.Web.HttpUtility.UrlDecode(u);

            try
            {

                if (Tracker.Current == null)
                {
                    Tracker.Initialize();
                }

                var goalItem = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(goal));

                Assert.IsNotNull(Tracker.Current, "Tracker.Current");
                Assert.IsNotNull(Tracker.Current.Session, "Tracker.Current.Session");
                var interaction = Tracker.Current.Session.Interaction;
                Assert.IsNotNull(interaction, "Tracker.Current.Session.Interaction");
                Assert.IsNotNull(interaction.CurrentPage, "Tracker.Current.Session.Interaction.CurrentPage");

                var pageEventData = new PageEventData(goalItem.Name, goalItem.ID.ToGuid())
                {
                    Data = url,
                    Text = "Goal Set By Redirection Rule"
                };

                interaction.CurrentPage.Register(pageEventData);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error when tracking goal for redirect.", ex, this);
            }

            return Redirect(url);
        }
    }
}
