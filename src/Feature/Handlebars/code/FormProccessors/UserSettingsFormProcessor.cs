using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using SF.Foundation.Facets;

namespace SF.Feature.Handlebars
{
    public class UserSettingsFormProcessor : IFormProcessor
    {
        public void Process(Item processorItem, Item formConfiguration, HttpRequestBase request)
        {
            var area = processorItem.Fields["Settings Area"].Value;
            var fields = (NameValueListField)processorItem.Fields["Fields"];

            foreach(var formKey in fields.NameValues.Keys)
            {
                var userSettingKey = fields.NameValues[formKey.ToString()];
                if (!string.IsNullOrEmpty(userSettingKey))
                {
                    var formValue = request.Form[formKey.ToString()];
                    if (formValue != null)
                    {
                        SF.Foundation.Facets.Facades.UserSettings.Settings[userSettingKey, area] = formValue;
                    }
                }
            }
        }
    }
}