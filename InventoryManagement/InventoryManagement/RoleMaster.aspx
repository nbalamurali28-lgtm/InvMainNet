<%@ Page Title="Role Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleMaster.aspx.cs" Inherits="InventoryManagement.RoleMaster" %>

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
                        <h5 class="mb-0">Role Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label>Role ID (e.g., ROLE_ADMIN) *</label>
                            <asp:TextBox ID="txtRoleId" runat="server" CssClass="form-control" placeholder="ROLE_"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Role Name *</label>
                            <asp:TextBox ID="txtRoleName" runat="server" CssClass="form-control" placeholder="e.g., Administrator"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label>Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <div class="form-check">
                                <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label">Active</label>
                            </div>
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
                        <h5 class="mb-0">Role List</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdRoleMaster" runat="server" CssClass="table table-striped"
                            AutoGenerateColumns="false" DataKeyNames="role_id" OnRowEditing="grdRoleMaster_RowEditing"
                            OnRowUpdating="grdRoleMaster_RowUpdating" OnRowCancelingEdit="grdRoleMaster_RowCancelingEdit"
                            OnRowDeleting="grdRoleMaster_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="role_id" HeaderText="Role ID" ReadOnly="true" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="role_name" HeaderText="Role Name" />
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100px" />
                            </Columns>
                            <EditRowStyle BackColor="#FFE699" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </main>
</asp:Content>
