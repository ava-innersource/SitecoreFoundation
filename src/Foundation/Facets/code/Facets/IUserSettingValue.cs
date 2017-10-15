using Sitecore.Analytics.Model.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Facets
{
    public interface IUserSettingValue : IElement, IValidatable
    {
        string Value { get; set; }
    }
}