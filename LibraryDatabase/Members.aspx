<%@ Page Title="Manage Members" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Members.aspx.cs" Inherits="LibraryDatabase.Members" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Manage Members</h1>

    <!-- Member Input Form -->
    <h3>Add a New Member</h3>
    <div class="form-group">
        <label for="txtName">Name:</label>
        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Placeholder="Enter Name"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtPhoneNumber">Phone Number:</label>
        <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" Placeholder="Enter Phone Number"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtEmail">Email:</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Enter Email"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtMembershipDate">Membership Date:</label>
        <asp:TextBox ID="txtMembershipDate" runat="server" CssClass="form-control" Placeholder="Enter Date (YYYY-MM-DD)"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddMember" runat="server" Text="Add Member" CssClass="btn btn-primary" OnClick="btnAddMember_Click" />
     <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    <br />
    <br />

    <!-- Member List Grid -->
    <h3>Members List</h3>
    <asp:GridView ID="gvMembers" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No members available." OnRowDeleting="gvMembers_RowDeleting" DataKeyNames="userID">
        <Columns>
            <asp:BoundField DataField="UserID" HeaderText="User ID" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
            <asp:BoundField DataField="Email" HeaderText="Email" />
            <asp:BoundField DataField="MembershipDate" HeaderText="Membership Date" DataFormatString="{0:yyyy-MM-dd}" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>

     <!-- Staff Account Input Form -->
    <h3>Add a New Staff</h3>
    <div class="form-group">
        <label for="txtStaffName">Name:</label>
        <asp:TextBox ID="txtStaffName" runat="server" CssClass="form-control" Placeholder="Enter Staff Name"></asp:TextBox>
    </div>
    <div class="form-group">
        <label for="txtRole">Role:</label>
        <asp:TextBox ID="txtRole" runat="server" CssClass="form-control" Placeholder="Enter Staff Role"></asp:TextBox>
    </div>
    <asp:Button ID="btnAddStaff" runat="server" Text="Add Staff" CssClass="btn btn-primary" OnClick="btnAddStaff_Click" />
    <asp:Label ID="lblStaffMessage" runat="server" Text=""></asp:Label>

    <br />
    <br />

    <!-- Staff List Grid -->
    <h3>Staff List</h3>
    <asp:GridView ID="gvStaff" runat="server" CssClass="table table-striped" AutoGenerateColumns="False" EmptyDataText="No staff available." OnRowDeleting="gvStaff_RowDeleting" DataKeyNames="StaffID">
        <Columns>
            <asp:BoundField DataField="StaffID" HeaderText="Staff ID" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Role" HeaderText="Role" />
            <asp:CommandField ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
