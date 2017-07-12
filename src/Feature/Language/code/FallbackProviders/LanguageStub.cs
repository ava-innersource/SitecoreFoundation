using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Feature.Language
{
    public class LanguageStub : Item
    {
        public LanguageStub(ID itemID, ItemData data, Database database)
            : base(itemID, data, database)
        {
        }

        private Sitecore.Globalization.Language originalLanguage;

        public Sitecore.Globalization.Language OriginalLanguage
        {
            get
            {
                return originalLanguage ?? Language;
            }
            set
            {
                originalLanguage = value;
            }
        }
    }


}
