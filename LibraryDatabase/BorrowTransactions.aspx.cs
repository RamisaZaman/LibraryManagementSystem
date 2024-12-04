using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace LibraryDatabase
{
    public partial class BorrowTransactions : System.Web.UI.Page
    {
        // List to simulate a database
        private static List<BorrowTransaction> transactions = new List<BorrowTransaction>();

        protected void Page_Load(object sender, EventArgs e)
        {
            string userRole = Session["UserRole"]?.ToString();

            if (userRole == null)
            {
                Response.Redirect("~/Home.aspx");
            }

            if (!IsPostBack)
            {
                if (userRole == "Staff")
                {
                    phStaffContent.Visible = true;
                    phStaffFields.Visible = true;
                    loadTransactions();
                    BindGrid();
                    LoadLateReturns();
                }
                else if (userRole == "Member")
                {
                    phMemberContent.Visible = true;
                    LoadAvailableBooks();
                }
            }
        }

        // Load Transactions for Staff
        public void loadTransactions()
        {
            try
            {
                transactions.Clear();
                DataTable data = databaseHelper.dbRead("SELECT * FROM borrow_transactions;");
                foreach (DataRow row in data.Rows)
                {
                    var transaction = new BorrowTransaction
                    {
                        TransactionID = Convert.ToInt32(row["transactionid"]),
                        UserID = Convert.ToInt32(row["userid"]),
                        BookID = Convert.ToInt32(row["bookid"]),
                        BorrowDate = Convert.ToDateTime(row["borrowdate"]),
                        ReturnDate = row["returndate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["returndate"])
                    };
                    transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading transactions: " + ex.Message);
            }
        }
        public void LoadLateReturns()
        {
            try
            {
                // Query to fetch late returns
                string query = @"
            SELECT 
                userid, 
                bookid, 
                (CURRENT_DATE - returndate) * 0.50 AS LateFee
            FROM borrow_transactions
            WHERE returndate < CURRENT_DATE;";

                DataTable lateReturnsData = databaseHelper.dbRead(query);

                // Bind data to the GridView
                gvLateReturns.DataSource = lateReturnsData;
                gvLateReturns.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading late returns: " + ex.Message);
            }
        }


        // Add Transaction for Staff
        protected void btnAddTransaction_Click(object sender, EventArgs e)
        {
            int userID = int.TryParse(txtUserID.Text, out int uid) ? uid : 0;
            int bookID = int.TryParse(txtBookID.Text, out int bid) ? bid : 0;

            // Validate book availability
            DataTable bookData = databaseHelper.dbRead($"SELECT borrowingstatus FROM books WHERE bookid={bookID};");
            if (bookData.Rows.Count == 0 || bookData.Rows[0]["borrowingstatus"].ToString() == "t")
            {
                lblStaffMessage.Text = "Book is not available or does not exist.";
                return;
            }

            // Validate user existence
            DataTable userData = databaseHelper.dbRead($"SELECT * FROM members WHERE userid={userID};");
            if (userData.Rows.Count == 0)
            {
                lblStaffMessage.Text = "User does not exist.";
                return;
            }

            // Validate and parse borrow and return dates for staff input
            string borrowDate = DateTime.Now.ToString("yyyy-MM-dd");
            string returnDate = DateTime.Now.AddDays(21).ToString("yyyy-MM-dd");

            if (phStaffFields.Visible) // For staff-specific input
            {
                borrowDate = !string.IsNullOrEmpty(txtBorrowDate.Text) ? txtBorrowDate.Text : borrowDate;
                returnDate = !string.IsNullOrEmpty(txtReturnDate.Text) ? txtReturnDate.Text : returnDate;
            }

            try
            {
                // Insert transaction
                string query = $"INSERT INTO borrow_transactions (userid, bookid, borrowdate, returndate) " +
                               $"VALUES ({userID}, {bookID}, '{borrowDate}', '{returnDate}')";
                databaseHelper.dbModify(query);

                // Update book status
                string updateQuery = $"UPDATE books SET borrowingstatus = 't' WHERE bookid = {bookID}";
                databaseHelper.dbModify(updateQuery);

                lblStaffMessage.Text = "Transaction added successfully.";
                loadTransactions();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblStaffMessage.Text = $"Error: {ex.Message}";
            }
        }


        // Load Available Books for Members
        public void LoadAvailableBooks()
        {
            try
            {
                string query = "SELECT bookid, title, author, genre, publishingyear FROM books WHERE borrowingstatus = 'f'";
                DataTable availableBooks = databaseHelper.dbRead(query);
                gvAvailableBooks.DataSource = availableBooks;
                gvAvailableBooks.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading available books: " + ex.Message);
            }
        }

        // Borrow Book for Members
        protected void btnBorrowBook_Click(object sender, EventArgs e)
        {
            int bookID = int.TryParse(txtMemberBookID.Text, out int bid) ? bid : 0;

            string memberEmail = Session["MemberEmail"]?.ToString();
            if (memberEmail == null)
            {
                lblMemberMessage.Text = "User not logged in.";
                return;
            }

            // Get member ID
            DataTable memberData = databaseHelper.dbRead($"SELECT userid FROM members WHERE email = '{memberEmail}';");
            if (memberData.Rows.Count == 0)
            {
                lblMemberMessage.Text = "Invalid user.";
                return;
            }
            int userID = Convert.ToInt32(memberData.Rows[0]["userid"]);

            // Validate book availability
            DataTable bookData = databaseHelper.dbRead($"SELECT borrowingstatus FROM books WHERE bookid = {bookID};");
            if (bookData.Rows.Count == 0 || bookData.Rows[0]["borrowingstatus"].ToString() == "t")
            {
                lblMemberMessage.Text = "Book is not available or does not exist.";
                return;
            }

            // Insert borrow transaction
            DateTime borrowDate = DateTime.Now;
            DateTime returnDate = borrowDate.AddDays(21);
            string query = $"INSERT INTO borrow_transactions (userid, bookid, borrowdate, returndate) " +
                           $"VALUES ({userID}, {bookID}, '{borrowDate:yyyy-MM-dd}', '{returnDate:yyyy-MM-dd}')";
            databaseHelper.dbModify(query);

            // Update book status
            string updateQuery = $"UPDATE books SET borrowingstatus = 't' WHERE bookid = {bookID}";
            databaseHelper.dbModify(updateQuery);

            lblMemberMessage.Text = "Book borrowed successfully.";
            LoadAvailableBooks();
        }
        protected void btnReturnBook_Click(object sender, EventArgs e)
        {
            int bookID = int.TryParse(txtReturnBookID.Text, out int bid) ? bid : 0;

            // Check if the user is logged in
            string memberEmail = Session["MemberEmail"]?.ToString();
            if (memberEmail == null)
            {
                lblReturnMessage.Text = "You are not logged in.";
                return;
            }

            // Get the Member ID for the logged-in user
            DataTable memberData = databaseHelper.dbRead($"SELECT userid FROM members WHERE email = '{memberEmail}';");
            if (memberData.Rows.Count == 0)
            {
                lblReturnMessage.Text = "Invalid user.";
                return;
            }
            int userID = Convert.ToInt32(memberData.Rows[0]["userid"]);

            // Check if the book is borrowed by the logged-in member
            DataTable borrowData = databaseHelper.dbRead($"SELECT * FROM borrow_transactions WHERE userid = {userID} AND bookid = {bookID} AND returndate IS NOT NULL;");
            if (borrowData.Rows.Count == 0)
            {
                // Check if the book is borrowed by someone else
                DataTable otherBorrowData = databaseHelper.dbRead($"SELECT * FROM borrow_transactions WHERE bookid = {bookID} AND returndate IS NOT NULL;");
                if (otherBorrowData.Rows.Count > 0)
                {
                    lblReturnMessage.Text = "The book is borrowed by someone else.";
                }
                else
                {
                    lblReturnMessage.Text = "No record found for this book.";
                }
                return;
            }

            // Check if the return is late
            DateTime dueDate = Convert.ToDateTime(borrowData.Rows[0]["returndate"]);
            if (DateTime.Now > dueDate)
            {
                int lateDays = (DateTime.Now - dueDate).Days;
                double fine = lateDays * 0.50;
                lblReturnMessage.Text = $"Book returned late by {lateDays} days. Fine: ${fine:F2}.";
            }
            else
            {
                lblReturnMessage.Text = "Book returned successfully.";
            }

            // Update the borrowing status of the book
            string updateQuery = $"UPDATE books SET borrowingstatus = 'f' WHERE bookid = {bookID};";
            databaseHelper.dbModify(updateQuery);

            // Mark the transaction as completed by removing it from the borrow_transactions table
            string transactionDeleteQuery = $"DELETE FROM borrow_transactions WHERE userid = {userID} AND bookid = {bookID};";
            databaseHelper.dbModify(transactionDeleteQuery);

            LoadAvailableBooks(); // Refresh the available books list
        }


        // Delete Transaction for Staff
        protected void gvBorrowTransactions_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            int transactionID = Convert.ToInt32(e.Keys["TransactionID"]);

            DataTable transactionData = databaseHelper.dbRead($"SELECT bookid FROM borrow_transactions WHERE transactionid = {transactionID};");
            if (transactionData.Rows.Count > 0)
            {
                int bookID = Convert.ToInt32(transactionData.Rows[0]["bookid"]);

                string deleteQuery = $"DELETE FROM borrow_transactions WHERE transactionid = {transactionID};";
                databaseHelper.dbModify(deleteQuery);

                string updateQuery = $"UPDATE books SET borrowingstatus = 'f' WHERE bookid = {bookID};";
                databaseHelper.dbModify(updateQuery);
            }

            loadTransactions();
            BindGrid();
        }

        protected void gvLateReturns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Paid")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvLateReturns.Rows[index];

                int userID = Convert.ToInt32(row.Cells[0].Text);
                int bookID = Convert.ToInt32(row.Cells[1].Text);

                try
                {
                    // Remove the late return record (simulating payment)
                    string deleteQuery = $"DELETE FROM borrow_transactions WHERE userid = {userID} AND bookid = {bookID};";
                    databaseHelper.dbModify(deleteQuery);

                    // Update book's borrowing status
                    string updateQuery = $"UPDATE books SET borrowingstatus = 'f' WHERE bookid = {bookID};";
                    databaseHelper.dbModify(updateQuery);

                    LoadLateReturns(); // Refresh the late returns grid
                    lblStaffMessage.Text = "Late return marked as paid successfully.";
                }
                catch (Exception ex)
                {
                    lblStaffMessage.Text = $"Error: {ex.Message}";
                }
            }
        }



        private void BindGrid()
        {
            gvBorrowTransactions.DataSource = transactions;
            gvBorrowTransactions.DataBind();
        }
    }

    // Borrow Transaction class to represent a single borrow record
    public class BorrowTransaction
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public int BookID { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
