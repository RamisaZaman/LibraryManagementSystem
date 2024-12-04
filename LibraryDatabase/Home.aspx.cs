using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LibraryDatabase
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Display the logout message if redirected with 'loggedout'
                if (Request.QueryString["message"] == "loggedout")
                {
                    lblMessage.Text = "You have successfully logged out.";
                }

                // Check if the user is already logged in
                if (Session["UserRole"] != null)
                {
                    string currentPage = Request.Url.AbsolutePath.ToLower();

                    // Redirect to the correct page if the user is not already there
                    if (Session["UserRole"].ToString() == "Staff" && !currentPage.EndsWith("default.aspx"))
                    {
                        Response.Redirect("~/Default.aspx", false); // Prevent infinite loop
                    }
                    else if (Session["UserRole"].ToString() == "Member" && !currentPage.EndsWith("borrowtransactions.aspx"))
                    {
                        Response.Redirect("~/BorrowTransactions.aspx", false); // Prevent infinite loop
                    }
                }

            }

        }

        protected void btnLoginStaff_Click(object sender, EventArgs e)
        {
            string staffID = txtStaffID.Text.Trim();
            string staffName = txtStaffName.Text.Trim();

            string query = $"SELECT * FROM staff WHERE staffid = {staffID} AND name = '{staffName}'";
            DataTable result = databaseHelper.dbRead(query);

            if (result.Rows.Count > 0)
            {
                Session["UserRole"] = "Staff";
                Session["StaffID"] = staffID;
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                lblMessageStaff.Text = "Invalid Staff ID or Name.";
            }
        }

        protected void btnLoginMember_Click(object sender, EventArgs e)
        {
            string memberEmail = txtMemberEmail.Text.Trim();

            string query = $"SELECT * FROM members WHERE email = '{memberEmail}'";
            DataTable result = databaseHelper.dbRead(query);

            if (result.Rows.Count > 0)
            {
                Session["UserRole"] = "Member";
                Session["MemberEmail"] = memberEmail;
                Response.Redirect("~/BorrowTransactions.aspx");
            }
            else
            {
                lblMessageMember.Text = "Invalid Member Email.";
            }
        }

        protected void btnCreateMemberAccount_Click(object sender, EventArgs e)
        {
            string name = txtNewMemberName.Text.Trim();
            string phoneNumber = txtNewMemberPhone.Text.Trim();
            string email = txtNewMemberEmail.Text.Trim();
            string membershipDate = txtNewMembershipDate.Text.Trim();

            // Check if email already exists
            string checkQuery = $"SELECT * FROM members WHERE email = '{email}'";
            DataTable existingMember = databaseHelper.dbRead(checkQuery);

            if (existingMember.Rows.Count > 0)
            {
                lblMessageCreateMember.Text = "An account with this email already exists.";
                return;
            }

            // Insert new member into the database
            string insertQuery = $"INSERT INTO members (name, phonenumber, email, membershipdate) " +
                                 $"VALUES ('{name}', '{phoneNumber}', '{email}', '{membershipDate}')";
            databaseHelper.dbModify(insertQuery);

            lblMessageCreateMember.Text = "Account created successfully!";
        }
    }
}