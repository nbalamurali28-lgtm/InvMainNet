<%@ Page Title="Pricing Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PricingMaster.aspx.cs" Inherits="InventoryManagement.PricingMaster" %>

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
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Add Pricing</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Product *</label>
                                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>Base Price *</label>
                                    <asp:TextBox ID="txtBasePrice" runat="server" CssClass="form-control" TextMode="Number" Step="0.01"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>GST *</label>
                                    <asp:DropDownList ID="ddlGST" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>To Date</label>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary form-control" Text="Save" OnClick="btnSave_Click" />
                                </div>
                            </div>
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
                        <h5 class="mb-0">Pricing List</h5>
                    </div>
                    <div class="card-body table-responsive">
                        <asp:GridView ID="grdPricingMaster" runat="server" CssClass="table table-striped"
                            AutoGenerateColumns="false" DataKeyNames="pricing_id" OnRowEditing="grdPricingMaster_RowEditing"
                            OnRowUpdating="grdPricingMaster_RowUpdating" OnRowCancelingEdit="grdPricingMaster_RowCancelingEdit"
                            OnRowDeleting="grdPricingMaster_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="pricing_id" HeaderText="ID" ReadOnly="true" />
                                <asp:BoundField DataField="ProductName" HeaderText="Product" />
                                <asp:BoundField DataField="base_price" HeaderText="Base Price" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="gst_percentage" HeaderText="GST %" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="effective_from" HeaderText="From" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="effective_to" HeaderText="To" DataFormatString="{0:yyyy-MM-dd}" />
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
