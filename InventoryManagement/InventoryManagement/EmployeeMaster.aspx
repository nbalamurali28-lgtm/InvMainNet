<%@ Page Title="Employee Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmployeeMaster.aspx.cs" Inherits="InventoryManagement.EmployeeMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <!-- Load jQuery first -->
<script src="Scripts/jquery-3.7.0.min.js"></script>
<!-- Load Popper.js (required for Bootstrap dropdowns) -->
<script src="Scripts/popper.min.js"></script>
<!-- Load Bootstrap JavaScript -->
<script src="Scripts/bootstrap.bundle.min.js"></script>
        <style type="text/css">
        .pageBody {
            width: 98%;
            margin: 1% 0% 0% 1%;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

    <div class="container mt-4">
        

        <!-- Form Section for Adding/Editing Employees -->
        <div class="row">
            <div class="col-md-8">
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Employee Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtEmployeeName">Employee Name *</label>
                            <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="form-control" placeholder="Enter employee name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" ControlToValidate="txtEmployeeName"
                                CssClass="text-danger" ErrorMessage="Employee Name is required"></asp:RequiredFieldValidator>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtDesignation">Designation</label>
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" placeholder="e.g., Manager, Developer"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtDepartment">Department</label>
                                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" placeholder="e.g., Sales, IT"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtMobileNo">Mobile No.</label>
                                    <asp:TextBox ID="txtMobileNo" runat="server" CssClass="form-control" placeholder="10-digit mobile number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtEmailId">Email</label>
                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" placeholder="Email address" TextMode="Email"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtAddress">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Enter address"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtCity">City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtState">State</label>
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtPincode">Pincode</label>
                                    <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="6-digit pincode"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="dtJoinDate">Join Date</label>
                                    <asp:TextBox ID="dtJoinDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtSalary">Salary</label>
                            <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" placeholder="0.00" TextMode="Number" Step="0.01"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="ddlStatus">Status *</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="ACTIVE">Active</asp:ListItem>
                                <asp:ListItem Value="INACTIVE">Inactive</asp:ListItem>
                                <asp:ListItem Value="ONLEAVE">On Leave</asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <div class="form-group mt-3">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-secondary" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        </div>

                        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3" Style="display: none;"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- GridView Section for Displaying Employees -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-secondary text-white">
                        <h5 class="mb-0">Employee List</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdEmployeeMaster" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="false" DataKeyNames="emp_code" OnRowEditing="grdEmployeeMaster_RowEditing"
                            OnRowUpdating="grdEmployeeMaster_RowUpdating" OnRowCancelingEdit="grdEmployeeMaster_RowCancelingEdit"
                            OnRowDeleting="grdEmployeeMaster_RowDeleting" OnRowDataBound="grdEmployeeMaster_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="emp_code" HeaderText="Emp Code" ReadOnly="true" ItemStyle-Width="80px" />
                                <asp:BoundField DataField="emp_name" HeaderText="Employee Name" />
                                <asp:BoundField DataField="designation" HeaderText="Designation" />
                                <asp:BoundField DataField="department" HeaderText="Department" />
                                <asp:BoundField DataField="mobile_no" HeaderText="Mobile" />
                                <asp:BoundField DataField="email_id" HeaderText="Email" />
                                <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-Width="80px" />
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="120px" />
                            </Columns>
                            <EditRowStyle BackColor="#FFE699" />
                            <EmptyDataTemplate>
                                <p class="text-center text-muted">No employees found.</p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </main>
</asp:Content>
