using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.DEF.General
{

    public class DictionaryAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{F36DF0C3-0E09-4015-928E-BDCA9B5BF2BA}");
        public DictionaryAccessorConverter(IItemModelRepository repository)
            : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        public override IValueAccessor Convert(ItemModel source)
        {
            var accessor = base.Convert(source);
            if (accessor == null)
            {
                return null;
            }
            var key = base.GetStringValue(source, DictionaryAccessorItemModel.Key);
            
            //unless a reader or writer is explicitly set use the property value
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new DictionaryValueReader(key);
            }
            if (accessor.ValueWriter == null)
            {
                accessor.ValueWriter = new DictionaryValueWriter(key);
            }
            return accessor;
        }

    }
}
