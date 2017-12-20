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

namespace SF.DEF.Feature.General
{
    public class ArrayValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{EB32248F-BA1E-430D-A739-FE1A556CD15D}");
        public ArrayValueAccessorConverter(IItemModelRepository repository)
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
            return new ArrayValueReader(base.GetIntValue(source, ArrayValueAccessorItemModel.Position));
        }

        protected override IValueWriter GetValueWriter(ItemModel source)
        {
            IValueWriter valueWriter = base.GetValueWriter(source);
            if (valueWriter != null)
            {
                return valueWriter;
            }
            return new ArrayValueWriter(base.GetIntValue(source, ArrayValueAccessorItemModel.Position));
        }

       

    }
}
