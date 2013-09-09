<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Moriyama.Umbraco.Password.umbraco.Plugins.Moriyama.Password.Confirm" %>
<%@ Register TagPrefix="cc1" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register Namespace="umbraco" TagPrefix="umb" Assembly="umbraco" %>
<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset your password</title>
        <cc1:UmbracoClientDependencyLoader runat="server" ID="ClientLoader" />
    <umb:CssInclude ID="CssInclude1" runat="server" FilePath="ui/default.css" PathNameAlias="UmbracoClient" />
    <umb:JsInclude ID="JsInclude1" runat="server" FilePath="ui/default.js" PathNameAlias="UmbracoClient" />
    <umb:JsInclude ID="JsInclude3" runat="server" FilePath="ui/jquery.js" PathNameAlias="UmbracoClient"
        Priority="0" />
    <umb:JsInclude ID="JsInclude2" runat="server" FilePath="ui/jqueryui.js" PathNameAlias="UmbracoClient" />
    <style type="text/css">
        label { width: 150px;display: inline-block; }
        #Submit { margin-top: 5px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="Panel1" runat="server" Visible="False">
            
            <h1>Reset your password</h1>
            
            <asp:Literal ID="ErrorLiteral" runat="server" Visible="False"></asp:Literal>

            <div>
                <label for="Password">New Password:</label>
                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
             <div>
                <label for="ConfirmPassword">Confirm Password:</label>
                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <asp:Button ID="Submit" runat="server" Text="Reset your password" OnClick="Submit_Click" />
            
        </asp:Panel>
        <asp:Panel ID="ResetExpiredPanel" runat="server" Visible="false">
             <h1>Reset Link Expired</h1>
            <p>This link to reset your password has expired, please <a href="reset.aspx">generate a new reset link to try again</a></p>


        </asp:Panel>
        
    </div>
    </form>
    
</body>
</html>
