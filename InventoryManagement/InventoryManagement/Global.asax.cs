using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace InventoryManagement
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // This triggers only ONCE when the website first powers up

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // This triggers EVERY TIME a new user starts a unique session

            // Set your default session variables here
            //Session["UserRoleId"] = "Guest";
            //Session["UserName"] = "Ram";
            //Session["SessionStartTime"] = DateTime.Now;
            Response.Redirect("~/Login.aspx");
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            // Log session end or perform cleanup
            // Note: The Session.Clear() call is often redundant here because 
            // Session.Abandon() or a timeout already destroys the collection.
        }
    }
}