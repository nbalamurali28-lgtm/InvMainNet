<%@ Page Title="Delivery Information" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeliveryInformationAdmin.aspx.cs" Inherits="InventoryManagement.DeliveryInformationAdmin" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Load jQuery first -->
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <!-- Load Popper.js (required for Bootstrap dropdowns) -->
    <script src="Scripts/popper.min.js"></script>
    <!-- Load Bootstrap JavaScript -->
    <script src="Scripts/bootstrap.bundle.min.js"></script>

    <script>
        function calculateTotalAmount() {
            $(document).ready(function () {
                // Attach keypress event to Quantity and Price textboxes
                $("[id*=GrdDeliveryInfo] [id*=GrdTxtQuantity], [id*=GrdDeliveryInfo] [id*=GrdTxtPrice]").on("keyup", function () {
                    // Get the current row of the triggered textbox
                    var row = $(this).closest("tr");

                    // Get Quantity and Price values
                    var quantity = parseInt(row.find("[id*=GrdTxtQuantity]").val()) || 0;
                    var price = parseFloat(row.find("[id*=GrdTxtPrice]").val()) || 0;

                    // Calculate total amount
                    var totalAmount = (quantity * price).toFixed(2);

                    // Set the total amount in the TotalAmount textbox
                    row.find("[id*=GrdTxtTotalAmount]").val(totalAmount);
                });
            });
        }

        function calculateRunningTotal() {
            $(document).ready(function () {
                // Attach keypress event to Quantity textboxes
                $("[id*=GrdDeliveryInfo] [id*=GrdTxtQuantity]").on("keyup", function () {
                    var totalQuantity = 0;
                    var onlineQuantity = 0;
                    var totalAmount = 0;
                    var onlineAmount = 0;
                    var FinalAmount = 0;

                    var row = $(this).closest("tr"); // Get the current row of the triggered textbox

                    var actualQty = parseInt($(this).val()) || 0;
                    var onlineQty = parseInt(row.find("[id*=GrdTxtOnlineQty]").val()) || 0;
                    var product = row.find("[id*=GrdDdlProduct] option:selected").text();

                    if (actualQty < onlineQty) {
                        //alert('Quantity cannot be lesser than online quantity for the product ' + product);
                        row.find("[id*=GrdTxtOnlineQty]").val(actualQty);
                    }


                    // Iterate through all Quantity textboxes to calculate the total
                    $("[id*=GrdDeliveryInfo] [id*=GrdTxtQuantity]").each(function () {
                        totalQuantity += parseInt($(this).val()) || 0;
                    });

                    // Update the footer label with the total quantity
                    $("[id*=LblFooterTotalQuantity]").text("Total Quantity: " + totalQuantity);

                    // Iterate through all Online Quantity textboxes to calculate the total
                    $("[id*=GrdDeliveryInfo] [id*=GrdTxtTotalAmount]").each(function () {
                        // Get the current row of the triggered textbox
                        var row = $(this).closest("tr");

                        // Get Quantity and Price values
                        var quantity = parseInt(row.find("[id*=GrdTxtQuantity]").val()) || 0;
                        var price = parseFloat(row.find("[id*=GrdTxtPrice]").val()) || 0;

                        // Calculate total amount
                        totalAmount += (quantity * price);
                    });

                    // Update the footer label with the total quantity
                    $("[id*=LblFooterTotalAmount]").text(totalAmount.toFixed(2));


                    // Iterate through all Online Quantity textboxes to calculate the total Online Quantity
                    $("[id*=GrdDeliveryInfo] [id*=GrdTxtOnlineQty]").each(function () {
                        //onlineQuantity += parseInt($(this).val()) || 0;

                        // Get the current row of the triggered textbox
                        var row = $(this).closest("tr");

                        // Get Quantity and Price values
                        var quantity = parseInt(row.find("[id*=GrdTxtOnlineQty]").val()) || 0;
                        var price = parseFloat(row.find("[id*=GrdTxtPrice]").val()) || 0;

                        // Calculate total amount
                        onlineQuantity += quantity;
                        onlineAmount += (quantity * price);

                        $("[id*=LblFooterTotalOnlineQty]").text("Online Quantity: " + onlineQuantity);
                        $("[id*=LblFooterOnlineAmount]").text(onlineAmount.toFixed(2));
                        $("[id*=LblFooterOnlineAmount]").trigger("change");
                    });

                    if (totalAmount > 0) {
                        debugger;
                        FinalAmount = totalAmount - onlineAmount;
                        $("[id*=LblFooterFinalAmount]").text(FinalAmount.toFixed(2));
                    }
                });

                // Attach keypress event to Price textboxes
                /*$("[id*=GrdDeliveryInfo] [id*=GrdTxtPrice]").on("keyup", function () {
                    var totalAmount = 0;
 
                    // Iterate through all TotalAmount textboxes to calculate the total
                    $("[id*=GrdDeliveryInfo] [id*=GrdTxtTotalAmount]").each(function () {
                        totalAmount += parseFloat($(this).val()) || 0;
                    });
 
                    // Update the footer label with the total quantity
                    $("[id*=LblFooterTotalAmount]").text("Total Amount: " + totalAmount.toFixed(2));
                });*/
            });
        }

        function calculateOnlineRunningTotal() {
            $(document).ready(function () {
                // Attach keypress event to Online Quantity textboxes
                $("[id*=GrdDeliveryInfo] [id*=GrdTxtOnlineQty]").on("keyup", function () {
                    var onlineQuantity = 0;
                    var onlineAmount = 0;
                    var FinalAmount = 0;

                    var row = $(this).closest("tr"); // Get the current row of the triggered textbox

                    var onlineQty = parseInt($(this).val()) || 0;
                    var actualQty = parseInt(row.find("[id*=GrdTxtQuantity]").val()) || 0;
                    var product = row.find("[id*=GrdDdlProduct] option:selected").text();

                    if (onlineQty > actualQty) {
                        //alert('Quantity cannot be lesser than online quantity for the product ' + product);
                        row.find("[id*=GrdTxtOnlineQty]").val(actualQty);
                    }

                    // Iterate through all Online Quantity textboxes to calculate the total
                    $("[id*=GrdDeliveryInfo] [id*=GrdTxtOnlineQty]").each(function () {
                        onlineQuantity += parseInt($(this).val()) || 0;

                        // Get the current row of the triggered textbox
                        var row = $(this).closest("tr");

                        // Get Quantity and Price values
                        var quantity = parseInt(row.find("[id*=GrdTxtOnlineQty]").val()) || 0;
                        var price = parseFloat(row.find("[id*=GrdTxtPrice]").val()) || 0;

                        // Calculate total amount
                        onlineAmount += (quantity * price);
                    });

                    // Update the footer label with the online total quantity
                    $("[id*=LblFooterTotalOnlineQty]").text("Online Quantity: " + onlineQuantity);

                    // Update the footer label with the online total amount
                    $("[id*=LblFooterOnlineAmount]").text(onlineAmount.toFixed(2));
                    $("[id*=LblFooterOnlineAmount]").trigger("change");

                    //if (onlineAmount > 0) {
                    debugger;
                    var totalAmount = $("[id*=LblFooterTotalAmount]").text();
                    FinalAmount = totalAmount - onlineAmount;
                    $("[id*=LblFooterFinalAmount]").text(FinalAmount.toFixed(2));
                    //}
                });
            });
        }

        function calculatePaymentRunningTotal() {
            $(document).ready(function () {
                // Attach keypress event to TotalAmount textboxes
                $("[id*=GrdPaymentMode] [id*=GrdTxtAmount]").on("keyup", function () {
                    var totalPayment = 0;

                    // Iterate through all TotalAmount textboxes to calculate the total
                    $("[id*=GrdPaymentMode] [id*=GrdTxtAmount]").each(function () {
                        totalPayment += parseFloat($(this).val()) || 0;
                    });

                    // Update the footer label with the total amount
                    $("[id*=LblFooterPaymentTotal]").text("Total Amount: " + totalPayment.toFixed(2));

                    var totalAmount = $("[id*=LblFooterTotalAmount]").text();
                    //var FinalAmount = $("[id*=LblFooterFinalAmount]").text();
                    $("[id*=LblFooterShortageAmount]").text((totalAmount - totalPayment).toFixed(2));
                });
            });
        }

        function setOnlineAmountToPaymentGrid() {
            $(document).ready(function () {
                // Attach keypress event to Online Amount textboxes
                $("[id*=LblFooterOnlineAmount]").on("change", function () {
                    var onlineAmount = parseFloat($(this).text()) || 0;
                    // Set the online amount to the first row of the PaymentMode grid
                    var firstRow = $("[id*=GrdPaymentMode] tr").eq(1); // Get the first data row (index 1)
                    firstRow.find("[id*=GrdTxtAmount]").val(onlineAmount.toFixed(2));
                    // Trigger the keyup event to recalculate the payment total
                    firstRow.find("[id*=GrdTxtAmount]").trigger("keyup");

                    // Update the footer label with the total amount
                    /*if (onlineAmount > 0) {
                        var totalAmount = $("[id*=LblFooterTotalAmount]").text();
                        $("[id*=LblFooterShortageAmount]").text((totalAmount - onlineAmount).toFixed(2));
                    }*/
                });
            });
        }

        //function pageLoad(sender, args) {
        // Call the functions to attach the events
        calculateTotalAmount();
        calculateRunningTotal();
        calculateOnlineRunningTotal();
        calculatePaymentRunningTotal();
        setOnlineAmountToPaymentGrid();
        //}
    </script>

    <style type="text/css">
        .pageBody {
            width: 98%;
            margin: 1% 0% 0% 1%;
        }

        .alignRight {
            text-align: right;
        }

        .alignFooterTextRight {
            /*padding: 1% 2.8% 0% 0% !important;*/
            text-align: right !important;
            font-weight: 500 !important;
        }

        .removeGridFooterBorder {
            border-width: 0px !important;
            border-style: none !important;
        }
    </style>

    <%--<asp:UpdatePanel ID="AjaxUpdatePanel" runat="server">
        <ContentTemplate>--%>
    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="">
            <div class="row p-2">
                <div class="col-2" style="width: 10%;">
                    <asp:Label ID="LblDate" runat="server" Text="Delivery Date:" class="form-label" Style="vertical-align: middle;"></asp:Label>
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:TextBox ID="TxtDeliveryDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" TabIndex="1"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorTxtDeliveryDate" runat="server" ControlToValidate="TxtDeliveryDate" ErrorMessage="Delivery Date is required." CssClass="text-danger" Display="Dynamic" />
                </div>
                <div class="col-3" style="width: 30%;">
                    <div class="row">
                        <div class="col-4" style="display: flex; align-items: center; padding-right: 0;">
                            <asp:Label ID="LblEmployee" runat="server" Text="Employee ID:" class="form-label" Style="vertical-align: middle; margin-bottom: 0;"></asp:Label>
                        </div>
                        <div class="col-8" style="padding-left: 5px;">
                            <asp:DropDownList ID="DdlEmployeeId" runat="server" AutoPostBack="false" EnableViewState="true" CssClass="form-select form-select-sm" TabIndex="2">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorDdlEmployeeId" runat="server" ControlToValidate="DdlEmployeeId" InitialValue="" ErrorMessage="Employee ID is required." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    </div>
                </div>
                <div class="col-2" style="width: 10%;">
                    <asp:Button ID="BtnView" runat="server" Text="View" OnClick="BtnView_Click" class="btn btn-primary btn-sm"></asp:Button>
                </div>
                <div class="col-2" style="width: 15%;">
                    <asp:Label ID="LblDelivery_Id" runat="server" Text="Delivery ID:" class="form-label" Style="vertical-align: middle;"></asp:Label>
                    <asp:Label ID="LblDeliveryId" runat="server" Text="" class="form-label" Style="vertical-align: middle;"></asp:Label>
                </div>
            </div>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="GrdDeliveryInfo" runat="server" AutoGenerateColumns="false" EnableViewState="true" ShowHeaderWhenEmpty="true"
                ShowFooter="true" OnRowDataBound="GrdDeliveryInfo_RowDataBound" CssClass="table table-striped table-bordered table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="bg-primary text-white" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <div class="form-check">
                                <input id="GrdCheckBoxSelect" runat="server" class="form-check-input" type="checkbox" value="">
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Product" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:DropDownList ID="GrdDdlProduct" runat="server" AutoPostBack="true" OnSelectedIndexChanged="GrdDdlProduct_SelectedIndexChanged"
                                EnableViewState="true" class="form-select form-select-sm" TabIndex="1">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnAddRow" runat="server" Text="Add Row" OnClick="btnAddRow_Click" class="btn btn-secondary btn-sm" />
                            <asp:Button ID="btnDeleteRow" runat="server" Text="Delete Row" OnClick="btnDeleteRow_Click" class="btn btn-secondary btn-sm" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtQuantity" runat="server" TextMode="Number" EnableViewState="true"
                                min="0" oninput="validity.valid||(value='');" onkeydown="return event.key !== '-';"
                                class="form-control form-control-sm alignRight" TabIndex="2"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LblFooterTotalQuantity" runat="server" class="form-label alignFooterTextRight"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Online" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtOnlineQty" runat="server" TextMode="Number" EnableViewState="true"
                                min="0" oninput="validity.valid||(value='');" onkeydown="return event.key !== '-';"
                                class="form-control form-control-sm alignRight" TabIndex="3"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LblFooterTotalOnlineQty" runat="server" class="form-label alignFooterTextRight"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtPrice" runat="server" TextMode="Number" ReadOnly="true" EnableViewState="true"
                                min="0" oninput="validity.valid||(value='');" onkeydown="return event.key !== '-';"
                                class="form-control form-control-sm alignRight" TabIndex="4"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Amount" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtTotalAmount" runat="server" TextMode="Number" EnableViewState="true" ReadOnly="true"
                                min="0" oninput="validity.valid||(value='');" onkeydown="return event.key !== '-';"
                                class="form-control form-control-sm alignRight" TabIndex="5"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Comments" HeaderStyle-CssClass="bg-primary text-white">
                        <ItemTemplate>
                            <asp:TextBox ID="GrdTxtRemarks" runat="server" EnableViewState="true" class="form-control form-control-sm" TabIndex="6"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="removeGridFooterBorder" />
            </asp:GridView>
            <hr />
        </div>

        <div class="table">
            <div class="row">
                <div class="col-5">
                    <asp:GridView ID="GrdPaymentMode" runat="server" AutoGenerateColumns="false" EnableViewState="true" ShowHeaderWhenEmpty="true"
                        ShowFooter="true" OnRowDataBound="GrdPaymentMode_RowDataBound" Width="100%" CssClass="table table-striped table-bordered table-hover">
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="bg-primary text-white" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <div class="form-check">
                                        <input id="GrdCheckBoxSelect" runat="server" class="form-check-input" type="checkbox" value="">
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Mode" HeaderStyle-CssClass="bg-primary text-white">
                                <ItemTemplate>
                                    <asp:DropDownList ID="GrdDdlPaymentMode" runat="server" AutoPostBack="false" EnableViewState="true" class="form-select form-select-sm" TabIndex="1">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnPaymentAddRow" runat="server" Text="Add Row" OnClick="btnPaymentAddRow_Click" class="btn btn-secondary btn-sm" />
                                    <asp:Button ID="btnPaymentDeleteRow" runat="server" Text="Delete Row" OnClick="btnPaymentDeleteRow_Click" class="btn btn-secondary btn-sm" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount" HeaderStyle-CssClass="bg-primary text-white" FooterStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:TextBox ID="GrdTxtAmount" runat="server" TextMode="Number" EnableViewState="true"
                                        min="0" oninput="validity.valid||(value='');" onkeydown="return event.key !== '-';"
                                        class="form-control form-control-sm alignRight" TabIndex="2"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LblFooterPaymentTotal" runat="server" class="form-label alignFooterTextRight"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="col-4">
                    <asp:Label ID="LblRemarks" runat="server" Text="Remarks:" class="form-label"></asp:Label>
                    <br />
                    <asp:TextBox ID="TxtRemarks" runat="server" TextMode="MultiLine" EnableViewState="true" class="form-control form-control-sm"
                        Style="margin: 2% 0% 0% 0%; width: 100%; height: 45%;" TabIndex="10"></asp:TextBox>
                </div>
                <div class="col-3">
                    <ul class="list-group">
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <span style="font-weight: 600;">Total Amount:</span>
                            <span class="badge bg-primary badge-pill">
                                <asp:Label ID="LblFooterTotalAmount" runat="server" class="form-label"></asp:Label>
                            </span>
                        </li>
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <span style="font-weight: 600;">Online Amount:</span>
                            <span class="badge bg-primary badge-pill">
                                <asp:Label ID="LblFooterOnlineAmount" runat="server" class="form-label"></asp:Label>
                            </span>
                        </li>
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <span style="font-weight: 600;">Final Amount:</span>
                            <span class="badge bg-success badge-pill">
                                <asp:Label ID="LblFooterFinalAmount" runat="server" class="form-label"></asp:Label>
                            </span>
                        </li>
                        <li class="list-group-item d-flex justify-content-between align-items-center">
                            <span style="font-weight: 600;">Shortage Amount:</span>
                            <span class="badge bg-danger badge-pill">
                                <asp:Label ID="LblFooterShortageAmount" runat="server" class="form-label"></asp:Label>
                            </span>
                        </li>
                    </ul>
                </div>
                <hr />
            </div>
        </div>

        <div class="text-center">
            <div class="row p-2">
                <div class="col-12">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" class="btn btn-primary btn-sm" />
                    <asp:Button ID="BtnModify" runat="server" Text="Update" OnClick="BtnModify_Click" class="btn btn-primary btn-sm" />
                    <asp:Button ID="BtnDelete" runat="server" Text="Delete" OnClientClick="showDeleteConfirmMessage(); return false;" OnClick="BtnDelete_Click" class="btn btn-danger btn-sm" />
                    <asp:Button ID="BtnReset" runat="server" Text="Reset" OnClick="BtnReset_Click" class="btn btn-secondary btn-sm" />
                </div>
            </div>
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

            function showDeleteConfirmMessage() {
                // Show the delete confirmation modal
                var modal = new bootstrap.Modal(document.getElementById('confirmDeleteModal'));
                modal.show();
            }

            document.addEventListener('DOMContentLoaded', function () {
                document.getElementById('btnConfirmDelete').addEventListener('click', function () {
                    __doPostBack('<%= BtnDelete.UniqueID %>', '');
                });
            });

            // Trigger Reset button click when OK button in successMessageModal is clicked
            document.getElementById('btnAlert').addEventListener('click', function () {
                document.getElementById('<%= BtnReset.ClientID %>').click();
            });

        </script>
    </main>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
