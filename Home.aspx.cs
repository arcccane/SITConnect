﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Home : System.Web.UI.Page
    {
        string SITDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        static byte[] creditcardinfo = null;
        static string email = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Email"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lblMessage.Text = "You are logged in.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    email = Session["Email"].ToString();
                    displayUserProfile(email);
                    btnLogout.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void displayUserProfile(string email)
        {
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
                        if (reader["FirstName"] != DBNull.Value)
                        {
                            lbl_firstname.Text = reader["FirstName"].ToString();
                        }
                        if (reader["LastName"] != DBNull.Value)
                        {
                            lbl_lastname.Text = reader["LastName"].ToString();
                        }
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["Email"].ToString();
                        }
                        if (reader["DateOfBirth"] != DBNull.Value)
                        {
                            lbl_date.Text = reader["DateOfBirth"].ToString();
                        }
                        if (reader["CreditCardInfo"] != DBNull.Value)
                        {
                            creditcardinfo = Convert.FromBase64String(reader["CreditCardInfo"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            IV = Convert.FromBase64String(reader["IV"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            Key = Convert.FromBase64String(reader["Key"].ToString());
                        }
                    }
                    lbl_credit.Text = decryptData(creditcardinfo);
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
        }

        protected void resetAttemptsLeft()
        {
            SqlConnection connection = new SqlConnection(SITDBConnectionString);
            string sql = "update Account SET Attempts=@Attempts WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Attempts", 3);
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

            finally
            {
                connection.Close();
            }
        }

        protected void Logout(object sender, EventArgs e)
        {

            resetAttemptsLeft();

            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Request.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Request.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", string.Empty));
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
    }
}