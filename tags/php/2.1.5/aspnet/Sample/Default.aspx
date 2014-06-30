<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GrabzIt Demo</title>
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script src="ajax/ui.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>GrabzIt Demo</h1>
    <p>Enter the URL of the website you want to take a screenshot of. Then resulting screenshot should be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="http://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
    <p><asp:Label ID="lblMessage" runat="server" /></p>
    <label style="font-weight:bold;margin-right:1em;">URL </label><asp:TextBox ID="txtURL" runat="server"></asp:TextBox><asp:DropDownList ID="ddlFormat" runat="server"><asp:ListItem Text="JPG" Value="jpg"></asp:ListItem><asp:ListItem Text="PDF" Value="pdf"></asp:ListItem></asp:DropDownList>
    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Grabz It"/><asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Clear Results"/>
    <br />
    <h2>Completed Screenshots</h2>
    <div id="divResults"></div>
    </div>
    </form>
</body>
</html>
