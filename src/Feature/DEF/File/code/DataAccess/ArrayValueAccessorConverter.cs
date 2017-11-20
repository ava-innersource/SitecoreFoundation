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

namespace SF.DEF.Feature.File
{
    public class ArrayValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{EB32248F-BA1E-430D-A739-FE1A556CD15D}");
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
            var position = base.GetIntValue(source, ArrayValueAccessorItemModel.Position);
            if (position <= 0)
            {
                return null;
            }
            //
            //array position is 1-based in the Sitecore item but is 0-based in the reader/writer
            position--;
            //
            //unless a reader or writer is explicitly set use the property value
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new ArrayValueReader(position);
            }
            if (accessor.ValueWriter == null)
            {
                accessor.ValueWriter = new ArrayValueWriter(position);
            }
            return accessor;
        }

    }
}
