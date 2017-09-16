<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorHandler.aspx.cs" Inherits="SF.Foundation.Multisite.ErrorHandler" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Error Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Configure Your 500 level errors to come to this page and it will redirect to any site configured error page.<br />
   <br />
    Design Your Default Error Page Here. <br />
    <br />
    It will display if the Sitecore Context can not be loaded and will be used for all sites on the instance.
        <br />
    </div>
    </form>
</body>
</html>
