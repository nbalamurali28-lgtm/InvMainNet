<%@ Page Title="Product Category Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryMaster.aspx.cs" Inherits="InventoryManagement.CategoryMaster" %>

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


        <div class="row">
            <div class="col-md-6">
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Add/Edit Category</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtCategoryName">Category Name *</label>
                            <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Enter category name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName"
                                CssClass="text-danger" ErrorMessage="Category Name is required"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group mt-3">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnReset" runat="server" CssClass="btn btn-secondary" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        </div>

                        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3" Style="display: none;"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-secondary text-white">
                        <h5 class="mb-0">Category List</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdCategoryMaster" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="false" DataKeyNames="category_id" OnRowEditing="grdCategoryMaster_RowEditing"
                            OnRowUpdating="grdCategoryMaster_RowUpdating" OnRowCancelingEdit="grdCategoryMaster_RowCancelingEdit"
                            OnRowDeleting="grdCategoryMaster_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="category_id" HeaderText="Category ID" ReadOnly="true" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="category_name" HeaderText="Category Name" />
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100px" />
                            </Columns>
                            <EditRowStyle BackColor="#FFE699" />
                            <EmptyDataTemplate>
                                <p class="text-center text-muted">No categories found.</p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </main>
        
</asp:Content>
