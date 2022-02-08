using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string SITDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int checkPassword(string password)
        {
            // score calculator for password
            int score = 0;

            // very weak
            if (password.Length < 12)
            {
                return score;
            }
            else
            {
                score = 1;
            }

            // weak
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            // medium
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            // strong
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // excellent
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }

            return score;
        }

        private bool feedBackPassword()
        {
            // generate feedback for password
            bool excellent = false;
            int scores = checkPassword(tb_new_pwd.Text);
            string status = "";
            switch (scores)
            {
                case 0:
                    status = "Password Length must be at least 12 characters";
                    break;
                case 1:
                    status = "Very Weak - Password require at least 1 lowercase letter";
                    break;
                case 2:
                    status = "Weak - Password require at least 1 uppercase letter";
                    break;
                case 3:
                    status = "Medium - Password require at least 1 number";
                    break;
                case 4:
                    status = "Strong - Password require at least 1 special character";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwdchecker.Text = status;
            if (scores < 3)
            {
                lbl_pwdchecker.ForeColor = Color.Red;
            }
            else if (scores < 4)
            {
                lbl_pwdchecker.ForeColor = Color.OrangeRed;
            }
            else if (scores < 5)
            {
                lbl_pwdchecker.ForeColor = Color.Orange;
            }
            else
            {
                lbl_pwdchecker.ForeColor = Color.Green;
                excellent = true;
            }
            return excellent;
        }

        public bool passwordTooNew(string email)
        {
            bool tooNew = false;
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select CreatedOn FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime passwordCreated = Convert.ToDateTime(reader["CreatedOn"]);
                        TimeSpan passwordAge = DateTime.Now.Subtract(passwordCreated);
                        if (passwordAge.Minutes < 10)
                        {
                            tooNew = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return tooNew;
        }

        protected string getDBSalt(string email)
        {
            string s = null;

            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt"] != null)
                        {
                            if (reader["PasswordSalt"] != DBNull.Value)
                            {
                                s = reader["PasswordSalt"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;
        }

        protected string getDBHash(string email)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }
        protected void protectPassword()
        {
            string pwd = HttpUtility.HtmlEncode(tb_new_pwd.Text.ToString().Trim());
            try
            {
                //Generate random salt 
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void updatePassword(string hash, string salt, string email)
        {
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "update Account SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt, CreatedOn=@CreatedOn WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PasswordHash", hash);
            command.Parameters.AddWithValue("@PasswordSalt", salt);
            command.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
        }

        public class CaptchaResult
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LctjlMdAAAAAN9VSH1FzgvEefIqNShP6vCQymCP &response=" + captchaResponse);
            try
            {
                // receive response from google
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        CaptchaResult jsonObject = js.Deserialize<CaptchaResult>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_Log_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                if (feedBackPassword())
                {
                    string oldpwd = tb_old_pwd.Text.ToString().Trim();
                    string newpwd = tb_new_pwd.Text.ToString().Trim();
                    string email = tb_login_email.Text.ToString().Trim();

                    SHA512Managed hashing = new SHA512Managed();
                    string dbHash = getDBHash(email);
                    string dbSalt = getDBSalt(email);
                    try
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            string pwdWithSalt = oldpwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbHash))
                            {
                                if (newpwd != oldpwd)
                                {
                                    if (!passwordTooNew(email))
                                    {
                                        protectPassword();
                                        updatePassword(finalHash, salt, email);
                                        Response.Redirect("Login.aspx", false);
                                    }
                                    else
                                    {
                                        lb_error.Text = "Password changed too recently.";
                                    }
                                }
                                else
                                {
                                    lb_error.Text = "Old and new password cannot be the same.";
                                }
                            }
                            else
                            {
                                lb_error.Text = "Email or old password is not valid. Please try again.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
            }
        }
    }
}