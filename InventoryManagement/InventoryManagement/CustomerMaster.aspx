<%@ Page Title="Customer Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerMaster.aspx.cs" Inherits="InventoryManagement.CustomerMaster" %>

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

        .validator-message {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.25rem;
            display: block;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="table-responsive">
            <asp:GridView ID="grdCustomerMaster" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                AllowSorting="true" AllowPaging="true" PageSize="6" DataKeyNames="customer_id"
                OnRowEditing="grdCustomerMaster_RowEditing" OnRowUpdating="grdCustomerMaster_RowUpdating"
                OnRowCancelingEdit="grdCustomerMaster_RowCancelingEdit" OnRowDeleting="grdCustomerMaster_RowDeleting"
                OnRowDataBound="grdCustomerMaster_RowDataBound" OnPageIndexChanging="grdCustomerMaster_PageIndexChanging"
                CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Customer Id" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("customer_id") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblGrdCustomerId" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("customer_id") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer Name" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("customer_name") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtCustomerName" runat="server" class="form-control form-control-sm" Text='<%# Bind("customer_name") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Person" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("contact_person") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtContactPerson" runat="server" class="form-control form-control-sm" Text='<%# Bind("contact_person") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Phone" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("phone") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtPhone" runat="server" class="form-control form-control-sm" Text='<%# Bind("phone") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("email") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtEmail" runat="server" class="form-control form-control-sm" Text='<%# Bind("email") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GST" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("gst_number") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtGSTNumber" runat="server" class="form-control form-control-sm" Text='<%# Bind("gst_number") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Area" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Eval("area_name") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="grdDdlArea" runat="server" class="form-select form-select-sm"></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate><%# Convert.ToBoolean(Eval("is_active")) ? "Active" : "Inactive" %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="grdDdlStatus" runat="server" class="form-select form-select-sm">
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" HeaderStyle-CssClass="bg-primary text-white" />
                </Columns>
                <PagerSettings Visible="true" Mode="NumericFirstLast" Position="Bottom" />
                <PagerStyle CssClass="table-secondary text-white" HorizontalAlign="Right" />
            </asp:GridView>
        </div>

        <div class="container">
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblCustomerId" runat="server" Text="Customer Id" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:Label ID="lblCustomerIdValue" runat="server" CssClass="form-control form-control-sm"></asp:Label></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblCustomerName" runat="server" Text="Customer Name" class="form-label"></asp:Label></div>
                <div class="col-4">
                    <asp:TextBox ID="txtCustomerName" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCustomerName" runat="server"
                        ControlToValidate="txtCustomerName"
                        Display="Dynamic"
                        CssClass="validator-message"
                        ErrorMessage="Customer Name is required."
                        ValidationGroup="Save" />
                </div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblContactPerson" runat="server" Text="Contact Person" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:TextBox ID="txtContactPerson" runat="server" class="form-control form-control-sm"></asp:TextBox></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblPhone" runat="server" Text="Phone" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:TextBox ID="txtPhone" runat="server" class="form-control form-control-sm"></asp:TextBox></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblEmail" runat="server" Text="Email" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:TextBox ID="txtEmail" runat="server" class="form-control form-control-sm"></asp:TextBox></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblGSTNumber" runat="server" Text="GST Number" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:TextBox ID="txtGSTNumber" runat="server" class="form-control form-control-sm"></asp:TextBox></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblArea" runat="server" Text="Area" class="form-label"></asp:Label></div>
                <div class="col-4">
                    <asp:DropDownList ID="ddlArea" runat="server" class="form-select form-select-sm"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvArea" runat="server"
                        ControlToValidate="ddlArea"
                        InitialValue=""
                        Display="Dynamic"
                        CssClass="validator-message"
                        ErrorMessage="Please select a valid Area."
                        ValidationGroup="Save" />
                </div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblStatus" runat="server" Text="Status" class="form-label"></asp:Label></div>
                <div class="col-4">
                    <asp:DropDownList ID="ddlStatus" runat="server" class="form-select form-select-sm">
                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"><asp:Label ID="lblAddress" runat="server" Text="Address" class="form-label"></asp:Label></div>
                <div class="col-4"><asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" class="form-control form-control-sm"></asp:TextBox></div>
                <div class="col-6"></div>
            </div>
            <div class="row p-2">
                <div class="col-2"></div>
                <div class="col-4">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Save" class="btn btn-primary btn-sm" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-secondary btn-sm" />
                </div>
                <div class="col-6"></div>
            </div>
        </div>

        <asp:HiddenField ID="HFieldTransactionType" runat="server" />
    </main>
</asp:Content>
