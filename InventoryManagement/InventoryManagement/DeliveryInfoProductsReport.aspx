<%@ Page Title="Delivery Information - Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryInfoProductsReport.aspx.cs" Inherits="InventoryManagement.DeliveryInfoProductsReport" %>

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

        .alignRight {
            text-align: right;
        }

        .alignDataRowTextRight {
            /*padding: 1% 2.8% 0% 0% !important;*/
            text-align: right !important;
        }

        .alignFooterTextRight {
            /*padding: 1% 2.8% 0% 0% !important;*/
            text-align: right !important;
            font-weight: bold !important;
        }

        .removeGridFooterBorder {
            border-width: 0px !important;
            border-style: none !important;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="">
            <div class="row p-2">
                <div class="col-2" style="width: 9%;">
                    <asp:Label ID="LblFromDate" runat="server" Text="From Date:" class="form-label" Style="vertical-align: middle;"></asp:Label>
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:TextBox ID="TxtFromDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorTxtFromDate" runat="server" ControlToValidate="TxtFromDate" ErrorMessage="From Date is required." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-2" style="width: 8%;">
                    <asp:Label ID="Label1" runat="server" Text="To Date:" class="form-label" Style="vertical-align: middle;"></asp:Label>
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:TextBox ID="TxtToDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorTxtToDate" runat="server" ControlToValidate="TxtToDate" ErrorMessage="To Date is required." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:Button ID="btnView" runat="server" Text="View" class="btn btn-primary btn-sm" OnClick="btnView_Click" />
                    <asp:Button ID="BtnExport" runat="server" Text="Export" class="btn btn-primary btn-sm" OnClick="BtnExport_Click" />
                    <asp:Button ID="BtnReset" runat="server" Text="Reset" class="btn btn-secondary btn-sm" OnClick="BtnReset_Click" />
                </div>
            </div>
            <hr />
        </div>

        <div class="table-responsive">
            <asp:GridView ID="GrdViewDeliveryInfo" runat="server" AutoGenerateColumns="true" CellPadding="1" CellSpacing="1" EnableViewState="true"
                EmptyDataText="No records found." ShowHeaderWhenEmpty="true" ShowFooter="true"
                OnRowDataBound="GrdViewDeliveryInfo_RowDataBound" CssClass="table table-striped table-bordered table-hover">
            </asp:GridView>
        </div>

        <div class="modal fade" id="mandatoryMessageModal" tabindex="-1" aria-labelledby="mandatoryMessageModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title text-danger" id="mandatoryMessageModalLabel">Please fill the mandatory field!</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <p id="mandatoryMessageContent">This is a mandatory message.</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" data-bs-dismiss="modal">OK</button>
                    </div>
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

        <script>
            document.addEventListener('DOMContentLoaded', function () {
                const today = new Date().toISOString().split('T')[0];
                document.getElementById('<%= TxtFromDate.ClientID %>').setAttribute('max', today);
                document.getElementById('<%= TxtToDate.ClientID %>').setAttribute('max', today);
            });

            function showMandatoryMessage(message) {
                // Set the message content dynamically
                document.getElementById('mandatoryMessageContent').innerText = message;
                // Show the modal
                var modal = new bootstrap.Modal(document.getElementById('mandatoryMessageModal'));
                modal.show();
            }

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
        </script>
    </main>
</asp:Content>
