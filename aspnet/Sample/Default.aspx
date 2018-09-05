<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sample._Default" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GrabzIt Demo</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <link rel="stylesheet" type="text/css" href="css/style.css">
    <script src="ajax/jquery.min.js"></script>
    <script src="ajax/ui.js"></script>
    <script>
        var ui = new UI('ajax/results.ashx?r=', 'css');
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>GrabzIt Demo</h1>
    <p><span id="spnIntro">Enter the HTML or URL you want to convert into a DOCX, PDF or Image. The resulting capture</span> should then be saved in the <a href="results/" target="_blank">results directory</a>. It may take a few seconds for it to appear! If nothing is happening check the <a href="https://grabz.it/account/diagnostics" target="_blank">diagnostics panel</a> to see if there is an error.</p>
    <asp:Panel ID="pnlCallbackWarning" Visible="false" runat="server"><p>As you are using this demo application on your local machine it will create captures synchronously, which will cause the web page to freeze while captures are generated. <u>Please wait for the capture to complete</u>.</p></asp:Panel>
    <p><asp:Label ID="lblMessage" runat="server" /></p>
<div class="Row" id="divConvert">
<label>Convert </label>
    <asp:DropDownList ID="ddlConvert" runat="server" onchange="ui.selectConvertChanged(this)"><asp:ListItem Text="URL" Value="url"></asp:ListItem><asp:ListItem Text="HTML" Value="html"></asp:ListItem></asp:DropDownList>
</div>
<div id="divHTML" class="Row hidden">
<label>HTML </label><asp:TextBox ID="txtHTML" runat="server" TextMode="MultiLine"><html><body><h1>Hello world!</h1></body></html></asp:TextBox>
</div>
<div id="divURL" class="Row">
<label>URL </label><asp:TextBox ID="txtURL" runat="server" placeholder="http://www.example.com"/>
</div>
<div class="Row">
<label>Format </label><asp:DropDownList ID="ddlFormat" runat="server" onchange="ui.selectChanged(this)"><asp:ListItem Text="JPG" Value="jpg"></asp:ListItem><asp:ListItem Text="PDF" Value="pdf"></asp:ListItem><asp:ListItem Text="DOCX" Value="docx"></asp:ListItem><asp:ListItem Text="GIF" Value="gif"></asp:ListItem><asp:ListItem Text="CSV" Value="csv"></asp:ListItem></asp:DropDownList>
</div>       
    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Grabz It"/><asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Clear Results"/>
    <br />
    <h2>Completed Screenshots</h2>
    <div id="divResults"></div>
    </div>
    </form>
</body>
</html>
