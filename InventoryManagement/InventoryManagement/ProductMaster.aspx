<%@ Page Title="Product Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductMaster.aspx.cs" Inherits="InventoryManagement.ProductMaster" %>

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
        

        <!-- Form Section for Adding/Editing Products -->
        <div class="row">
            <div class="col-md-8">
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Product Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtProductName">Product Name *</label>
                            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Enter product name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName"
                                CssClass="text-danger" ErrorMessage="Product Name is required"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="ddlCategory">Category *</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">-- Select Category --</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory"
                                CssClass="text-danger" ErrorMessage="Category is required" InitialValue=""></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtDescription">Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter description"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtUnitPrice">Unit Price *</label>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="form-control" placeholder="0.00" TextMode="Number" Step="0.01"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                        CssClass="text-danger" ErrorMessage="Unit Price is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtStockQuantity">Stock Quantity *</label>
                                    <asp:TextBox ID="txtStockQuantity" runat="server" CssClass="form-control" placeholder="0" TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvStockQuantity" runat="server" ControlToValidate="txtStockQuantity"
                                        CssClass="text-danger" ErrorMessage="Stock Quantity is required"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtReorderLevel">Reorder Level</label>
                                    <asp:TextBox ID="txtReorderLevel" runat="server" CssClass="form-control" placeholder="0" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtWeight">Weight</label>
                                    <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" placeholder="0.00" TextMode="Number" Step="0.01"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="txtGasType">Gas Type</label>
                            <asp:TextBox ID="txtGasType" runat="server" CssClass="form-control" placeholder="Enter gas type if applicable"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="chkIsActive">Active</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkAvailableOutDelivery" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkAvailableOutDelivery">Available for Out Delivery</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <asp:CheckBox ID="chkFlexiPrice" runat="server" CssClass="form-check-input" />
                                <label class="form-check-label" for="chkFlexiPrice">Flexi Price</label>
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
        </div>

        <!-- GridView Section for Displaying Products -->
        <div class="row mt-4">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-secondary text-white">
                        <h5 class="mb-0">Product List</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdProductMaster" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="false" DataKeyNames="ProductID" OnRowEditing="grdProductMaster_RowEditing"
                            OnRowUpdating="grdProductMaster_RowUpdating" OnRowCancelingEdit="grdProductMaster_RowCancelingEdit"
                            OnRowDeleting="grdProductMaster_RowDeleting" OnRowDataBound="grdProductMaster_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="ProductID" HeaderText="Product ID" ReadOnly="true" ItemStyle-Width="80px" />

                                <asp:TemplateField HeaderText="Product Name">
                                    <ItemTemplate>
                                        <%# Eval("ProductName") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtProductNameEdit" runat="server" Text='<%# Bind("ProductName") %>' CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="category_name" HeaderText="Category" ReadOnly="true" />

                                <asp:TemplateField HeaderText="Unit Price">
                                    <ItemTemplate>
                                        <%# Eval("UnitPrice", "{0:N2}") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUnitPriceEdit" runat="server" Text='<%# Bind("UnitPrice") %>' CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Stock Qty">
                                    <ItemTemplate>
                                        <%# Eval("StockQuantity") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtStockQuantityEdit" runat="server" Text='<%# Bind("StockQuantity") %>' CssClass="form-control" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Active" ItemStyle-Width="60px">
                                    <ItemTemplate>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "Yes" : "No" %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkIsActiveEdit" runat="server" Checked='<%# Bind("IsActive") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="120px" />
                            </Columns>
                            <EditRowStyle BackColor="#FFE699" />
                            <EmptyDataTemplate>
                                <p class="text-center text-muted">No products found.</p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </main>
</asp:Content>
