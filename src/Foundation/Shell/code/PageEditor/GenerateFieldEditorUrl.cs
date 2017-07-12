using Sitecore.Data;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Foundation.Shell
{
    /// <summary>
    /// This is needed to support Page Editor Buttons using Speak
    /// To edit meta data and other fields.
    /// </summary>
    public class GenerateFieldEditorUrl : PipelineProcessorRequest<ItemContext>
    {
        public string GenerateUrl()
        {
            var fieldList = CreateFieldDescriptors(RequestContext.Argument);
            var fieldeditorOption = new Sitecore.Shell.Applications.ContentManager.FieldEditorOptions(fieldList);
            //Save item when ok button is pressed
            fieldeditorOption.SaveItem = true;
            return fieldeditorOption.ToUrlString().ToString();
        }
        private List<FieldDescriptor> CreateFieldDescriptors(string fields)
        {
            var fieldList = new List<FieldDescriptor>();
            var fieldString = new ListString(fields);
            foreach (string field in new ListString(fieldString))
                fieldList.Add(new FieldDescriptor(RequestContext.Item, field));
            return fieldList;
        }
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            return new PipelineProcessorResponseValue
            {
                Value = GenerateUrl()
            };
        }
    }
}
