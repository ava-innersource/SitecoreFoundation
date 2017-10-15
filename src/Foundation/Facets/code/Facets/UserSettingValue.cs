using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    [Serializable]
    public class UserSettingValue : Element, IUserSettingValue, IElement, IValidatable
    {
        public UserSettingValue()
        {
            EnsureAttribute<string>(FieldNames.Value);
        }

        public string Value
        {
            get { return GetAttribute<string>(FieldNames.Value); }
            set { SetAttribute(FieldNames.Value, value); }
        }
    }
}