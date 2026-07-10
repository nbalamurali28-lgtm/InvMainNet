using InventoryManagement.IL;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebGrease.Css.Ast.Selectors;
using static System.Net.Mime.MediaTypeNames;

namespace InventoryManagement
{
    public partial class DeliveryInformation : System.Web.UI.Page
    {
        public ClsDeliveryInformation objDeliveryInformation = new ClsDeliveryInformation();
        decimal footerTotalQuantity = 0.00M;
        decimal footerTotalAmount = 0.00M;
        decimal footerPayModeTotal = 0.00M;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    TxtDeliveryDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    TxtDeliveryDate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");

                    // Load product details for product dropdown in gridview
                    DataSet ds = objDeliveryInformation.GetMasters(); ;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        ViewState["dtProducts"] = ds.Tables[0];
                        ViewState["dtPaymentMode"] = ds.Tables[1];
                        //ViewState["dtEmployeeId"] = ds.Tables[2];

                        DdlEmployeeId.DataSource = ds.Tables[2];
                        DdlEmployeeId.DataTextField = "emp_name";
                        DdlEmployeeId.DataValueField = "emp_code";
                        DdlEmployeeId.DataBind();
                        DdlEmployeeId.Items.Insert(0, new ListItem("-- Select --", "0"));
                    }

                    BindDeliveryGridView();
                    ShowOrHideButtons();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Delivery Date and Employee ID have values
                if (string.IsNullOrWhiteSpace(TxtDeliveryDate.Text))
                {
                    // Show error message for Delivery Date
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a Delivery Date.');", true);
                    return;
                }

                if (DdlEmployeeId.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlEmployeeId.SelectedValue))
                {
                    // Show error message for Employee ID
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select an Employee ID.');", true);
                    return;
                }

                ArrayList arrListDeliveryMaster = new ArrayList();
                ArrayList arrListDeliveryDetail = new ArrayList();
                ArrayList arrListPaymentDetail = new ArrayList();
                StringBuilder sqlQueryBuilder = new StringBuilder();
                string CreatedBy = "1"; // Convert.ToString(Session["UserName"]);
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                footerTotalQuantity = 0.00M;
                footerTotalAmount = 0.00M;

                // Delivery Details - Iterate through each row of the gridview and capture the values
                foreach (GridViewRow row in GrdDeliveryInfo.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DeliveryDetails objDeliveryDetails = new DeliveryDetails();

                        // Access data from each row
                        //string customerId = row.Cells[0].Text; // For BoundFields
                        DropDownList GrdDdlProduct = (DropDownList)row.FindControl("GrdDdlProduct"); // For TemplateFields
                        TextBox GrdTxtQuantity = (TextBox)row.FindControl("GrdTxtQuantity");
                        TextBox GrdTxtPrice = (TextBox)row.FindControl("GrdTxtPrice");
                        TextBox GrdTxtTotalAmount = (TextBox)row.FindControl("GrdTxtTotalAmount");
                        TextBox GrdTxtRemarks = (TextBox)row.FindControl("GrdTxtRemarks");

                        // Process each row (e.g., add to a list or update database)

                        if (GrdDdlProduct.SelectedValue != "" && GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            footerTotalQuantity = Math.Round(footerTotalQuantity + Convert.ToDecimal(GrdTxtQuantity.Text), 2);

                            decimal quantity, price, totalAmount = 0.00M;
                            quantity = Convert.ToDecimal(GrdTxtQuantity.Text);
                            price = Convert.ToDecimal(GrdTxtPrice.Text);
                            totalAmount = Math.Round((quantity * price), 2);
                            GrdTxtTotalAmount.Text = Convert.ToString(totalAmount);
                            footerTotalAmount = footerTotalAmount + totalAmount;

                            objDeliveryDetails.DeliveryDate = TxtDeliveryDate.Text.Trim();
                            objDeliveryDetails.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                            objDeliveryDetails.ProductId = GrdDdlProduct.SelectedValue.Trim();
                            objDeliveryDetails.Quantity = GrdTxtQuantity.Text.Trim();
                            objDeliveryDetails.Price = GrdTxtPrice.Text.Trim();
                            objDeliveryDetails.TotalAmount = GrdTxtTotalAmount.Text.Trim();
                            objDeliveryDetails.Remarks = GrdTxtRemarks.Text.Trim();
                            objDeliveryDetails.CreatedBy = CreatedBy;
                            objDeliveryDetails.created_at = CreatedOn;
                            arrListDeliveryDetail.Add(objDeliveryDetails);
                        }
                    }
                }

                // Delivery Master - Capture the values from the form fields (outside the gridview)
                if (TxtDeliveryDate.Text.Trim() != "" && DdlEmployeeId.SelectedValue.Trim() != "")
                {
                    DeliveryMaster objDeliveryMaster = new DeliveryMaster();

                    objDeliveryMaster.DeliveryDate = TxtDeliveryDate.Text.Trim();
                    objDeliveryMaster.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                    objDeliveryMaster.TotalQuantity = Convert.ToString(footerTotalQuantity);
                    objDeliveryMaster.TotalAmount = Convert.ToString(footerTotalAmount);
                    objDeliveryMaster.CreatedBy = CreatedBy;
                    objDeliveryMaster.created_at = CreatedOn;
                    arrListDeliveryMaster.Add((DeliveryMaster)objDeliveryMaster);
                }

                // Payment Details - Iterate through each row of the gridview and capture the values
                foreach (GridViewRow row in GrdPaymentMode.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PaymentDetails objPaymentDetails = new PaymentDetails();
                        DropDownList GrdDdlPaymentMode = (DropDownList)row.FindControl("GrdDdlPaymentMode"); // For TemplateFields
                        TextBox GrdTxtAmount = (TextBox)row.FindControl("GrdTxtAmount");

                        if (GrdDdlPaymentMode.SelectedValue != "" && GrdTxtAmount.Text != "")
                        {
                            objPaymentDetails.DeliveryDate = TxtDeliveryDate.Text.Trim();
                            objPaymentDetails.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                            objPaymentDetails.PaymentMode = GrdDdlPaymentMode.SelectedValue.Trim();
                            objPaymentDetails.Amount = GrdTxtAmount.Text.Trim();
                            objPaymentDetails.CreatedBy = CreatedBy;
                            objPaymentDetails.created_at = CreatedOn;
                            arrListPaymentDetail.Add(objPaymentDetails);
                        }
                    }
                }

                HFieldTransactionType.Value = "Save";
                ShowResult(objDeliveryInformation.InsertTransaction(arrListDeliveryMaster, arrListDeliveryDetail, arrListPaymentDetail));
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnModify_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Delivery Date and Employee ID have values
                if (string.IsNullOrWhiteSpace(TxtDeliveryDate.Text))
                {
                    // Show error message for Delivery Date
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a Delivery Date.');", true);
                    return;
                }

                if (DdlEmployeeId.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlEmployeeId.SelectedValue))
                {
                    // Show error message for Employee ID
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select an Employee ID.');", true);
                    return;
                }

                ArrayList arrListDeliveryMaster = new ArrayList();
                ArrayList arrListDeliveryDetail = new ArrayList();
                ArrayList arrListPaymentDetail = new ArrayList();
                StringBuilder sqlQueryBuilder = new StringBuilder();
                string CreatedBy = "1"; // Convert.ToString(Session["UserName"]);
                string CreatedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                footerTotalQuantity = 0.00M;
                footerTotalAmount = 0.00M;

                // Delivery Details - Iterate through each row of the gridview and capture the values
                foreach (GridViewRow row in GrdDeliveryInfo.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        DeliveryDetails objDeliveryDetails = new DeliveryDetails();

                        // Access data from each row
                        //string customerId = row.Cells[0].Text; // For BoundFields
                        DropDownList GrdDdlProduct = (DropDownList)row.FindControl("GrdDdlProduct"); // For TemplateFields
                        TextBox GrdTxtQuantity = (TextBox)row.FindControl("GrdTxtQuantity");
                        TextBox GrdTxtPrice = (TextBox)row.FindControl("GrdTxtPrice");
                        TextBox GrdTxtTotalAmount = (TextBox)row.FindControl("GrdTxtTotalAmount");
                        TextBox GrdTxtRemarks = (TextBox)row.FindControl("GrdTxtRemarks");

                        // Process each row (e.g., add to a list or update database)

                        if (GrdDdlProduct.SelectedValue != "" && GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            footerTotalQuantity = Math.Round(footerTotalQuantity + Convert.ToDecimal(GrdTxtQuantity.Text), 2);

                            decimal quantity, price, totalAmount = 0.00M;
                            quantity = Convert.ToDecimal(GrdTxtQuantity.Text);
                            price = Convert.ToDecimal(GrdTxtPrice.Text);
                            totalAmount = Math.Round((quantity * price), 2);
                            GrdTxtTotalAmount.Text = Convert.ToString(totalAmount);
                            footerTotalAmount = footerTotalAmount + totalAmount;

                            objDeliveryDetails.DeliveryId = LblDeliveryId.Text.Trim();
                            objDeliveryDetails.DeliveryDate = TxtDeliveryDate.Text.Trim();
                            objDeliveryDetails.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                            objDeliveryDetails.ProductId = GrdDdlProduct.SelectedValue.Trim();
                            objDeliveryDetails.Quantity = GrdTxtQuantity.Text.Trim();
                            objDeliveryDetails.Price = GrdTxtPrice.Text.Trim();
                            objDeliveryDetails.TotalAmount = GrdTxtTotalAmount.Text.Trim();
                            objDeliveryDetails.Remarks = GrdTxtRemarks.Text.Trim();
                            objDeliveryDetails.CreatedBy = CreatedBy;
                            objDeliveryDetails.created_at = CreatedOn;
                            arrListDeliveryDetail.Add(objDeliveryDetails);
                        }
                    }
                }

                // Delivery Master - Capture the values from the form fields (outside the gridview)
                if (TxtDeliveryDate.Text.Trim() != "" && DdlEmployeeId.SelectedValue.Trim() != "")
                {
                    DeliveryMaster objDeliveryMaster = new DeliveryMaster();

                    objDeliveryMaster.DeliveryId = LblDeliveryId.Text.Trim(); ;
                    objDeliveryMaster.DeliveryDate = TxtDeliveryDate.Text.Trim();
                    objDeliveryMaster.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                    objDeliveryMaster.TotalQuantity = Convert.ToString(footerTotalQuantity);
                    objDeliveryMaster.TotalAmount = Convert.ToString(footerTotalAmount);
                    objDeliveryMaster.CreatedBy = CreatedBy;
                    objDeliveryMaster.created_at = CreatedOn;
                    arrListDeliveryMaster.Add((DeliveryMaster)objDeliveryMaster);
                }

                // Payment Details - Iterate through each row of the gridview and capture the values
                foreach (GridViewRow row in GrdPaymentMode.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        PaymentDetails objPaymentDetails = new PaymentDetails();
                        DropDownList GrdDdlPaymentMode = (DropDownList)row.FindControl("GrdDdlPaymentMode"); // For TemplateFields
                        TextBox GrdTxtAmount = (TextBox)row.FindControl("GrdTxtAmount");

                        if (GrdDdlPaymentMode.SelectedValue != "" && GrdTxtAmount.Text != "")
                        {
                            objPaymentDetails.DeliveryId = LblDeliveryId.Text.Trim(); ;
                            objPaymentDetails.DeliveryDate = TxtDeliveryDate.Text.Trim();
                            objPaymentDetails.EmployeeID = DdlEmployeeId.SelectedValue.Trim();
                            objPaymentDetails.PaymentMode = GrdDdlPaymentMode.SelectedValue.Trim();
                            objPaymentDetails.Amount = GrdTxtAmount.Text.Trim();
                            objPaymentDetails.CreatedBy = CreatedBy;
                            objPaymentDetails.created_at = CreatedOn;
                            arrListPaymentDetail.Add(objPaymentDetails);
                        }
                    }
                }

                HFieldTransactionType.Value = "Update";
                ShowResult(objDeliveryInformation.UpdateTransaction(arrListDeliveryMaster, arrListDeliveryDetail, arrListPaymentDetail));
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Delivery Date and Employee ID have values
                if (string.IsNullOrWhiteSpace(TxtDeliveryDate.Text))
                {
                    // Show error message for Delivery Date
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a Delivery Date.');", true);
                    return;
                }

                if (DdlEmployeeId.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlEmployeeId.SelectedValue))
                {
                    // Show error message for Employee ID
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select an Employee ID.');", true);
                    return;
                }

                HFieldTransactionType.Value = "Delete";
                ShowResult(objDeliveryInformation.DeleteTransaction(LblDeliveryId.Text.Trim()));
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtDeliveryInfo"] = null;
                ViewState["dtPaymentInfo"] = null;
                HFieldTransactionType.Value = "";
                Response.Redirect("~/DeliveryInformation.aspx", true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (ViewState["dtDeliveryInfo"] != null) // Retrieve the existing DataTable from ViewState
                {
                    // Capturing values before adding a new row
                    dt = (DataTable)ViewState["dtDeliveryInfo"];
                    dt = Rebind_GridDeliveryInformation(dt);

                    // Now add the new blank row to dt and rebind
                    dt = AddNewRow(dt);
                    ViewState["dtDeliveryInfo"] = dt;
                }

                GrdDeliveryInfo.DataSource = (DataTable)ViewState["dtDeliveryInfo"];
                GrdDeliveryInfo.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                // Capturing values before deleting an existing row
                DataTable dt = Rebind_GridDeliveryInformation((DataTable)ViewState["dtDeliveryInfo"]);
                bool isRowSelected = false;

                if (dt != null) // Retrieve the existing DataTable from ViewState
                {
                reIterate:
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CheckBox"].ToString() == "True")
                        {
                            isRowSelected = true;
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                            goto reIterate;
                        }
                    }
                }

                if (isRowSelected == true)
                {
                    if (dt != null && dt.Rows.Count == 0)
                        dt = AddNewRow(dt);

                    ViewState["dtDeliveryInfo"] = dt;
                    GrdDeliveryInfo.DataSource = dt;
                    GrdDeliveryInfo.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrdDeliveryInfo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Check if the current row is a data row (not header/footer)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Find the DropDownList using its ID
                    DropDownList GrdDdlProduct = (DropDownList)e.Row.FindControl("GrdDdlProduct");
                    if (GrdDdlProduct != null)
                    {
                        // Populate the DropDownList from your data source
                        GrdDdlProduct.DataSource = (DataTable)ViewState["dtProducts"]; // Method to fetch your data
                        GrdDdlProduct.DataTextField = "ProductName";
                        GrdDdlProduct.DataValueField = "ProductID";
                        GrdDdlProduct.DataBind();

                        // Optional: Add a default "Select" item
                        GrdDdlProduct.Items.Insert(0, new ListItem("-- Select --", ""));

                        // Set the selected value based on the current row data
                        GrdDdlProduct.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ProductID").ToString();
                    }

                    TextBox GrdTxtQuantity = (TextBox)e.Row.FindControl("GrdTxtQuantity");
                    if (GrdTxtQuantity != null)
                    {
                        GrdTxtQuantity.Text = DataBinder.Eval(e.Row.DataItem, "Quantity").ToString();
                        if (GrdTxtQuantity.Text != "")
                            footerTotalQuantity = Math.Round(footerTotalQuantity + Convert.ToDecimal(GrdTxtQuantity.Text), 2);
                    }

                    TextBox GrdTxtPrice = (TextBox)e.Row.FindControl("GrdTxtPrice");
                    if (GrdTxtPrice != null)
                        GrdTxtPrice.Text = DataBinder.Eval(e.Row.DataItem, "Price").ToString();

                    TextBox GrdTxtTotalAmount = (TextBox)e.Row.FindControl("GrdTxtTotalAmount");
                    if (GrdTxtTotalAmount != null)
                    {
                        GrdTxtTotalAmount.Text = DataBinder.Eval(e.Row.DataItem, "TotalAmount").ToString();
                        decimal quantity, price, totalAmount = 0.00M;
                        if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                        {
                            quantity = Convert.ToDecimal(GrdTxtQuantity.Text);
                            price = Convert.ToDecimal(GrdTxtPrice.Text);
                            totalAmount = Math.Round((quantity * price), 2);
                            footerTotalAmount = footerTotalAmount + totalAmount;
                            //GrdTxtTotalAmount.Text = Convert.ToString(totalAmount);
                        }
                    }

                    TextBox GrdTxtRemarks = (TextBox)e.Row.FindControl("GrdTxtRemarks");
                    if (GrdTxtRemarks != null)
                        GrdTxtRemarks.Text = DataBinder.Eval(e.Row.DataItem, "Remarks").ToString();
                }

                // Check if the current row being bound is the footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // Access cells by index and set text
                    // You can use a calculated variable here
                    if (footerTotalQuantity > 0)
                    {
                        Label LblFooterTotalQuantity = (Label)e.Row.Cells[2].FindControl("LblFooterTotalQuantity");
                        LblFooterTotalQuantity.Text = "Total Quantity: " + Convert.ToString(footerTotalQuantity);
                        //e.Row.Cells[2].Text = Convert.ToString(footerTotalQuantity);
                        e.Row.Cells[2].CssClass = "alignFooterTextRight";
                    }

                    if (footerTotalAmount > 0)
                    {
                        Label LblFooterTotalAmount = (Label)e.Row.Cells[4].FindControl("LblFooterTotalAmount");
                        LblFooterTotalAmount.Text = "Total Amount: " + Convert.ToString(footerTotalAmount);
                        //e.Row.Cells[4].Text = Convert.ToString(footerTotalAmount);
                        e.Row.Cells[4].CssClass = "alignFooterTextRight";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable AddNewRow(DataTable dtNewRow)
        {
            // 1. Retrieve the existing DataTable from ViewState
            DataTable dt = dtNewRow; // (DataTable)ViewState["dtDeliveryInfo"];
            DataRow dr;

            try
            {
                /*if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("ProductID");
                    dt.Columns.Add("Quantity");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("TotalAmount");
                    dt.Columns.Add("Remarks");
                }*/

                // 2. Add a new row
                dr = dt.NewRow();
                /*dr["ProductID"] = "";
                dr["Quantity"] = "";
                dr["Price"] = "";
                dr["TotalAmount"] = "";
                dr["Remarks"] = "";*/
                dt.Rows.Add(dr);

                // 3. Save back to ViewState and rebind
                //ViewState["dtDeliveryInfo"] = dt;
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        private void BindDeliveryGridView()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtDeliveryInfo = new DataTable();
                DataTable dtPaymentInfo = new DataTable();

                if (ViewState["dtDeliveryInfo"] == null)
                {
                    ds = objDeliveryInformation.GetDeliveryDetails(TxtDeliveryDate.Text, DdlEmployeeId.SelectedValue.Trim());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                            LblDeliveryId.Text = ds.Tables[0].Rows[0]["Delivery_ID"].ToString();
                        else
                            LblDeliveryId.Text = "";

                        dtDeliveryInfo = ds.Tables[1];
                        dtPaymentInfo = ds.Tables[2];
                    }

                    ViewState["dtDeliveryInfo"] = dtDeliveryInfo;
                    ViewState["dtPaymentInfo"] = dtPaymentInfo;
                }

                // Set initial row structure for gridview if no records found for the selected date
                if (dtDeliveryInfo == null || dtDeliveryInfo.Rows.Count == 0)
                {
                    dtDeliveryInfo = AddNewRow(dtDeliveryInfo);
                }

                if (dtPaymentInfo == null || dtPaymentInfo.Rows.Count == 0)
                {
                    dtPaymentInfo = AddNewRow(dtPaymentInfo);
                }

                GrdDeliveryInfo.DataSource = dtDeliveryInfo;
                GrdDeliveryInfo.DataBind();

                GrdPaymentMode.DataSource = dtPaymentInfo;
                GrdPaymentMode.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ShowResult(int rowsAffected)
        {
            try
            {
                if (rowsAffected > 0)
                {
                    // Execute server logic here, then show the popup
                    if (HFieldTransactionType.Value == "Save")
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('Saved successfully.','Success');", true);
                    else if (HFieldTransactionType.Value == "Update")
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('Modified successfully.','Success');", true);
                    else if (HFieldTransactionType.Value == "Delete")
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('Deleted successfully.','Success');", true);

                    //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#successMessageModal').modal('show');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showSuccessMessage('Transaction failed.','Error');", true);
                }
                HFieldTransactionType.Value = "";
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void BtnView_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if Delivery Date and Employee ID have values
                if (string.IsNullOrWhiteSpace(TxtDeliveryDate.Text))
                {
                    // Show error message for Delivery Date
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select a Delivery Date.');", true);
                    return;
                }

                if (DdlEmployeeId.SelectedValue == "0" || string.IsNullOrWhiteSpace(DdlEmployeeId.SelectedValue))
                {
                    // Show error message for Employee ID
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "showMandatoryMessage('Please select an Employee ID.');", true);
                    return;
                }

                ViewState["dtDeliveryInfo"] = null;
                ViewState["dtPaymentInfo"] = null;
                BindDeliveryGridView();
                ShowOrHideButtons();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void GrdPaymentMode_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Check if the current row is a data row (not header/footer)
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // Find the DropDownList using its ID
                    DropDownList GrdDdlPaymentMode = (DropDownList)e.Row.FindControl("GrdDdlPaymentMode");
                    if (GrdDdlPaymentMode != null)
                    {
                        // Populate the DropDownList from your data source
                        GrdDdlPaymentMode.DataSource = (DataTable)ViewState["dtPaymentMode"];
                        GrdDdlPaymentMode.DataTextField = "PayModeName";
                        GrdDdlPaymentMode.DataValueField = "PayModeId";
                        GrdDdlPaymentMode.DataBind();

                        // Optional: Add a default "Select" item
                        GrdDdlPaymentMode.Items.Insert(0, new ListItem("-- Select --", ""));

                        // Set the selected value based on the current row data
                        GrdDdlPaymentMode.SelectedValue = DataBinder.Eval(e.Row.DataItem, "PaymentMode").ToString();
                    }

                    TextBox GrdTxtAmount = (TextBox)e.Row.FindControl("GrdTxtAmount");
                    if (GrdTxtAmount != null)
                    {
                        GrdTxtAmount.Text = DataBinder.Eval(e.Row.DataItem, "Amount").ToString();
                        decimal Amount = 0.00M;
                        if (GrdTxtAmount.Text != "")
                        {
                            Amount = Math.Round(Convert.ToDecimal(GrdTxtAmount.Text), 2);
                            footerPayModeTotal = footerPayModeTotal + Amount;
                        }
                    }
                }

                // Check if the current row being bound is the footer
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    // Access cells by index and set text
                    // You can use a calculated variable here
                    if (footerPayModeTotal > 0)
                    {
                        Label LblFooterAmount = (Label)e.Row.Cells[2].FindControl("LblFooterAmount");
                        LblFooterAmount.Text = "Total Amount: " + Convert.ToString(footerPayModeTotal);
                        //e.Row.Cells[2].Text = Convert.ToString(footerTotalQuantity);
                        e.Row.Cells[2].CssClass = "alignFooterTextRight";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnPaymentAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                Rebind_GridDeliveryInformation((DataTable)ViewState["dtDeliveryInfo"]);
                GrdDeliveryInfo.DataSource = (DataTable)ViewState["dtDeliveryInfo"];
                GrdDeliveryInfo.DataBind();

                DataTable dt = new DataTable();

                if (ViewState["dtPaymentInfo"] != null) // Retrieve the existing DataTable from ViewState
                {
                    // Capturing values before adding a new row
                    dt = (DataTable)ViewState["dtPaymentInfo"];
                    dt = Rebind_GridPaymentMode(dt);

                    // Now add the new blank row to dt and rebind
                    dt = AddNewRow(dt);
                    ViewState["dtPaymentInfo"] = dt;
                }

                GrdPaymentMode.DataSource = (DataTable)ViewState["dtPaymentInfo"];
                GrdPaymentMode.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnPaymentDeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                // Capturing values before deleting an existing row
                DataTable dt = Rebind_GridPaymentMode((DataTable)ViewState["dtPaymentInfo"]);
                bool isRowSelected = false;

                if (dt != null) // Retrieve the existing DataTable from ViewState
                {
                reIterate:
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["CheckBox"].ToString() == "True")
                        {
                            isRowSelected = true;
                            dt.Rows[i].Delete();
                            dt.AcceptChanges();
                            goto reIterate;
                        }
                    }
                }

                if (isRowSelected == true)
                {
                    if (dt != null && dt.Rows.Count == 0)
                        dt = AddNewRow(dt);

                    ViewState["dtPaymentInfo"] = dt;
                    GrdPaymentMode.DataSource = dt;
                    GrdPaymentMode.DataBind();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DataTable Rebind_GridDeliveryInformation(DataTable currentDt)
        {
            DataTable dt = currentDt; // (DataTable)ViewState["dtDeliveryInfo"];

            try
            {
                if (dt != null && dt.Columns.Contains("CheckBox") == false)
                {
                    dt.Columns.Add("CheckBox");
                    dt.AcceptChanges();
                }

                for (int i = 0; i < GrdDeliveryInfo.Rows.Count; i++)
                {
                    HtmlInputCheckBox GrdCheckBoxSelect = (HtmlInputCheckBox)GrdDeliveryInfo.Rows[i].FindControl("GrdCheckBoxSelect");
                    if (GrdCheckBoxSelect.Checked == true)
                        dt.Rows[i]["CheckBox"] = true;

                    DropDownList GrdDdlProduct = (DropDownList)GrdDeliveryInfo.Rows[i].FindControl("GrdDdlProduct");
                    if (GrdDdlProduct.SelectedValue != "")
                        dt.Rows[i]["ProductID"] = GrdDdlProduct.SelectedValue;

                    TextBox GrdTxtQuantity = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtQuantity");
                    if (GrdTxtQuantity.Text != "")
                        dt.Rows[i]["Quantity"] = GrdTxtQuantity.Text;

                    TextBox GrdTxtPrice = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtPrice");
                    if (GrdTxtPrice.Text != "")
                        dt.Rows[i]["Price"] = GrdTxtPrice.Text;

                    TextBox GrdTxtTotalAmount = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtTotalAmount");
                    if (GrdTxtQuantity.Text != "" && GrdTxtPrice.Text != "")
                    {
                        decimal quantity = Math.Round(Convert.ToDecimal(GrdTxtQuantity.Text), 2);
                        decimal price = Math.Round(Convert.ToDecimal(GrdTxtPrice.Text), 2);
                        decimal totalAmount = Math.Round((quantity * price), 2);
                        dt.Rows[i]["TotalAmount"] = totalAmount;
                    }
                    else
                    {
                        dt.Rows[i]["TotalAmount"] = 0.00;
                    }

                    TextBox GrdTxtRemarks = (TextBox)GrdDeliveryInfo.Rows[i].FindControl("GrdTxtRemarks");
                    dt.Rows[i]["Remarks"] = GrdTxtRemarks.Text;
                }
            }
            catch (Exception)
            {
                throw;
            }

            ViewState["dtDeliveryInfo"] = dt;
            return dt;
        }

        private void ShowOrHideButtons()
        {
            if (LblDeliveryId.Text.Trim() == "")
            {
                BtnSave.Visible = true;
                BtnModify.Visible = false;
                BtnDelete.Visible = false;
            }
            else if (LblDeliveryId.Text.Trim() != "")
            {
                BtnSave.Visible = false;
                BtnModify.Visible = true;
                BtnDelete.Visible = true;
            }
        }

        private void ClearFooterTotals()
        {
            footerTotalQuantity = 0.00M;
            footerTotalAmount = 0.00M;
            footerPayModeTotal = 0.00M;
        }

        private DataTable Rebind_GridPaymentMode(DataTable currentDt)
        {
            DataTable dt = currentDt; // (DataTable)ViewState["dtPaymentInfo"];

            try
            {
                if (dt != null && dt.Columns.Contains("CheckBox") == false)
                {
                    dt.Columns.Add("CheckBox");
                    dt.AcceptChanges();
                }

                for (int i = 0; i < GrdPaymentMode.Rows.Count; i++)
                {
                    HtmlInputCheckBox GrdCheckBoxSelect = (HtmlInputCheckBox)GrdPaymentMode.Rows[i].FindControl("GrdCheckBoxSelect");
                    if (GrdCheckBoxSelect.Checked == true)
                        dt.Rows[i]["CheckBox"] = true;

                    DropDownList GrdDdlPaymentMode = (DropDownList)GrdPaymentMode.Rows[i].FindControl("GrdDdlPaymentMode");
                    if (GrdDdlPaymentMode.SelectedValue != "")
                        dt.Rows[i]["PaymentMode"] = GrdDdlPaymentMode.SelectedValue;

                    decimal Amount = 0.00M;
                    TextBox GrdTxtAmount = (TextBox)GrdPaymentMode.Rows[i].FindControl("GrdTxtAmount");
                    if (GrdTxtAmount.Text != "")
                    {
                        dt.Rows[i]["Amount"] = GrdTxtAmount.Text;
                        Amount = Math.Round(Convert.ToDecimal(GrdTxtAmount.Text), 2);
                    }
                    else
                    {
                        //dt.Rows[i]["Amount"] = Amount;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            ViewState["dtPaymentInfo"] = dt;
            return dt;
        }
    }
}