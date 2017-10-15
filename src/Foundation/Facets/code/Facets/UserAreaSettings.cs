using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    [Serializable]
    public class UserAreaSettings : Facet, IUserAreaSettings, IFacet, IElement, IValidatable
    {
        public UserAreaSettings()
        {
            EnsureDictionary<IUserSettingValue>(FieldNames.Entries);
        }

        public IElementDictionary<IUserSettingValue> Entries
        {
            get
            {
                return GetDictionary<IUserSettingValue>(FieldNames.Entries);
            }
        }
    }
}