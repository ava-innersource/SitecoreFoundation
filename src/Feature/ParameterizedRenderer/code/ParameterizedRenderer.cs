using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SF.Foundation.Configuration;

namespace SF.Feature.ParameterizedRenderer
{
    public class ParameterizedRenderer
    {
        private Regex _regex = new Regex(@"(\{.*?})");        

        public void Process(Sitecore.Pipelines.RenderField.RenderFieldArgs args)
        {
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                return;
            }

            var dictionarySettings = Sitecore.Context.Site.GetSiteSettings<DictionarySiteSettings>();
            if (dictionarySettings == null)
            {
                return;
            }

            var dictionary = dictionarySettings.DictionaryDomain;

            args.Result.FirstPart = ReplacePlaceholders(args.Result.FirstPart, dictionary);
            args.Result.LastPart = ReplacePlaceholders(args.Result.LastPart, dictionary);
           
        }

        private string ReplacePlaceholders(string input, string domain)
        {
            var output = input;

            Dictionary<string,string> replacements = new Dictionary<string,string>();

            foreach (Match match in this._regex.Matches(output))
            {
                string param = match.Value;
                //ensure we aren't replacing the outer bracket if we need to
                param = param.Replace("{{", "{");

                if (!replacements.ContainsKey(param))
                {
                    string dictionaryEntry = param.Replace("{", "").Replace("}", "");
                    string dictionaryValue = string.Empty;
                    if (!string.IsNullOrEmpty(domain))
                    {
                        dictionaryValue = Sitecore.Globalization.Translate.TextByDomain(domain, dictionaryEntry);
                    }
                    else
                    {
                        dictionaryValue = Sitecore.Globalization.Translate.Text(dictionaryEntry);
                    }

                    replacements.Add(param, dictionaryValue);                    
                }
            }

            foreach (string key in replacements.Keys)
            {
                output = output.Replace(key, replacements[key]);
            }

            return output;  

        }
    }
}
