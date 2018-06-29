using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
using SF.Foundation.GlassBootstrap.Models;
using Sitecore.XA.Foundation.Mvc.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.GlassBootstrap.Repositories
{
    public class GlassModelRepository<T> : ModelRepository where T : class
    {
        public ISitecoreContext SitecoreContext { get; set; }

        public GlassModelRepository(ISitecoreContext context)
        {
            SitecoreContext = context;
        }

        public override IRenderingModelBase GetModel()
        {
            var model = new RenderingGlassModel<T>();
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