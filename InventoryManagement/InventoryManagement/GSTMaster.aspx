<%@ Page Title="GST Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GSTMaster.aspx.cs" Inherits="InventoryManagement.GSTMaster" %>

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
                        <h5 class="mb-0">GST Information</h5>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtGSTPercentage">GST Percentage (%) *</label>
                            <asp:TextBox ID="txtGSTPercentage" runat="server" CssClass="form-control" placeholder="e.g., 5, 12, 18" TextMode="Number" Step="0.01"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvGST" runat="server" ControlToValidate="txtGSTPercentage"
                                CssClass="text-danger" ErrorMessage="GST Percentage is required"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtDescription">Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="e.g., Standard Rate, Reduced Rate"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="dtEffectiveFrom">Effective From</label>
                                    <asp:TextBox ID="dtEffectiveFrom" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="dtEffectiveTo">Effective To</label>
                                    <asp:TextBox ID="dtEffectiveTo" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="form-check">
                                <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label" for="chkIsActive">Active</label>
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
                        <h5 class="mb-0">GST List</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="grdGSTMaster" runat="server" CssClass="table table-striped table-hover"
                            AutoGenerateColumns="false" DataKeyNames="gst_id" OnRowEditing="grdGSTMaster_RowEditing"
                            OnRowUpdating="grdGSTMaster_RowUpdating" OnRowCancelingEdit="grdGSTMaster_RowCancelingEdit"
                            OnRowDeleting="grdGSTMaster_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="gst_id" HeaderText="GST ID" ReadOnly="true" ItemStyle-Width="70px" />
                                <asp:BoundField DataField="gst_percentage" HeaderText="Percentage (%)" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="description" HeaderText="Description" />
                                <asp:BoundField DataField="is_active" HeaderText="Active" DataFormatString="{0:Yes;No}" ItemStyle-Width="60px" />
                                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" ItemStyle-Width="100px" />
                            </Columns>
                            <EditRowStyle BackColor="#FFE699" />
                            <EmptyDataTemplate>
                                <p class="text-center text-muted">No GST records found.</p>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </main>
</asp:Content>
