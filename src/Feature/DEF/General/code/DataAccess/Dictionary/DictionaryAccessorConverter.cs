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

        protected override IValueReader GetValueReader(ItemModel source)
        {
            IValueReader valueReader = base.GetValueReader(source);
            if (valueReader != null)
            {
                return valueReader;
            }
            return new DictionaryValueReader(base.GetStringValue(source, DictionaryAccessorItemModel.Key));
        }

        protected override IValueWriter GetValueWriter(ItemModel source)
        {
            IValueWriter valueWriter = base.GetValueWriter(source);
            if (valueWriter != null)
            {
                return valueWriter;
            }
            return new DictionaryValueWriter(base.GetStringValue(source, DictionaryAccessorItemModel.Key));
        }

    }
}
