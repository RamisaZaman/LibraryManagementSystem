<%@ Page Title="Library Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LibraryDatabase._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h1>Library Management System</h1>

    <!-- Book Input Form -->
    <h3>Add a New Book</h3>
    <div class="form-group">
        <label for="txtTitle">Title:</label>
        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" Placeholder="Enter Title"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtAuthor">Author:</label>
        <asp:TextBox ID="txtAuthor" runat="server" CssClass="form-control" Placeholder="Enter Author"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtYear">Year:</label>
        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" Placeholder="Enter Year"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtGenre">Genre:</label>
        <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control" Placeholder="Enter Genre"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddBook" runat="server" Text="Add Book" CssClass="btn btn-primary" OnClick="btnAddBook_Click" />

    <br />
    <br />

    <!-- Book Display Grid -->
    <h3>Books List</h3>
    <asp:GridView ID="gvBooks" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No books available." OnRowDeleting="gvBooks_RowDeleting" DataKeyNames="bookid">
        <Columns>
            <asp:BoundField DataField="bookid" HeaderText="Book ID" visible="false"/>
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Author" HeaderText="Author" />
            <asp:BoundField DataField="Year" HeaderText="Year" />
            <asp:BoundField DataField="Genre" HeaderText="Genre" />
            <asp:BoundField DataField="borrowingStatus" HeaderText="Borrowing Status" visible="false"/>
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
