<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reset.aspx.cs" Inherits="Moriyama.Umbraco.Password.umbraco.Plugins.Moriyama.Password.Reset" %>
<%@ Register TagPrefix="cc1" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register Namespace="umbraco" TagPrefix="umb" Assembly="umbraco" %>
<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        Reset your password
    </title>
    <cc1:UmbracoClientDependencyLoader runat="server" ID="ClientLoader" />
    <umb:CssInclude ID="CssInclude1" runat="server" FilePath="ui/default.css" PathNameAlias="UmbracoClient" />
    <umb:JsInclude ID="JsInclude1" runat="server" FilePath="ui/default.js" PathNameAlias="UmbracoClient" />
    <umb:JsInclude ID="JsInclude3" runat="server" FilePath="ui/jquery.js" PathNameAlias="UmbracoClient"
        Priority="0" />
    <umb:JsInclude ID="JsInclude2" runat="server" FilePath="ui/jqueryui.js" PathNameAlias="UmbracoClient" />
    <style type="text/css">
        label { width: 100px;display: inline-block; }
        #Submit { margin-top: 5px;}
    </style>

</head>
<body>
    <h1>Reset your password</h1>
    
    <asp:Literal ID="Confirm" runat="server" Visible="False"/>


    <form id="form1" runat="server">
        
        <div>
            <label for="Login">Login:</label>
            <asp:TextBox ID="Login" runat="server"></asp:TextBox>
        </div>
        
        <asp:Button ID="Submit" runat="server" Text="Request Password Reset" OnClick="Submit_Click" />

    </form>
</body>
</html>
