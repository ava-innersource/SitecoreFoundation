using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.DEF.Feature.SitecoreProvider
{

    public class StaticValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{C559AA17-221A-4374-A6B6-58B5DDE38364}");
        public StaticValueAccessorConverter(IItemModelRepository repository)
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
            var value = base.GetStringValue(source, StaticValueItemModel.Value);
            
            //unless a reader or writer is explicitly set use the property value
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new StaticValueReader(value);
            }
            if (accessor.ValueWriter == null)
            {
                accessor.ValueWriter = new StaticValueWriter(value);
            }
            return accessor;
        }

    }
}
