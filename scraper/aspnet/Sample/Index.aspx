<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Sample.Index" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblError" runat="server" style="color:red;font-weight:bold"></asp:Label><asp:Label ID="lblMessage" runat="server" style="color:green;font-weight:bold"></asp:Label>
        <asp:GridView ID="grdScrapes" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Scrape Name">
                    <ItemTemplate>
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Scrape Status">
                    <ItemTemplate>
                        <%# Eval("Status") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                    <asp:Button id="btnStart" runat="server" OnClick="btnStart_Click" Text="Start" CommandArgument='<%# Eval("ID") %>'/>
                    <asp:Button id="btnStop" runat="server" OnClick="btnStop_Click" Text="Stop" CommandArgument='<%# Eval("ID") %>'/>
                    <asp:Button id="btnEnable" runat="server" OnClick="btnEnable_Click" Text="Enable" CommandArgument='<%# Eval("ID") %>'/>
                    <asp:Button id="btnDisable" runat="server" OnClick="btnDisable_Click" Text="Disable" CommandArgument='<%# Eval("ID") %>'/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
