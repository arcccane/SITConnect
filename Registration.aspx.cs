using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        string SITDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(SITDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName,@CreditCardInfo,@Email,@PasswordHash,@PasswordSalt,@DateOfBirth,@IV,@Key,@CreatedOn,@AttemptsLeft,@TotalFailedAttempts,@isLocked,@LockedDatetime)"))                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lastname.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardInfo", Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_date.Text.Trim());
                            // cmd.Parameters.AddWithValue("@Photo", null);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now);
                            cmd.Parameters.AddWithValue("@AttemptsLeft", 3);
                            cmd.Parameters.AddWithValue("TotalFailedAttempts", 0);
                            cmd.Parameters.AddWithValue("@isLocked", 0);
                            cmd.Parameters.AddWithValue("@LockedDatetime", DBNull.Value);
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            return cipherText;
        }

        protected void protectPassword()
        {
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());
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

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
            } 
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
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
            if (Regex.IsMatch(password,"[a-z]"))
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
            int scores = checkPassword(tb_pwd.Text);
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

        private bool checkEmail()
        {
            bool valid = true;
            string emailexists = "Email already registered";

            // check if email exists in db
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", tb_email.Text);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = emailexists;
                            lbl_email.ForeColor = Color.Red;
                            valid = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }
            return valid;
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
        protected void btn_Reg_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                if (checkEmail())
                {
                    if (feedBackPassword())
                    {
                        protectPassword();
                        createAccount();
                        Response.Redirect("Login.aspx", false);
                    }
                }
            }
        }
    }
}