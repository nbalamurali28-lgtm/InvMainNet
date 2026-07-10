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
            userDropdown.InnerText = Convert.ToString(Session["UserName"]);
            //LblLoginTime.InnerText = Convert.ToString(Session["LoginTime"]);
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