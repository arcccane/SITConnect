<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LctjlMdAAAAALvO9sUcOvgVa8S240iApRCsFQmQ"></script>
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
                <legend>Account Login</legend>
                <br />
            <table class="style1">
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Email address"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_login_email" TextMode="Email" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email format" ControlToValidate="tb_login_email" ForeColor="Red" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Enter email" ControlToValidate="tb_login_email" Display="None" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_login_pwd" TextMode="Password" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter password" Display="None" ControlToValidate="tb_login_pwd" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style6"></td>
                    <td class="style2">
                        <asp:Button ID="btn_Reg" runat="server" Height="48px"
                            OnClick="btn_Log_Click" Text="Login" Width="288px" />
                        <br />
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Blue" NavigateUrl="~/Registration.aspx">Register</asp:HyperLink>
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="HyperLink1" runat="server" ForeColor="Blue" NavigateUrl="~/ChangePassword.aspx">Change Password</asp:HyperLink>
                    </td>
                </tr>
            </table>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <br />
                <asp:Label ID="lb_error" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <asp:Label ID="lb_error2" runat="server" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" />
                <br />
            </fieldset>
        </div>
    </form>
</body>
</html>

<script>
    grecaptcha.ready(function () {
        grecaptcha.execute('6LctjlMdAAAAALvO9sUcOvgVa8S240iApRCsFQmQ', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
        });
    });
</script>