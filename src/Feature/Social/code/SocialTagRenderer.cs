
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SF.Feature.Social
{
    /// <summary>
    /// Web Forms Control to Render Social Tags
    /// </summary>
    [ToolboxData("<{0}:SocialTagRenderer runat=server></{0}:SocialTagRenderer>")]
    public class SocialTagRenderer : WebControl
    {
        
        protected override void RenderContents(HtmlTextWriter output)
        {
            SocialTagManager manager = new SocialTagManager();
            output.Write(manager.GetSocialTags());
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
