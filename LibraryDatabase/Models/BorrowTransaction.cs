using System;

public class BorrowTransaction
{
    public int TransactionID { get; set; }
    public int UserID { get; set; }
    public int BookID { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}
