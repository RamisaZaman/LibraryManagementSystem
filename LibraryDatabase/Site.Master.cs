using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryDatabase
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Show the logout button only if the user is logged in
            if (Session["UserRole"] == null)
            {
                phLogout.Visible = false; // Hide the logout button
            }
            else
            {
                phLogout.Visible = true; // Show the logout button
            }
        }
        protected void Logout_Click(object sender, EventArgs e)
        {
            // Clear the user's session and redirect to the home page with a logout message
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Home.aspx?message=loggedout");
        }

    }
}