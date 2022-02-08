<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPassword.aspx.cs" Inherits="SITConnect.ForgetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="SITConnect"></asp:Label>
                <br />
                <br />
            </h2>
            <fieldset>
                <legend>Forget Password</legend>
                <table class="style1">
                <tr>
                    <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" UserNameInstructionText="Enter your Email to receive your password." UserNameLabelText="Email:" UserNameTitleText=""></asp:PasswordRecovery>
                </tr>
                </table>
            </fieldset>
            <br />
            
        </div>
    </form>
</body>
</html>
