using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace SF.Feature.Doc.Pipeline
{
    public class DotNetProcessor : BaseDocProcessor
    {
        public override void ConfigureSection(Item item)
        {
            var strTypes = item.Fields["Dot Net Classes"].Value.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<Type> types = new List<Type>();
            foreach (var type in strTypes)
            {
                try
                {
                    var parsed = Type.GetType(type);
                    if (parsed != null)
                    {
                        types.Add(parsed);
                    }
                }
                catch(Exception ex)
                {
                    Sitecore.Diagnostics.Log.Warn("Skipping Invalid Type:" + type, ex, this);
                }
            }

            if (types.Count > 0)
            {
                this.IncludeInTOC = true;
                this.SectionName = "Dot Net Classes";
                this.SectionContent = RenderPartialView("/Views/Docs/DotNetClasses.cshtml", types);
            }
        }
    }
}