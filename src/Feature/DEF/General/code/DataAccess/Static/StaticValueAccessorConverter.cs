using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.General
{

    public class StaticValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{C559AA17-221A-4374-A6B6-58B5DDE38364}");
        public StaticValueAccessorConverter(IItemModelRepository repository)
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
            return new StaticValueReader(base.GetStringValue(source, StaticValueItemModel.Value));
        }

        protected override IValueWriter GetValueWriter(ItemModel source)
        {
            IValueWriter valueWriter = base.GetValueWriter(source);
            if (valueWriter != null)
            {
                return valueWriter;
            }
            return new StaticValueWriter(base.GetStringValue(source, StaticValueItemModel.Value));
        }
    }
}
