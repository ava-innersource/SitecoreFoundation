using System;
using System.Web.Mvc;

namespace SF.Foundation.Container
{
  public class DisposableHelper : IDisposable
  {
    private HtmlHelper helper;
    private string containerTag;

    // When the object is created, write "begin" function
    public DisposableHelper(HtmlHelper helper)
    {
      this.helper = helper;
      this.containerTag = "div";
    }

    public DisposableHelper(HtmlHelper helper, string containerTag)
    {
      this.helper = helper;
      this.containerTag = containerTag;
    }

    // When the object is disposed (end of using block), write "end" function
    public void Dispose()
    {
      helper.ViewContext.Writer.Write("</" + containerTag + "> ");
    }
  }
}
