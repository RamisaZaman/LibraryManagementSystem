using System;
using System.Collections.Generic;
using System.Data;

namespace LibraryDatabase
{
    public partial class Members : System.Web.UI.Page
    {
        // In-memory lists to simulate the database
        private static List<Member> members = new List<Member>();
        private static List<Staff> staffList = new List<Staff>();
        private static int memberIDCounter = 1;
        private static int staffIDCounter = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in and has staff privileges
            if (Session["UserRole"] == null)
            {
                Response.Redirect("~/Home.aspx");
            }
            else if (Session["UserRole"].ToString() != "Staff")
            {
                Response.Redirect("~/BorrowTransactions.aspx");
            }

            if (!IsPostBack)
            {
                loadMembers();
                loadStaff();
                BindGrids();
            }
        }

        public void loadMembers()
        {
            members.Clear(); // Reset list to avoid duplicates
            DataTable data = databaseHelper.dbRead("SELECT * FROM members;");
            foreach (DataRow row in data.Rows)
            {
                members.Add(new Member
                {
                    UserID = Convert.ToInt32(row["userid"]),
                    Name = row["name"].ToString(),
                    PhoneNumber = row["phonenumber"].ToString(),
                    Email = row["email"].ToString(),
                    MembershipDate = Convert.ToDateTime(row["membershipdate"])
                });
                memberIDCounter++;
            }
        }

        public void loadStaff()
        {
            staffList.Clear(); // Reset list to avoid duplicates
            DataTable data = databaseHelper.dbRead("SELECT * FROM staff;");
            foreach (DataRow row in data.Rows)
            {
                staffList.Add(new Staff
                {
                    StaffID = Convert.ToInt32(row["staffid"]),
                    Name = row["name"].ToString(),
                    Role = row["role"].ToString()
                });
                staffIDCounter++;
            }
        }

        protected void btnAddMember_Click(object sender, EventArgs e)
        {
            // Create a new member
            var newMember = new Member
            {
                UserID = memberIDCounter++,
                Name = txtName.Text,
                PhoneNumber = txtPhoneNumber.Text,
                Email = txtEmail.Text,
                MembershipDate = DateTime.TryParse(txtMembershipDate.Text, out DateTime date) ? date : DateTime.Now
            };

            // Check if phone or email already exists in the database
            DataTable data = databaseHelper.dbRead($"SELECT * FROM members WHERE phonenumber='{newMember.PhoneNumber}' OR email='{newMember.Email}';");
            if (data.Rows.Count == 0)
            {
                string query = $"INSERT INTO members (name, phonenumber, email, membershipdate) VALUES ('{newMember.Name}', '{newMember.PhoneNumber}', '{newMember.Email}', '{newMember.MembershipDate:yyyy-MM-dd}')";
                databaseHelper.dbModify(query);

                members.Add(newMember);
                ClearMemberFields();
                loadMembers();
                BindGrids();
            }
            else
            {
                Label1.Text = "Phone or Email already exists in members.";
            }
        }

        protected void btnAddStaff_Click(object sender, EventArgs e)
        {
            // Create a new staff account
            var newStaff = new Staff
            {
                StaffID = staffIDCounter++,
                Name = txtStaffName.Text,
                Role = txtRole.Text
            };

            string query = $"INSERT INTO staff (name, role) VALUES ('{newStaff.Name}', '{newStaff.Role}')";
            databaseHelper.dbModify(query);

            staffList.Add(newStaff);
            ClearStaffFields();
            loadStaff();
            BindGrids();
        }

        protected void gvMembers_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(e.Keys["userID"]);

            // Remove dependencies from borrow and reservation transactions
            databaseHelper.dbModify($"DELETE FROM borrow_transactions WHERE userid={userId}");
            databaseHelper.dbModify($"DELETE FROM reservation_transactions WHERE userid={userId}");

            // Delete the member
            databaseHelper.dbModify($"DELETE FROM members WHERE userid={userId}");
            members.RemoveAll(m => m.UserID == userId);
            loadMembers();
            BindGrids();
        }

        protected void gvStaff_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int staffId = Convert.ToInt32(e.Keys["StaffID"]);
            databaseHelper.dbModify($"DELETE FROM staff WHERE staffid={staffId}");
            staffList.RemoveAll(s => s.StaffID == staffId);
            loadStaff();
            BindGrids();
        }

        private void ClearMemberFields()
        {
            txtName.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMembershipDate.Text = string.Empty;
        }

        private void ClearStaffFields()
        {
            txtStaffName.Text = string.Empty;
            txtRole.Text = string.Empty;
        }

        private void BindGrids()
        {
            gvMembers.DataSource = members;
            gvMembers.DataBind();

            gvStaff.DataSource = staffList;
            gvStaff.DataBind();
        }
    }

    // Member class to represent a single member entity
    public class Member
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime MembershipDate { get; set; }
    }

    // Staff class to represent a single staff entity
    public class Staff
    {
        public int StaffID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
