using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    [Serializable]
    public class UserSettings : Facet, IUserSettings, IFacet, IElement, IValidatable
    {
        public UserSettings()
        {
            EnsureDictionary<IUserAreaSettings>(FieldNames.Entries);
        }

        public IElementDictionary<IUserAreaSettings> Entries
        {
            get
            {
                return GetDictionary<IUserAreaSettings>(FieldNames.Entries);
            }
        }
    }
}