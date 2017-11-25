using Sitecore.Analytics;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    public class ContactFacetHasValueCondition<T> : WhenCondition<T> where T : RuleContext
    {
        public string FacetValue { get; set; }

        public string FacetPath { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Contact contact = Tracker.Current.Session.Contact;

            if (contact == null)
            {
                Log.Info(this.GetType() + ": contact is null", this);
                return false;
            }

            if (string.IsNullOrEmpty(FacetPath))
            {
                Log.Info(this.GetType() + ": facet path is empty", this);
                return false;
            }

            var inputPropertyToFind = FacetPath;

            string[] propertyPathArr = inputPropertyToFind.Split('.');
            if (propertyPathArr.Length == 0)
            {
                Log.Info(this.GetType() + ": facet path is empty", this);
                return false;
            }

            Queue propertyQueue = new Queue(propertyPathArr);
            string facetName = propertyQueue.Dequeue().ToString();
            IFacet facet = contact.Facets[facetName];
            if (facet == null)
            {
                Log.Info(string.Format("{0} : cannot find facet {1}", this.GetType(), facetName), this);
                return false;
            }

            var datalist = facet.Members[propertyQueue.Dequeue().ToString()];
            if (datalist == null)
            {
                Log.Info(string.Format("{0} : cannot find facet {1}", this.GetType(), facetName), this);
                return false;
            }

            if (typeof(IModelAttributeMember).IsInstanceOfType(datalist))
            {
                var propValue = ((IModelAttributeMember)datalist).Value;
                return (propValue != null ? propValue.Equals(FacetValue) : false);
            }
            if (typeof(IModelDictionaryMember).IsInstanceOfType(datalist))
            {
                var dictionaryMember = (IModelDictionaryMember)datalist;

                string elementName = propertyQueue.Dequeue().ToString();
                IElement element = dictionaryMember.Elements[elementName];
                if (element == null)
                {
                    Log.Info(string.Format("{0} : cannot find element {1}", this.GetType(), elementName), this);
                    return false;
                }

                string propertyToFind = propertyQueue.Dequeue().ToString();
                var prop = element.Members[propertyToFind];
                if (prop == null)
                {
                    Log.Info(string.Format("{0} : cannot find property {1}", this.GetType(), propertyToFind), this);
                    return false;
                }

                var propValue = ((IModelAttributeMember)prop).Value;
                return (propValue != null ? propValue.Equals(FacetValue) : false);
            }
            if (typeof(IModelCollectionMember).IsInstanceOfType(datalist))
            {
                var collectionMember = (IModelCollectionMember)datalist;
                var propertyToFind = propertyQueue.Dequeue().ToString();
                for (int i = 0; i < collectionMember.Elements.Count; i++)
                {
                    IElement element = collectionMember.Elements[i];
                    var prop = element.Members[propertyToFind];
                    if (prop == null)
                    {
                        Log.Info(string.Format("{0} : cannot find property {1}", this.GetType(), propertyToFind), this);
                        return false;
                    }
                    var propValue = ((IModelAttributeMember)prop).Value;
                    if (propValue.Equals(FacetValue))
                        return true;
                }
            }

            return false;
        }
    }
}