using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SF.Feature.MetaTags
{
    /// <summary>
    /// This Web Forms control renders meta tags based on global prefix and suffix settings
    /// and item settings.
    /// </summary>
    [ToolboxData("<{0}:MetaTagRenderer runat=server></{0}:MetaTagRenderer>")]
    public class MetaTagRenderer : WebControl
    {
        
        protected override void RenderContents(HtmlTextWriter output)
        {
            MetaTagManager manager = new MetaTagManager();
            output.Write(manager.GetMetaTags());
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            //Don't render span tag.
            //base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            //Don't render span tag.
            //base.RenderEndTag(writer);
        }
    }
}
