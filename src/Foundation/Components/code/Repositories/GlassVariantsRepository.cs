using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
using SF.Foundation.Components.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using Sitecore.XA.Foundation.RenderingVariants.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Repositories
{
    public class GlassVariantsRepository<T> : VariantsRepository where T : class
    {
        public ISitecoreContext SitecoreContext { get; set; }

        public GlassVariantsRepository(ISitecoreContext context)
        {
            SitecoreContext = context;
        }

        public override IRenderingModelBase GetModel()
        {
            var model = new VariantsRenderingGlassModel<T>();
            FillBaseProperties(model);

            model.GlassModel = SitecoreContext.Cast<T>(model.Item);

            return model;
        }

        private static ISitecoreContext GetContextFromFactory()
        {
            try
            {
                return SitecoreContextFactory.Default.GetSitecoreContext();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to create SitecoreContext", ex, typeof(GlassModelRepository<T>));
                return null;
            }
        }
    }
}