<%@ Page Title="Borrow Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BorrowTransactions.aspx.cs" Inherits="LibraryDatabase.BorrowTransactions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Borrow Transactions</h1>

    <!-- Content for Staff -->
    <asp:PlaceHolder ID="phStaffContent" runat="server" Visible="false">
        <h3>Add a New Borrow Transaction</h3>
        <div class="form-group">
            <label for="txtUserID">User ID:</label>
            <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control" Placeholder="Enter User ID"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtBookID">Book ID:</label>
            <asp:TextBox ID="txtBookID" runat="server" CssClass="form-control" Placeholder="Enter Book ID"></asp:TextBox>
        </div>

        <!-- Staff-Only Fields -->
        <asp:PlaceHolder ID="phStaffFields" runat="server" Visible="false">
            <div class="form-group">
                <label for="txtBorrowDate">Borrow Date:</label>
                <asp:TextBox ID="txtBorrowDate" runat="server" CssClass="form-control" Placeholder="Enter Date (YYYY-MM-DD)"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtReturnDate">Return Date:</label>
                <asp:TextBox ID="txtReturnDate" runat="server" CssClass="form-control" Placeholder="Enter Date (YYYY-MM-DD, optional)"></asp:TextBox>
            </div>
        </asp:PlaceHolder>

        <asp:Button ID="btnAddTransaction" runat="server" Text="Add Transaction" CssClass="btn btn-primary" OnClick="btnAddTransaction_Click" />
        <asp:Label ID="lblStaffMessage" runat="server" CssClass="text-danger"></asp:Label>

        <hr />

        <h3>Borrow Transactions List</h3>
        <asp:GridView ID="gvBorrowTransactions" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No transactions available." OnRowDeleting="gvBorrowTransactions_RowDeleting" DataKeyNames="TransactionID">
            <Columns>
                <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                <asp:BoundField DataField="UserID" HeaderText="User ID" />
                <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                <asp:BoundField DataField="BorrowDate" HeaderText="Borrow Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="ReturnDate" HeaderText="Return Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <h3>Late Return List</h3>
        <asp:GridView ID="gvLateReturns" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No late returns available.">
            <Columns>
                <asp:BoundField DataField="UserID" HeaderText="User ID" />
                <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                <asp:BoundField DataField="LateFee" HeaderText="Late Fee ($)" DataFormatString="{0:0.00}" />
                <asp:ButtonField ButtonType="Button" CommandName="Paid" Text="Paid" />

            </Columns>
        </asp:GridView>
    </asp:PlaceHolder>

    <!-- Content for Members -->
    <asp:PlaceHolder ID="phMemberContent" runat="server" Visible="false">
        <h3>Available Books</h3>
        <asp:GridView ID="gvAvailableBooks" runat="server" CssClass="table table-striped" AutoGenerateColumns="True" EmptyDataText="No available books." />

        <hr />

        <h3>Borrow a Book</h3>
        <div class="form-group">
            <label for="txtMemberBookID">Book ID:</label>
            <asp:TextBox ID="txtMemberBookID" runat="server" CssClass="form-control" Placeholder="Enter Book ID"></asp:TextBox>
        </div>
        <asp:Button ID="btnBorrowBook" runat="server" Text="Borrow Book" CssClass="btn btn-primary" OnClick="btnBorrowBook_Click" />
        <asp:Label ID="lblMemberMessage" runat="server" CssClass="text-danger"></asp:Label>

        <h3>Return a Book</h3>
        <div class="form-group">
            <label for="txtReturnBookID">Book ID:</label>
            <asp:TextBox ID="txtReturnBookID" runat="server" CssClass="form-control" Placeholder="Enter Book ID"></asp:TextBox>
        </div>
        <asp:Button ID="btnReturnBook" runat="server" Text="Return Book" CssClass="btn btn-primary" OnClick="btnReturnBook_Click" />
        <asp:Label ID="lblReturnMessage" runat="server" CssClass="text-danger"></asp:Label>
    </asp:PlaceHolder>
</asp:Content>
