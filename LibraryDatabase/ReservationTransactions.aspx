<%@ Page Title="Reservation Transactions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReservationTransactions.aspx.cs" Inherits="LibraryDatabase.ReservationTransactions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Reservation Transactions</h1>

    <!-- Content for Staff -->
    <asp:PlaceHolder ID="phStaffContent" runat="server" Visible="false">
        <!-- Reservation Transaction Input Form -->
        <h3>Add a New Reservation Transaction</h3>
        <div class="form-group">
            <label for="txtUserID">User ID:</label>
            <asp:TextBox ID="txtUserID" runat="server" CssClass="form-control" Placeholder="Enter User ID"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtBookID">Book ID:</label>
            <asp:TextBox ID="txtBookID" runat="server" CssClass="form-control" Placeholder="Enter Book ID"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtReservationDate">Reservation Date:</label>
            <asp:TextBox ID="txtReservationDate" runat="server" CssClass="form-control" Placeholder="Enter Date (YYYY-MM-DD)"></asp:TextBox>
        </div>
            <asp:Button ID="Button1" runat="server" Text="Reserve Book" CssClass="btn btn-primary" OnClick="btnReserveBook_Click" />
        <asp:Label ID="lblStaffMessage" runat="server" CssClass="text-danger"></asp:Label>

        <br />
        <br />

        <!-- Reservation Transactions List -->
        <h3>Reservation Transactions List</h3>
        <asp:GridView ID="gvReservations" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No reservations available." OnRowDeleting="gvReservations_RowDeleting" DataKeyNames="ReservationID">
            <Columns>
                <asp:BoundField DataField="ReservationID" HeaderText="Reservation ID" />
                <asp:BoundField DataField="UserID" HeaderText="User ID" />
                <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                <asp:BoundField DataField="ReservationDate" HeaderText="Reservation Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>

        <br />
        <h3>Reservation Priority List</h3>
        <asp:GridView ID="gvPriorityList" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No reservations in the priority list.">
            <Columns>
                <asp:BoundField DataField="BookID" HeaderText="Book ID" />
                <asp:BoundField DataField="UserID" HeaderText="User ID" />
                <asp:BoundField DataField="ReservationDate" HeaderText="Reservation Date" DataFormatString="{0:yyyy-MM-dd}" />
            </Columns>
        </asp:GridView>

    </asp:PlaceHolder>

   <!-- Content for Members -->
    <asp:PlaceHolder ID="phMemberContent" runat="server" Visible="false">
    <h3>Books List</h3>
    <asp:GridView ID="gvBooks" runat="server" CssClass="table table-striped" AutoGenerateColumns="True" EmptyDataText="No books available.">
    </asp:GridView>

    <br />
    <h3>Reserve a Book</h3>
    <div class="form-group">
        <label for="txtMemberBookID">Book ID:</label>
        <asp:TextBox ID="txtMemberBookID" runat="server" CssClass="form-control" Placeholder="Enter Book ID"></asp:TextBox>
    </div>
    <asp:Button ID="btnReserveBook" runat="server" Text="Reserve Book" CssClass="btn btn-primary" OnClick="btnReserveBook_Click" />
    <asp:Label ID="lblReservationMessage" runat="server" CssClass="text-danger"></asp:Label>
</asp:PlaceHolder>
</asp:Content>
