<%@ Page Title="Area Master" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AreaMaster.aspx.cs" Inherits="InventoryManagement.AreaMaster" %>

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

        <div class="table-responsive">
            <asp:GridView ID="grdAreaMaster" runat="server" AutoGenerateColumns="false" EnableViewState="true"
                AllowSorting="true" AllowPaging="true" DataKeyNames="area_id,area_name"
                OnRowEditing="grdAreaMaster_RowEditing" OnRowUpdating="grdAreaMaster_RowUpdating"
                OnRowCancelingEdit="grdAreaMaster_RowCancelingEdit" OnRowDeleting="grdAreaMaster_RowDeleting"
                OnRowDataBound="grdAreaMaster_RowDataBound" CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Area Id" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("area_id") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtAreaId" runat="server" class="form-control form-control-sm" Text='<%# Bind("area_id") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Area Name" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("area_name") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtAreaName" runat="server" class="form-control form-control-sm" Text='<%# Bind("area_name") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Zip Code" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <%# Eval("ZIPCODE") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="grdTxtZipCode" runat="server" class="form-control form-control-sm" Text='<%# Bind("ZIPCODE") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="true" HeaderStyle-CssClass="bg-primary text-white" />
                </Columns>
            </asp:GridView>
            <hr />
        </div>

        <div class="container">
            <div class="row p-2">
                <div class="col-2">
                    <asp:Label ID="lblAreaId" runat="server" Text="Area Id" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtAreaId" runat="server" class="form-control form-control-sm"></asp:TextBox>
                </div>
                <div class="col-2">
                    <asp:Label ID="lblAreaName" runat="server" Text="Area Name" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtAreaName" runat="server" class="form-control form-control-sm"></asp:TextBox>
                </div>
            </div>
            <%--<div class="row p-2">

                <div class="col-6">
                </div>
            </div>--%>
            <div class="row p-2">
                <div class="col-2">
                    <asp:Label ID="lblZipCode" runat="server" Text="Zip Code" class="form-label"></asp:Label>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtZipCode" runat="server" class="form-control form-control-sm"></asp:TextBox>
                </div>
                <div class="col-6">
                </div>
            </div>
            <div class="row p-2">
                <div class="col-4">
                </div>
                <div class="col-4" style="text-align: center;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary btn-sm" />
                    <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" class="btn btn-secondary btn-sm" />
                </div>
                <div class="col-4">
                </div>
            </div>
        </div>

        <div class="modal fade" id="successMessageModal" tabindex="-1" aria-labelledby="successMessageModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-success" id="successMessageModalLabel">Success!</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p id="successMessageContent">Saved successfully!</p>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAlert" type="button" class="btn btn-success" data-bs-dismiss="modal">OK</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="confirmDeleteModal" tabindex="-1" aria-labelledby="confirmDeleteModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-danger" id="confirmDeleteModalLabel">Confirm Delete</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete this record?</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button id="btnConfirmDelete" type="button" class="btn btn-danger">Delete</button>
                    </div>
                </div>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="HFieldTransactionType" runat="server" />
        </div>

        <script>
            function showSuccessMessage(message, alertType) {
                // Set the message content dynamically
                document.getElementById('successMessageContent').innerText = message;
                if (alertType == "Error") {
                    document.getElementById('successMessageModalLabel').innerText = alertType;
                    document.getElementById('successMessageModalLabel').className = "modal-title text-danger";
                    document.getElementById('btnAlert').className = "btn btn-danger";
                }
                // Show the modal
                var modal = new bootstrap.Modal(document.getElementById('successMessageModal'));
                modal.show();
            }

            function showDeleteConfirmMessage() {
                // Show the delete confirmation modal
                var modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
                modal.show();
            }

        </script>
    </main>
</asp:Content>
