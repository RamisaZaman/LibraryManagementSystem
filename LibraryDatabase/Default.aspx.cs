using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;

namespace LibraryDatabase
{
    public partial class _Default : System.Web.UI.Page
    {
        // Static list to store books (simulating a database)
        private static List<Book> books = new List<Book>();

        public void loadBooks() {
            books.Clear();
            DataTable data = databaseHelper.dbRead("SELECT * FROM books;");
            foreach (DataRow row in data.Rows)
            {
                var book = new Book
                {
                    BookId = Convert.ToInt32(row["bookid"]),
                    Title = Convert.ToString(row["title"]),
                    Author = Convert.ToString(row["author"]),
                    Year = Convert.ToInt32(row["publishingyear"]),
                    Genre = Convert.ToString(row["genre"]),
                    BorrowingStatus = Convert.ToBoolean(row["borrowingstatus"])
                };
                books.Add(book);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Staff")
            {
                Response.Redirect("~/Home.aspx"); // Redirect to login if not logged in as staff
            }
        

            if (!IsPostBack)
            {
                loadBooks();
                // Load the book list into the GridView on the first page load
                BindGrid();
            }
        }

        protected void btnAddBook_Click(object sender, EventArgs e)
        {
            // Add a new book to the list
            var newBook = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Year = int.TryParse(txtYear.Text, out int year) ? year : 0,
                Genre = txtGenre.Text,
                BorrowingStatus = false
            };

            string query = $"INSERT INTO Books (Title, Author, Genre, PublishingYear, BorrowingStatus) VALUES  ('{newBook.Title}', '{newBook.Author}', '{newBook.Genre}', {newBook.Year}, FALSE)";
            databaseHelper.dbModify(query);

            books.Add(newBook);

            // Clear the form fields after adding
            txtTitle.Text = string.Empty;
            txtAuthor.Text = string.Empty;
            txtYear.Text = string.Empty;
            txtGenre.Text = string.Empty;

            // Rebind the GridView to reflect the new book
            loadBooks();
             BindGrid();
      
        }

        protected void gvBooks_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            var bookid = e.Keys["bookid"];

            // CHECKING THE BORROW TRANSACTIONS TO DELETE DEPENDENCIES
            DataTable data = databaseHelper.dbRead($"SELECT * FROM borrow_transactions WHERE bookid={bookid};");
            if (data.Rows.Count != 0)       // IF THE BOOK HAS BEEN RETURNED
            {
                foreach (DataRow row in data.Rows) {            // Iterate through all entries
                    databaseHelper.dbModify($"DELETE FROM borrow_transactions WHERE bookid={bookid};");
                }

            }
            
            // CHECKING THE RESERVATION TRANSCATIONS TO DELETE DEPENDENCIES 
            DataTable dt = databaseHelper.dbRead($"SELECT * FROM reservation_transactions WHERE bookid={bookid};");
            if (dt.Rows.Count != 0) { 
                foreach (DataRow row in dt.Rows)
                {
                    databaseHelper.dbModify($"DELETE FROM reservation_transactions WHERE bookid={bookid}");
                }
            }
            
            databaseHelper.dbModify($"DELETE FROM books WHERE bookid={bookid};");       // DELETING THE DESIRED BOOK

            // Get the index of the book to delete
            books.RemoveAt(e.RowIndex);

            // Rebind the GridView after deletion
            loadBooks();
            BindGrid();
        }

        private void BindGrid()
        {
            // Bind the books list to the GridView
            gvBooks.DataSource = books;
            gvBooks.DataBind();
        }
    }

    // Book class to represent a single book entity
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public bool BorrowingStatus { get; set; }
    }
}
