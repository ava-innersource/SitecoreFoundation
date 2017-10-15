using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    public interface IUserAreaSettings : IFacet, IElement, IValidatable
    {
        IElementDictionary<IUserSettingValue> Entries { get; }
    }
}