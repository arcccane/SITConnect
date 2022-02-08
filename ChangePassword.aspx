<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect Change Password</title>
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
                <legend>Change Password</legend>
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
                        <asp:Label ID="Label6" runat="server" Text="Old Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_old_pwd" TextMode="Password" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Enter old password" Display="None" ControlToValidate="tb_old_pwd" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="New Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_new_pwd" TextMode="Password" runat="server" Height="32px" Width="281px" onkeyup="javascript:validatePw()"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Enter new password" Display="None" ControlToValidate="tb_new_pwd" ForeColor="Red"></asp:RequiredFieldValidator>
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
                            OnClick="btn_Log_Click" Text="Change Password" Width="288px" />
                        <br />
                        <br />
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

<script type="text/javascript">
    function validatePw() {
        var str = document.getElementById('<%=tb_new_pwd.ClientID %>').value;

        if (str.length < 8) {
            document.getElementById("lbl_pwdchecker").innerHTML = "Password Length must be at least 12 characters";
            document.getElementById("lbl_pwdchecker").style.color = "Red";
            return ("too_short");
        }

        else if (str.search(/[a-z]/) == -1) {
            document.getElementById("lbl_pwdchecker").innerHTML = "Very Weak - Password require at least 1 lowercase letter";
            document.getElementById("lbl_pwdchecker").style.color = "Red";
            return ("no_lowercase");
        }

        else if (str.search(/[A-Z]/) == -1) {
            document.getElementById("lbl_pwdchecker").innerHTML = "Weak - Password require at least 1 uppercase letter";
            document.getElementById("lbl_pwdchecker").style.color = "Red";
            return ("no_uppercase");
        }

        else if (str.search(/[0-9]/) == -1) {
            document.getElementById("lbl_pwdchecker").innerHTML = "Medium - Password require at least 1 number";
            document.getElementById("lbl_pwdchecker").style.color = "OrangeRed";
            return ("no_number");
        }

        else if (str.search(/[^a-zA-Z0-9]/) == -1) {
            document.getElementById("lbl_pwdchecker").innerHTML = "Strong - Password require at least 1 special character";
            document.getElementById("lbl_pwdchecker").style.color = "Orange";
            return ("no_special");
        }

        document.getElementById("lbl_pwdchecker").innerHTML = "Excellent";
        document.getElementById("lbl_pwdchecker").style.color = "Green";
    }

    grecaptcha.ready(function () {
        grecaptcha.execute('6LctjlMdAAAAALvO9sUcOvgVa8S240iApRCsFQmQ', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
        });
    });
</script>
