using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    userNameDropdown.InnerText = Convert.ToString(Session["UserName"]);
                    LblUserRoleName.InnerText = "User Role: " + Convert.ToString(Session["UserRoleName"]);

                    if (Convert.ToString(Session["UserRoleId"]) == "ROLE_ADMIN")
                    {
                        menuMasters.Visible = true;
                        menuTransactions.Visible = true;
                        menuDeliveryInfoAdmin.Visible = true;
                        menuReports.Visible = true;
                        menuToDoTasks.Visible = true;
                    }
                    else
                    {
                        menuTransactions.Visible = true;
                        menuReports.Visible = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnLogout_ServerClick(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                FormsAuthentication.SignOut();
                Response.Redirect("Login.aspx");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}