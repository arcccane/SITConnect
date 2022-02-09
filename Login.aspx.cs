using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        string SITDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

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

        private bool checkEmail(string email)
        {
            bool valid = true;

            // check if email exists in db
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
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

        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }

        public int getAttemptsLeft(string email)
        {
            int RetryAttempts = 0;
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select AttemptsLeft FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RetryAttempts = Convert.ToInt32(reader["AttemptsLeft"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return RetryAttempts;
        }

        public int getTotalFailedAttempts(string email)
        {
            int totalFailedAttempts = 0;
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select TotalFailedAttempts FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalFailedAttempts = Convert.ToInt32(reader["TotalFailedAttempts"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return totalFailedAttempts;
        }

        public int getLoginCount(string email)
        {
            int logincount = 0;
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select LoginCount FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        logincount = Convert.ToInt32(reader["LoginCount"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return logincount;
        }

        public bool getIsLocked(string email)
        {
            bool isLocked = false;
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "select isLocked FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        isLocked = Convert.ToBoolean(reader["isLocked"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return isLocked;
        }
        
        public void decreaseAttempts(int attemptsleft,string email)
        {
            if (attemptsleft > 0)
            {
                SqlConnection connection = new SqlConnection(SITDBConnectionString);
                string sql = "update Account SET AttemptsLeft=@AttemptsLeft WHERE Email=@Email";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@AttemptsLeft", attemptsleft - 1);
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
        }

        public void increaseTotalFailedAttempts(int totalfailedattempts, string email)
        {
            
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "update Account SET TotalFailedAttempts=@TotalFailedAttempts WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@TotalFailedAttempts", totalfailedattempts + 1);
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

        public void lockAccount(bool isLocked, string email)
        {
            if (!isLocked)
            {
                SqlConnection connection = new SqlConnection(SITDBConnectionString);
                string sql = "update Account SET isLocked=@isLocked, LockedDatetime=@LockedDatetime WHERE Email=@Email";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isLocked", 1);
                command.Parameters.AddWithValue("@LockedDatetime", DateTime.Now);
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
        }

        public void unlockAccount(bool isLocked, string email)
        {
            if (isLocked)
            {
                SqlConnection connection = new SqlConnection(SITDBConnectionString);
                string sql = "update Account SET AttemptsLeft=@AttemptsLeft, isLocked=@isLocked, LockedDatetime=@LockedDatetime WHERE Email=@Email";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@AttemptsLeft", 2);
                command.Parameters.AddWithValue("@isLocked", false);
                command.Parameters.AddWithValue("@LockedDatetime", DBNull.Value);
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
        }

        public int accountRecovery(bool isLocked, string email)
        {
            int timePassedinMins = 0;
            if (isLocked)
            {
                SqlConnection connection = new SqlConnection(SITDBConnectionString);
                string sql = "select LockedDatetime FROM Account WHERE Email=@Email";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", email);
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            if (reader["LockedDatetime"] != DBNull.Value)
                            {
                                DateTime dateTimeOfAccountLock = Convert.ToDateTime(reader["LockedDatetime"]);
                                TimeSpan timePassed = DateTime.Now.Subtract(dateTimeOfAccountLock);
                                timePassedinMins = timePassed.Minutes;
                                if (timePassedinMins >= 60)
                                {
                                    unlockAccount(isLocked, email);
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
            }
            return timePassedinMins;
        }

        public bool passwordExpired(string email)
        {
            bool expired = false;
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
                        if (passwordAge.Minutes > 129600)
                        {
                            expired = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return expired;
        }

        protected void increaseLoginCount(int count, string email)
        {
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "update Account SET LoginCount=@LoginCount WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@LoginCount", count + 1);
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

        protected void setIsLoggedIn(string email)
        {
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "update Account SET isLoggedIn=@isLoggedIn WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@isLoggedIn", 1);
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

        protected void btn_Log_Click(object sender, EventArgs e)
        {
            lb_error.Text = "";
            lb_error2.Text = "";

            if (ValidateCaptcha())
            {
                string pwd = tb_login_pwd.Text.ToString().Trim();
                string email = tb_login_email.Text.ToString().Trim();

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                int RetryAttempts = getAttemptsLeft(email);
                int TotalFailedAttempts = getTotalFailedAttempts(email);
                int TotalLoginCount = getLoginCount(email);
                bool isLocked = getIsLocked(email);

                int timePassedAfterLockOutinMins = accountRecovery(isLocked, email);

                int minutesToUnlock = 60 - timePassedAfterLockOutinMins;

                try
                {
                    if (checkEmail(email) == true)
                    {
                        lb_error.Text = "Email or password is incorrect. Please try again.";
                        lb_error2.Text = "";
                    }

                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash.Equals(dbHash) && isLocked == false)
                        {
                            if (passwordExpired(email))
                            {
                                Response.Redirect("ChangePassword.aspx", false);
                            }
                            else
                            {
                                Session["Email"] = email;

                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;

                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                                setIsLoggedIn(email);
                                increaseLoginCount(TotalLoginCount, email);

                                Response.Redirect("Home.aspx", false);
                            }
                        }
                        else
                        {
                            if (RetryAttempts == 0)
                            {
                                if (isLocked == false)
                                {
                                    lockAccount(isLocked, email);
                                    increaseTotalFailedAttempts(TotalFailedAttempts, email);
                                    lb_error.Text = "Account locked.";
                                    lb_error2.Text = "";
                                }
                                else
                                {
                                    if (minutesToUnlock > 0)
                                    {
                                        lb_error.Text = "Your account is locked. Unlock account automatically in " + minutesToUnlock + " minutes.";
                                        lb_error2.Text = "";
                                    }
                                    else
                                    {
                                        lb_error.Text = "Account unlocked.";
                                        lb_error2.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                lb_error.Text = "Password is incorrect. Please try again.";
                                lb_error2.Text = "You have " + RetryAttempts + " attempts remaining.";
                                decreaseAttempts(RetryAttempts, email);
                                increaseTotalFailedAttempts(TotalFailedAttempts, email);
                            }
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