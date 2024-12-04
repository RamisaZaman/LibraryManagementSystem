<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="LibraryDatabase.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Welcome to the Library Management System</h1>
        <p class="lead">Manage books, members, reservations, and transactions with ease.</p>
        <hr class="my-4">

        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />


        <!-- Staff Login Section -->
        <h3>Staff Login</h3>
        <div class="form-group">
            <label for="txtStaffID">Staff ID:</label>
            <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control" Placeholder="Enter Staff ID"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtStaffName">Name:</label>
            <asp:TextBox ID="txtStaffName" runat="server" CssClass="form-control" Placeholder="Enter Name"></asp:TextBox>
        </div>
        <asp:Button ID="btnLoginStaff" runat="server" Text="Login as Staff" CssClass="btn btn-primary" OnClick="btnLoginStaff_Click" />
        <asp:Label ID="lblMessageStaff" runat="server" CssClass="text-danger"></asp:Label>

        <hr />

        <!-- Member Login Section -->
        <h3>Member Login</h3>
        <div class="form-group">
            <label for="txtMemberEmail">Email:</label>
            <asp:TextBox ID="txtMemberEmail" runat="server" CssClass="form-control" Placeholder="Enter Email"></asp:TextBox>
        </div>
        <asp:Button ID="btnLoginMember" runat="server" Text="Login as Member" CssClass="btn btn-success" OnClick="btnLoginMember_Click" />
        <asp:Label ID="lblMessageMember" runat="server" CssClass="text-danger"></asp:Label>

        <hr />

        <!-- Create Member Account Section -->
        <h3>&nbsp;</h3>
        <h3>&nbsp;</h3>
        <h3>Create a Member Account</h3>
        <div class="form-group">
            <label for="txtNewMemberName">Name:</label>
            <asp:TextBox ID="txtNewMemberName" runat="server" CssClass="form-control" Placeholder="Enter Name"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtNewMemberPhone">Phone Number:</label>
            <asp:TextBox ID="txtNewMemberPhone" runat="server" CssClass="form-control" Placeholder="Enter Phone Number"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtNewMemberEmail">Email:</label>
            <asp:TextBox ID="txtNewMemberEmail" runat="server" CssClass="form-control" Placeholder="Enter Email"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="txtNewMembershipDate">Membership Date:</label>
            <asp:TextBox ID="txtNewMembershipDate" runat="server" CssClass="form-control" Placeholder="Enter Date (YYYY-MM-DD)"></asp:TextBox>
        </div>
        <asp:Button ID="btnCreateMemberAccount" runat="server" Text="Create Member Account" CssClass="btn btn-info" OnClick="btnCreateMemberAccount_Click" />
        <asp:Label ID="lblMessageCreateMember" runat="server" CssClass="text-success"></asp:Label>
    </div>
</asp:Content>
