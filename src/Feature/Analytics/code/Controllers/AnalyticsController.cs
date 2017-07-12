using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Analytics.Outcome;
using Sitecore.Analytics.Outcome.Model;
using Sitecore.Data;
using Sitecore.Services.Infrastructure.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Sitecore.Analytics.Outcome.Extensions;
using Sitecore.Analytics.Tracking;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Analytics.Model.Framework;

namespace SF.Feature.Analytics
{
    public class PageEventDetails
    {
        public string name { get; set; }
        public string pageId { get; set; }

        public string data { get; set; }

        public string dataKey { get; set; }
        public string text { get; set; }

        public string pageEventDefinitionId { get; set; }

    }

    public class InteractionDetails
    {
        public string interactionId {get;set;}
    }

    public class OutcomeDetails
    {
        public string definitionId { get; set; }
        public string monetaryValue { get; set; }
    }

    public class PageDetails
    {
        public string pageId { get; set; }
    }

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

    public class EventDetails
    {
        public string name { get; set;}
        public string value { get; set; }
        public string text { get; set; }

        public string isGoal { get; set; }
    }

    public class AnalyticsController : ServicesApiController
    {
        [HttpGet]
        public string Index()
        {
            return "OK";
        }

        [HttpPost]
        public TrackerDetails GetTracker(PageDetails data)
        {
            if (IsContextInvalid())
            {
                Tracker.StartTracking();
                if (IsContextInvalid())
                {
                    Tracker.Current.CurrentPage.Cancel();
                    throw new ArgumentException("Context is invalid");
                }                
            }

            var pageId = Guid.Empty;
            Guid.TryParse(data.pageId, out pageId);

            var pageInteraction = Tracker.Current.Interaction.Pages.LastOrDefault(a => a.Item.Id == pageId);
            var page = pageInteraction != null ? Tracker.Current.Interaction.GetPage(pageInteraction.VisitPageIndex) : Tracker.Current.Interaction.PreviousPage;

            var details = new TrackerDetails();
            foreach (var pageEvent in page.PageEvents)
            {
                EventDetails eventDetails = new EventDetails();
                eventDetails.name = pageEvent.Name;
                eventDetails.text = pageEvent.Text;
                eventDetails.value = pageEvent.Value.ToString();
                eventDetails.isGoal = pageEvent.IsGoal ? "1" : "0";
                details.events.Add(eventDetails);
            }

            details.contactId = Tracker.Current.Contact.ContactId.ToString();
            details.userName = Tracker.Current.Contact.Identifiers.Identifier;

            try
            {
                Contact contact = Tracker.Current.Contact;
                IContactPersonalInfo personal = contact.GetFacet<IContactPersonalInfo>("Personal");
                details.name = string.Format("{0} {1} {2}", personal.FirstName, personal.MiddleName, personal.Surname);
                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not get Name", ex);
            }

            try
            {
                Contact contact = Tracker.Current.Contact;
                IContactEmailAddresses emails = contact.GetFacet<IContactEmailAddresses>("Personal");
                details.email = emails.Entries[emails.Preferred].SmtpAddress;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not get Preferred Email Address", ex);
            }

            Tracker.Current.CurrentPage.Cancel();

            return details;
        }

        [HttpPost]
        public string RegisterInteraction(InteractionDetails data)
        {
            if (IsContextInvalid())
            {
                Tracker.StartTracking();
                if (IsContextInvalid())
                {
                    return "Disabled";
                }
            }

            try
            {
                var id = data.interactionId;
                var item = Sitecore.Context.Database.GetItem(new ID(id));

                var url = item.Fields["Name"].Value;
                if (!url.StartsWith("/"))
                {
                    url = "/" + url;
                }

                Uri intUri = new Uri(System.Web.HttpContext.Current.Request.Url, url);

                Tracker.Current.CurrentPage.Url.Path = intUri.AbsolutePath;
                Tracker.Current.CurrentPage.Url.QueryString = intUri.Query;

                //Not showing up in path analyzer unless I set the ID
                Tracker.Current.CurrentPage.SetItemProperties(item.ID.Guid, item.Language.CultureInfo.DisplayName, item.Version.Number);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(string.Format("Error Tracking Interaction for {0}", data.interactionId), ex);
            }
            
            return "OK";
        }

        [HttpPost]
        public string RegisterEvent(PageEventDetails data)
        {
            if (IsContextInvalid())
            {
                Tracker.StartTracking();
                if (IsContextInvalid())
                {
                    return "Disabled";
                }
            }

            var pageId = Guid.Empty;
            Guid.TryParse(data.pageId, out pageId);

            var pageInteraction = Tracker.Current.Interaction.Pages.LastOrDefault(a => a.Item.Id == pageId);
            var page = pageInteraction != null ? Tracker.Current.Interaction.GetPage(pageInteraction.VisitPageIndex) : Tracker.Current.Interaction.PreviousPage;

            var pageEventData = new PageEventData(data.name);
            pageEventData.ItemId = page.Item.Id;
            pageEventData.Data = data.data;
            pageEventData.Text = data.text;
            pageEventData.DataKey = data.dataKey;
                       

            var peDefId = Guid.Empty;
            if (!string.IsNullOrEmpty(data.pageEventDefinitionId) && Guid.TryParse(data.pageEventDefinitionId, out peDefId))
            {
                pageEventData.PageEventDefinitionId = peDefId;
            }

            page.Register(pageEventData);

            //Do not track the API call
            Tracker.Current.CurrentPage.Cancel();

            return "OK";

        }

        [HttpGet]
        public string EndSession()
        {
            if (!IsContextInvalid())
            {
                Sitecore.Analytics.Tracker.Current.CurrentPage.Cancel();
            }
            System.Web.HttpContext.Current.Session.Abandon();
            return "OK";
        }

        [HttpPost]
        public string RegisterOutcome(OutcomeDetails data)
        {
            if (IsContextInvalid())
            {
                Tracker.StartTracking();
                if (IsContextInvalid())
                {
                    return "Disabled";
                }
            }

            var id = ID.NewID;
            var interactionId = ID.Parse(Tracker.Current.Interaction.InteractionId);
            var contactId = ID.Parse(Tracker.Current.Contact.ContactId);

            var definitionId = new ID(data.definitionId);

            decimal monetaryValue = 0;
            decimal.TryParse(data.monetaryValue, out monetaryValue);

            var outcome = new ContactOutcome(id, definitionId, contactId)
            {
                DateTime = DateTime.UtcNow.Date,
                MonetaryValue = monetaryValue,
                InteractionId = interactionId                 
            };

            var manager = Sitecore.Configuration.Factory.CreateObject("outcome/outcomeManager", true) as OutcomeManager;
            manager.Save(outcome);

            Tracker.Current.RegisterContactOutcome(outcome);

            return "OK";
        }


        //private Sitecore.Analytics.Core.Page GetPage(string pageId)
        //{
        //    return Tracker.Current.Interaction.Pages.Last<Sitecore.Analytics.Core.Page>(a=>a.Item.Id == new Guid(pageId));
        //}

        private bool IsContextInvalid()
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
