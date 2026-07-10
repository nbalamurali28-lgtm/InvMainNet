<%@ Page Title="Product-GST Mapping" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductGSTMapping.aspx.cs" Inherits="InventoryManagement.ProductGSTMapping" %>

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
        
        
        <div class="row mb-4">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Assign GST to Product</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Product *</label>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>GST Rate *</label>
                                    <asp:DropDownList ID="ddlGST" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Assign GST" OnClick="btnSave_Click" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3" Style="display: none;"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-secondary text-white">
                        <h5 class="mb-0">Current Mappings</h5>
                    </div>
                    <div class="card-body table-responsive">
                        <asp:GridView ID="grdMapping" runat="server" CssClass="table table-striped"
                            AutoGenerateColumns="false" DataKeyNames="ProductID" OnRowDeleting="grdMapping_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="ProductID" HeaderText="Product ID" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                <asp:BoundField DataField="gst_percentage" HeaderText="GST %" DataFormatString="{0:N2}" />
                                <asp:CommandField ShowDeleteButton="true" HeaderText="Action" ItemStyle-Width="80px" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        <</main>
</asp:Content>
