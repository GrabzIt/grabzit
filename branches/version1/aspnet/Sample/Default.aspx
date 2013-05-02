<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GrabzIt Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>GrabzIt Demo</h1>
    <p>Enter the URL of the website you want to take a screenshot of. Then resulting screenshot should be saved in the <a href="screenshots/">screenshots directory</a>. It may take a few seconds for it to appear!</p>
    <p>If nothing is happening check the <a href="http://grabz.it/account/diagnostics">diagnostics panel</a> to see if there is an error.</p>
    <strong>URL</strong> <asp:TextBox ID="txtURL" runat="server"></asp:TextBox>
    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Grabz"/>
    <br />
    <h2>Completed Screenshots</h2>
    <iframe id="iFImages" src="Images.aspx" width="100%" height="500px;" style="border:0;overflow-y:auto;"></iframe>
    </div>
    </form>
    <script type="text/javascript">
            function reloadIFrame() {
                var iframe = document.getElementById('iFImages');
                iframe.src = iframe.src;
                setTimeout("reloadIFrame()", 5000);
            }

            reloadIFrame();
    </script>
</body>
</html>
