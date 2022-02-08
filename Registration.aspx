<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect Registeration</title>
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
            <legend>Account Registration</legend>
                <br />
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_firstname" runat="server" Height="36px" Width="280px"></asp:TextBox>
                        <%-- Only allow letters --%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Invalid first name" ControlToValidate="tb_firstname" ValidationExpression="(^[a-zA-Z]+$)" ForeColor="Red"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_firstname" ErrorMessage="First Name required" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_lastname" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <%-- Only allow letters --%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Invalid last name" ValidationExpression="(^[a-zA-Z]+$)" ControlToValidate="tb_lastname" ForeColor="Red"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Last Name required" ControlToValidate="tb_lastname" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Credit Card Info"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_creditcard" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <%-- for visa cards: 13 or 16 digits long, starts with 4, each digit should be 0-9, no alphabets and special characters--%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid credit card" ControlToValidate="tb_creditcard" ValidationExpression="^4[0-9]{12}(?:[0-9]{3})?$" ForeColor="Red"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Credit Card Info required" ControlToValidate="tb_creditcard" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Email address"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_email" TextMode="Email" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <%-- check if email is unique --%>
                        <asp:Label ID="lbl_email" runat="server" Text=""></asp:Label>
                        <%-- email regex --%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid email format" ControlToValidate="tb_email" ForeColor="Red" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$" Display="None"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email required" ControlToValidate="tb_email" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
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
                        <asp:TextBox ID="tb_pwd" TextMode="Password" runat="server" Height="32px" Width="281px" onkeyup="javascript:validatePw()"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Password required" ControlToValidate="tb_pwd" Display="None" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="Label7" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_date" runat="server" TextMode="Date" Height="32px" Width="285px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Date of Birth required" ControlToValidate="tb_date" Display="None" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style5">
                        <asp:Label ID="Label8" runat="server" Text="Photo"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:FileUpload ID="fu_file" runat="server" />
                        <asp:Label ID="lbl_photo" runat="server" Text=""></asp:Label>
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
                            OnClick="btn_Reg_Click" Text="Register" Width="288px" />
                        <br />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Blue" NavigateUrl="~/Login.aspx">Login</asp:HyperLink>
                    </td>
                </tr>
            </table>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            &nbsp;<br />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Display="Static" ForeColor="Red" />
            <br />
            </fieldset>
        </div>
    </form>
</body>
</html>

<script type="text/javascript">

    function validatePw() {
        var str = document.getElementById('<%=tb_pwd.ClientID %>').value;

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

