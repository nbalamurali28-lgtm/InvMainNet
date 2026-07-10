using InventoryManagement.IL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class Login : System.Web.UI.Page
    {
        ClsLogin objLogin = new ClsLogin();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                // Simple static validation (Replace this with a Database check!)
                DataTable dtUser = objLogin.GetUserDetails(userName);

                if(dtUser != null && dtUser.Rows.Count > 0)
                {
                    if (userName == Convert.ToString(dtUser.Rows[0]["username"]) && password == "admin")
                    {
                        // Create a session to keep the user logged in
                        Session["UserRoleId"] = Convert.ToString(dtUser.Rows[0]["role_id"]);
                        Session["UserName"] = Convert.ToString(dtUser.Rows[0]["username"]);
                        Session["LoginTime"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                        Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Invalid credentials.";
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid credentials.";
                }

                /*if (user == "admin" && pass == "admin")
                {
                    // Create a session to keep the user logged in
                    Session["UserRoleId"] = "Guest";
                    Session["UserName"] = user;
                    Session["LoginTime"] = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblMessage.Text = "Invalid credentials.";
                }*/
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}