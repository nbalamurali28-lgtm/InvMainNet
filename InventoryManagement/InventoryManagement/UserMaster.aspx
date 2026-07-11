<%@ Page Title="User Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="InventoryManagement.UserMaster" %>

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

        <div class="container">
            <div class="row">
                <div class="col-4">
                    <div class="form-group">
                        <label>Username *</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Unique username"></asp:TextBox>
                    </div>
                </div>
                <div class="col-4">
                    <div class="form-group">
                        <label>Password *</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="col-4">
                    <div class="form-group">
                        <label>Role *</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-4">
                    <div class="form-group">
                        <asp:Label ID="LblEmployee" runat="server" Text="Employee ID:" class="form-label"></asp:Label>
                        <asp:DropDownList ID="DdlEmployeeId" runat="server" AutoPostBack="false" EnableViewState="true" CssClass="form-select form-select-sm" TabIndex="2">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorDdlEmployeeId" runat="server" ControlToValidate="DdlEmployeeId" InitialValue="" ErrorMessage="Employee ID is required." CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
                <div class="col-4">
                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    </div>
                </div>
                <div class="col-4">
                    <div class="form-group">
                        <label>Status</label>
                        <div class="form-check">
                            <asp:CheckBox ID="chkIsActive" runat="server" class="form-check-input" Checked="true" />
                            <label>Active</label>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row p-2">
                <div class="col-4">
                </div>
                <div class="col-4" style="text-align: center;">
                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-sm" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-secondary btn-sm" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                </div>
                <div class="col-4">
                </div>
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3" Style="display: none;"></asp:Label>
            <asp:HiddenField ID="hfUserId" runat="server" />
        </div>

        <div class="table-responsive">
            <asp:GridView ID="grdUserMaster" runat="server" AutoGenerateColumns="false"
                DataKeyNames="user_id" OnRowDataBound="grdUserMaster_RowDataBound"
                OnRowCommand="grdUserMaster_RowCommand"
                OnRowDeleting="grdUserMaster_RowDeleting" CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:BoundField DataField="user_id" HeaderText="User ID" ReadOnly="true" HeaderStyle-CssClass="bg-primary text-white" />
                    <%--<asp:BoundField DataField="username" HeaderText="Username" />--%>
                    <asp:BoundField DataField="username" HeaderText="Username" ReadOnly="true" HeaderStyle-CssClass="bg-primary text-white" />
                    <%--<asp:BoundField DataField="employee_id" HeaderText="Employee Id" />--%>
                    <asp:BoundField DataField="employee_id" HeaderText="Employee Id" ReadOnly="true" HeaderStyle-CssClass="bg-primary text-white" />
                    <asp:BoundField DataField="role_name" HeaderText="Role" ReadOnly="true" HeaderStyle-CssClass="bg-primary text-white" />
                    <asp:BoundField DataField="email" HeaderText="Email" ReadOnly="true" HeaderStyle-CssClass="bg-primary text-white" />
                    <asp:TemplateField HeaderText="Is Active" HeaderStyle-CssClass="bg-primary text-white" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <div class="form-check">
                                <input id="GrdChkBoxIsActive" runat="server" class="form-check-input" type="checkbox" value="" disabled="disabled">
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("user_id") %>' Text="Edit" CssClass="btn btn-sm btn-primary me-1" />
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CssClass="btn btn-sm btn-danger" OnClientClick="return confirm('Are you sure you want to delete this record?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
    </main>
</asp:Content>
