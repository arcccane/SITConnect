<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect Home</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>SITConnect Home</h2>
            <fieldset>
                <legend><asp:Label ID="lblMessage" runat="server" EnableViewState="false" /></legend>
                <h2>Account Details</h2>
                <asp:Image ID="photo" runat="server" Width="100" Height="100" />
                <h3>First Name:
                <asp:Label ID="lbl_firstname" runat="server"></asp:Label>
                </h3>
                <h3>Last Name:
                <asp:Label ID="lbl_lastname" runat="server"></asp:Label>
                </h3>
                <h3>Credit Card Info:
                <asp:Label ID="lbl_credit" runat="server"></asp:Label>
                </h3>
                <h3>Email:&nbsp;
            <asp:Label ID="lbl_email" runat="server"></asp:Label>
                </h3>
                <h3>Date of Birth:&nbsp;
            <asp:Label ID="lbl_date" runat="server"></asp:Label>
                </h3>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="Logout" Visible="false" />
            </fieldset>
        </div>
    </form>
</body>
</html>
