using Glass.Mapper;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.IoC;
using Glass.Mapper.Sc.Web;
using Glass.Mapper.Sc.Web.Mvc;
using SF.Foundation.Components.Models;
using SF.Foundation.Components.Repositories;
using Sitecore.Data.Items;
using Sitecore.XA.Foundation.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF.Foundation.Components.Controllers
{
    public class GlassStandardController<T> : GlassStandardController where T : class
    {
        private T dataSourceItem;
        private T contextItem;
        private GlassModelRepository<T> repository;

        public GlassStandardController() : this(GetContextFromFactory(), new RenderingContextMvcWrapper())
        {

        }

        public GlassStandardController(ISitecoreContext sitecoreContext, IRenderingContext renderingContextWrapper) :
            base(sitecoreContext, renderingContextWrapper)
        {
        }

        public T Layout
        {
            get { return DataSource ?? Context; }
        }

        public T DataSource
        {
            get { return dataSourceItem ?? (dataSourceItem = GetDataSourceItem<T>()); }
        }

        public T Context
        {
            get { return contextItem ?? (contextItem = GetContextItem<T>()); }
        }

        public GlassModelRepository<T> Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = new GlassModelRepository<T>(this.SitecoreContext);
                }
                return repository;
            } 
        }

        public RenderingGlassModel<T> Model
        {
            get
            {
                return Repository.GetModel() as RenderingGlassModel<T>;
            }
        }

        public T GlassModel
        {
            get
            {
                return Model.GlassModel;
            }
        }
    }

    public class GlassStandardController : StandardController
    {
        public ISitecoreContext SitecoreContext { get; set; }

        public IGlassHtml GlassHtml { get; set; }

        public IRenderingContext RenderingContextWrapper { get; set; }

        public GlassStandardController()
            : this(GetContextFromFactory(), new RenderingContextMvcWrapper())
        {

        }

        public GlassStandardController(
            ISitecoreContext sitecoreContext) : this(sitecoreContext, new RenderingContextMvcWrapper())
        {

        }
        public GlassStandardController(
            ISitecoreContext sitecoreContext,
            IRenderingContext renderingContextWrapper)
        {
            SitecoreContext = sitecoreContext;
            GlassHtml = sitecoreContext != null ? sitecoreContext.GlassHtml : null;
            RenderingContextWrapper = renderingContextWrapper;
        }

        /// <summary>
        /// Returns either the item specified by the DataSource or the current context item
        /// </summary>
        /// <value>The layout item.</value>
        public virtual Item LayoutItem
        {
            get
            {
                return DataSourceItem ?? ContextItem;
            }
        }

        /// <summary>
        /// Returns either the item specified by the current context item
        /// </summary>
        /// <value>The layout item.</value>
        public virtual Item ContextItem
        {
            get { return Sitecore.Context.Item; }
        }

        /// <summary>
        /// Returns the item specificed by the data source only. Returns null if no datasource set
        /// </summary>
        public virtual Item DataSourceItem
        {
            get
            {
                return RenderingContextWrapper.HasDataSource ? Sitecore.Context.Database.GetItem(RenderingContextWrapper.GetDataSource()) : null;
            }
        }

        /// <summary>
        /// Returns the Context Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetContextItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return SitecoreContext.GetCurrentItem<T>(isLazy, inferType);
        }

        /// <summary>
        /// Returns the Data Source Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetDataSourceItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            if (!RenderingContextWrapper.HasDataSource)
            {
                return null;
            }

            string dataSource = RenderingContextWrapper.GetDataSource();
            return !String.IsNullOrEmpty(dataSource) ? SitecoreContext.GetItem<T>(dataSource, isLazy, inferType) : null;
        }

        /// <summary>
        /// Returns the Layout Item as strongly typed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetLayoutItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return RenderingContextWrapper.HasDataSource
                ? GetDataSourceItem<T>(isLazy, inferType)
                : GetContextItem<T>(isLazy, inferType);
        }


        protected virtual T GetRenderingParameters<T>() where T : class
        {
            string renderingParameters = RenderingContextWrapper.GetRenderingParameters();
            return renderingParameters.HasValue() ? GlassHtml.GetRenderingParameters<T>(renderingParameters) : null;

        }

        /// <summary>
        /// Returns the data source item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isLazy"></param>
        /// <param name="inferType"></param>
        /// <returns></returns>
        [Obsolete("Use GetDataSourceItem")]
        protected virtual T GetRenderingItem<T>(bool isLazy = false, bool inferType = false) where T : class
        {
            return GetDataSourceItem<T>(isLazy, inferType);
        }


        protected static ISitecoreContext GetContextFromFactory()
        {
            try
            {
                return SitecoreContextFactory.Default.GetSitecoreContext();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Failed to create SitecoreContext", ex, typeof(GlassStandardController));
                return null;
            }
        }

    }
}