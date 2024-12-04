using System;
using System.Collections.Generic;
using System.Data;

namespace LibraryDatabase
{
    public partial class ReservationTransactions : System.Web.UI.Page
    {
        // In-memory list to simulate a database
        private static List<ReservationTransaction> reservations = new List<ReservationTransaction>();
        private static List<Book> books = new List<Book>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string userRole = Session["UserRole"]?.ToString();

            if (userRole == null || (userRole != "Staff" && userRole != "Member"))
            {
                Response.Redirect("~/Home.aspx");
            }

            if (!IsPostBack)
            {
                if (userRole == "Staff")
                {
                    phStaffContent.Visible = true;
                    LoadReservations();
                    BindGrid();
                    LoadPriorityList();
                }
                else if (userRole == "Member")
                {
                    phMemberContent.Visible = true;
                    loadBooks();
                    BindGrid();
                }
            }
        }

        public void loadBooks()
        {
            books.Clear();
            DataTable data = databaseHelper.dbRead("SELECT * FROM books;");
            foreach (DataRow row in data.Rows)
            {
                books.Add(new Book
                {
                    BookId = Convert.ToInt32(row["bookid"]),
                    Title = row["title"].ToString(),
                    Author = row["author"].ToString(),
                    Year = Convert.ToInt32(row["publishingyear"]),
                    Genre = row["genre"].ToString(),
                    BorrowingStatus = Convert.ToBoolean(row["borrowingstatus"])
                });
            }
        }

        public void LoadReservations()
        {
            try
            {
                reservations.Clear();
                DataTable data = databaseHelper.dbRead("SELECT * FROM reservation_transactions ORDER BY reservationdate ASC;");
                foreach (DataRow row in data.Rows)
                {
                    reservations.Add(new ReservationTransaction
                    {
                        ReservationID = Convert.ToInt32(row["reservationid"]),
                        UserID = Convert.ToInt32(row["userid"]),
                        BookID = Convert.ToInt32(row["bookid"]),
                        ReservationDate = Convert.ToDateTime(row["reservationdate"])
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading reservations: {ex.Message}");
            }
        }

        private void LoadPriorityList()
        {
            try
            {
                string query = @"
                SELECT bookid, userid, reservationdate
                FROM reservation_transactions
                ORDER BY bookid ASC, reservationdate ASC;";
                DataTable priorityList = databaseHelper.dbRead(query);

                gvPriorityList.DataSource = priorityList;
                gvPriorityList.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading priority list: {ex.Message}");
            }
        }

        protected void btnReserveBook_Click(object sender, EventArgs e)
        {
            string userRole = Session["UserRole"]?.ToString();

            if (userRole == "Staff")
            {
                ReserveForStaff();
            }
            else if (userRole == "Member")
            {
                ReserveForMember();
            }
            // Update priority list after every reservation
            LoadPriorityList();
        }

        private void ReserveForStaff()
        {
            int userID = int.TryParse(txtUserID.Text, out int uid) ? uid : 0;
            int bookID = int.TryParse(txtBookID.Text, out int bid) ? bid : 0;
            string reservationDate = txtReservationDate.Text;

            if (userID == 0 || bookID == 0 || string.IsNullOrEmpty(reservationDate))
            {
                lblStaffMessage.Text = "All fields are required.";
                return;
            }

            try
            {
                string query = $"INSERT INTO reservation_transactions (userid, bookid, reservationdate) " +
                               $"VALUES ({userID}, {bookID}, '{reservationDate}');";
                databaseHelper.dbModify(query);

                lblStaffMessage.Text = "Reservation added successfully.";
                LoadReservations();
                BindGrid();
                LoadPriorityList(); // Update the priority list

            }
            catch (Exception ex)
            {
                lblStaffMessage.Text = $"Error: {ex.Message}";
            }
        }

        private void ReserveForMember()
        {
            int bookID = int.TryParse(txtMemberBookID.Text, out int bid) ? bid : 0;

            string memberEmail = Session["MemberEmail"]?.ToString();
            if (string.IsNullOrEmpty(memberEmail))
            {
                lblReservationMessage.Text = "User not logged in.";
                return;
            }

            DataTable memberData = databaseHelper.dbRead($"SELECT userid FROM members WHERE email = '{memberEmail}';");
            if (memberData.Rows.Count == 0)
            {
                lblReservationMessage.Text = "Invalid user.";
                return;
            }
            int userID = Convert.ToInt32(memberData.Rows[0]["userid"]);

            DataTable bookData = databaseHelper.dbRead($"SELECT borrowingstatus FROM books WHERE bookid = {bookID};");
            if (bookData.Rows.Count == 0)
            {
                lblReservationMessage.Text = "Book does not exist.";
                return;
            }

            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string query = $"INSERT INTO reservation_transactions (userid, bookid, reservationdate) " +
                           $"VALUES ({userID}, {bookID}, '{currentDate}');";
            try
            {
                databaseHelper.dbModify(query);
                lblReservationMessage.Text = "Reservation placed successfully.";
                loadBooks();
                BindGrid();
                LoadPriorityList();
            }
            catch (Exception ex)
            {
                lblReservationMessage.Text = $"Error: {ex.Message}";
            }
        }

        protected void gvReservations_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int reservationID = Convert.ToInt32(e.Keys["ReservationID"]);
            try
            {
                string query = $"DELETE FROM reservation_transactions WHERE reservationid={reservationID};";
                databaseHelper.dbModify(query);

                reservations.RemoveAll(r => r.ReservationID == reservationID);
                LoadReservations();
                BindGrid();
                LoadPriorityList();
            }
            catch (Exception ex)
            {
                lblStaffMessage.Text = $"Error deleting reservation: {ex.Message}";
            }
        }
         
        private void BindGrid()
        {
            gvReservations.DataSource = reservations;
            gvReservations.DataBind();

            gvBooks.DataSource = books;
            gvBooks.DataBind();
        }
    }

    public class ReservationTransaction
    {
        public int ReservationID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
