using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DXF.Feature.RSS
{
    public class ArrayValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{186DA58E-733A-4066-85C4-601535B25AD8}");
        public ArrayValueAccessorConverter(IItemModelRepository repository)
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
            var property = base.GetStringValue(source, ArrayValueAccessorItemModel.Property);
            if (string.IsNullOrEmpty(property))
            {
                return null;
            }
            //
            
            //
            
            //unless a reader or writer is explicitly set use the property value
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new PropertyValueReader(property);
            }
            if (accessor.ValueWriter == null)
            {
                accessor.ValueWriter = new PropertyValueWriter(property);
            }

            return accessor;
        }

    }
}
