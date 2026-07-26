using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class PricingMaster : Page
    {
        readonly ClsPricingMaster objPricingMaster = new ClsPricingMaster();
        readonly ClsProductMaster objProductMaster = new ClsProductMaster();
        readonly ClsGSTMaster objGSTMaster = new ClsGSTMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadDropdowns();
                BindGridView();
            }
        }

        private void LoadDropdowns()
        {
            try
            {
                // Load products
                DataTable dtProducts = objProductMaster.GetProductMaster();
                ddlProduct.DataSource = dtProducts;
                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataBind();
                ddlProduct.Items.Insert(0, new ListItem("-- Select Product --", ""));

                // Load GST
                DataTable dtGST = objGSTMaster.GetGSTMaster();
                ddlGST.DataSource = dtGST;
                ddlGST.DataTextField = "gst_percentage";
                ddlGST.DataValueField = "gst_id";
                ddlGST.DataBind();
                ddlGST.Items.Insert(0, new ListItem("-- Select GST --", ""));
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading dropdowns: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                DataTable dt = objPricingMaster.GetPricing();
                grdPricingMaster.DataSource = dt;
                grdPricingMaster.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading pricing: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(ddlGST.SelectedValue))
                {
                    ShowMessage("Please select Product and GST.", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtBasePrice.Text))
                {
                    ShowMessage("Please enter Base Price.", "warning");
                    return;
                }

                // Validate From Date is provided
                DateTime? effectiveFrom = null;
                DateTime? effectiveTo = null;
                int editId = 0;

                // If editing, use the preserved From Date (read-only display)
                bool isEditing = ViewState["EditPricingID"] != null && int.TryParse(ViewState["EditPricingID"].ToString(), out editId) && editId > 0;

                if (isEditing)
                {
                    // Use the original effective_from date (preserved from when Edit was clicked)
                    effectiveFrom = ViewState["OriginalEffectiveFrom"] != null 
                        ? (DateTime)ViewState["OriginalEffectiveFrom"] 
                        : DateTime.Now;
                }
                else
                {
                    // For new records, From Date is required
                    if (string.IsNullOrEmpty(txtFromDate.Text))
                    {
                        ShowMessage("Please enter From Date.", "warning");
                        return;
                    }

                    if (!DateTime.TryParse(txtFromDate.Text, out DateTime fromDate))
                    {
                        ShowMessage("Invalid From Date format.", "warning");
                        return;
                    }

                    effectiveFrom = fromDate;
                }

                // Validate To Date if provided
                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    if (!DateTime.TryParse(txtToDate.Text, out DateTime toDate))
                    {
                        ShowMessage("Invalid To Date format.", "warning");
                        return;
                    }

                    effectiveTo = toDate;

                    // Validate: From Date must be before To Date
                    if (effectiveFrom.HasValue && effectiveTo.HasValue)
                    {
                        if (effectiveFrom.Value >= effectiveTo.Value)
                        {
                            ShowMessage("From Date must be before To Date.", "warning");
                            return;
                        }
                    }
                }

                int productId = int.Parse(ddlProduct.SelectedValue);

                // If editing existing pricing, update; otherwise create new
                if (isEditing)
                {
                    // Check for overlapping date ranges (excluding current pricing)
                    if (effectiveFrom.HasValue && objPricingMaster.HasOverlappingPricingExcludingCurrent(productId, editId, effectiveFrom.Value, effectiveTo))
                    {
                        ShowMessage("Cannot update. This date range overlaps with an existing pricing entry for this product. Please check the From Date and To Date.", "warning");
                        return;
                    }

                    // Check if there are other open pricing records for this product (excluding current)
                    if (objPricingMaster.HasOpenPricingExcludingCurrent(productId, editId))
                    {
                        ShowMessage("Cannot update. Another pricing entry for this product exists without an 'Effective To' date. Please close that pricing entry first.", "warning");
                        return;
                    }

                    var objPricing = new ClsPricingMaster
                    {
                        PRICING_ID = editId,
                        PRODUCT_ID = productId,
                        BASE_PRICE = decimal.Parse(txtBasePrice.Text),
                        GST_ID = ddlGST.SelectedValue,
                        EFFECTIVE_FROM = effectiveFrom,
                        EFFECTIVE_TO = effectiveTo,
                        EFFECTIVE_STATUS = "ACTIVE",
                        UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                        UPDATED_AT = DateTime.Now
                    };

                    int result = objPricingMaster.UpdatePricing(objPricing);
                    ShowResult(result);
                    if (result > 0)
                    {
                        ClearControls();
                        BindGridView();
                    }
                }
                else
                {
                    // Check for overlapping date ranges
                    if (effectiveFrom.HasValue && objPricingMaster.HasOverlappingPricing(productId, effectiveFrom.Value, effectiveTo))
                    {
                        ShowMessage("Cannot add new pricing. This date range overlaps with an existing pricing entry for this product. Please check the From Date and To Date.", "warning");
                        return;
                    }

                    // Check if product already has an open pricing (without effective_to date)
                    if (objPricingMaster.HasOpenPricing(productId))
                    {
                        ShowMessage("Cannot add new pricing. This product already has an active pricing entry without an 'Effective To' date. Please close the existing pricing entry by setting its 'To Date' before adding a new price.", "warning");
                        return;
                    }

                    var objPricing = new ClsPricingMaster
                    {
                        PRODUCT_ID = productId,
                        BASE_PRICE = decimal.Parse(txtBasePrice.Text),
                        GST_ID = ddlGST.SelectedValue,
                        EFFECTIVE_FROM = effectiveFrom,
                        EFFECTIVE_TO = effectiveTo,
                        EFFECTIVE_STATUS = "ACTIVE",
                        CREATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                        CREATED_AT = DateTime.Now
                    };

                    int result = objPricingMaster.CreatePricing(objPricing);
                    ShowResult(result);
                    if (result > 0)
                    {
                        ClearControls();
                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
            // Focus on the product dropdown using ClientScript
            ScriptManager.RegisterStartupScript(this, GetType(), "FocusProduct", 
                "$('#" + ddlProduct.ClientID + "').focus();", true);
        }

        protected void grdPricingMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // Prevent inline GridView edit; populate main form for editing
                e.Cancel = true;
                int pricingId = (int)grdPricingMaster.DataKeys[e.NewEditIndex].Value;
                DataTable dt = objPricingMaster.GetPricing();
                DataRow[] rows = dt.Select("pricing_id = " + pricingId);
                if (rows.Length > 0)
                {
                    var r = rows[0];
                    ddlProduct.SelectedValue = r["ProductID"] != DBNull.Value ? r["ProductID"].ToString() : "";
                    txtBasePrice.Text = r["base_price"] != DBNull.Value ? Convert.ToDecimal(r["base_price"]).ToString("0.00") : "";
                    ddlGST.SelectedValue = r["gst_id"] != DBNull.Value ? r["gst_id"].ToString() : "";

                    // Display From Date as read-only when editing
                    if (r["effective_from"] != DBNull.Value)
                    {
                        DateTime fromDate = Convert.ToDateTime(r["effective_from"]);
                        ViewState["OriginalEffectiveFrom"] = fromDate;

                        // Hide the textbox and show the label with datetime format
                        txtFromDate.Visible = false;
                        lblFromDateDisplay.Visible = true;
                        lblFromDateDisplay.Text = fromDate.ToString("yyyy-MM-dd HH:mm");
                    }

                    // Populate To Date if it exists (using datetime-local format)
                    if (r["effective_to"] != DBNull.Value)
                    {
                        DateTime toDate = Convert.ToDateTime(r["effective_to"]);
                        txtToDate.Text = toDate.ToString("yyyy-MM-ddTHH:mm");
                    }
                    else
                    {
                        txtToDate.Text = "";
                    }

                    ViewState["EditPricingID"] = pricingId;
                    btnSave.Text = "Update";
                    lblMessage.Style["display"] = "none";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error preparing pricing for edit: " + ex.Message, "danger");
            }
        }

        protected void grdPricingMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int pricingId = (int)grdPricingMaster.DataKeys[e.RowIndex].Value;
                GridViewRow row = grdPricingMaster.Rows[e.RowIndex];

                // Extract values from the editing row
                string productName = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
                decimal basePrice = decimal.Parse(((TextBox)row.Cells[2].Controls[0]).Text);
                decimal gstPercentage = decimal.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
                DateTime effectiveFrom = DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text);
                string effectiveToText = ((TextBox)row.Cells[5].Controls[0]).Text;
                DateTime? effectiveTo = string.IsNullOrEmpty(effectiveToText) ? (DateTime?)null : DateTime.Parse(effectiveToText);

                // Get the product ID and GST ID from the current data
                DataTable dt = objPricingMaster.GetPricing();
                DataRow[] rows = dt.Select("pricing_id = " + pricingId);
                if (rows.Length == 0)
                {
                    ShowMessage("Pricing record not found.", "danger");
                    return;
                }

                int productId = Convert.ToInt32(rows[0]["ProductID"]);
                string gstId = rows[0]["gst_id"]?.ToString() ?? "";

                var objPricing = new ClsPricingMaster
                {
                    PRICING_ID = pricingId,
                    PRODUCT_ID = productId,
                    BASE_PRICE = basePrice,
                    GST_ID = gstId,
                    EFFECTIVE_FROM = effectiveFrom,
                    EFFECTIVE_TO = effectiveTo,
                    EFFECTIVE_STATUS = "ACTIVE",
                    UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int result = objPricingMaster.UpdatePricing(objPricing);
                ShowResult(result);
                grdPricingMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating: " + ex.Message, "danger");
            }
        }

        protected void grdPricingMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdPricingMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdPricingMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int pricingId = (int)grdPricingMaster.DataKeys[e.RowIndex].Value;
                var objPricing = new ClsPricingMaster { PRICING_ID = pricingId };
                int result = objPricingMaster.DeletePricing(objPricing);
                ShowResult(result);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting: " + ex.Message, "danger");
            }
        }

        private void ShowResult(int rowsAffected)
        {
            ShowMessage(rowsAffected > 0 ? "Operation completed successfully." : "No records affected.", rowsAffected > 0 ? "success" : "warning");
        }

        private void ClearControls()
        {
            ddlProduct.SelectedValue = "";
            txtBasePrice.Text = "";
            ddlGST.SelectedValue = "";
            txtFromDate.Text = "";
            txtFromDate.Visible = true;
            lblFromDateDisplay.Visible = false;
            lblFromDateDisplay.Text = "";
            txtToDate.Text = "";
            ViewState["EditPricingID"] = null;
            ViewState["OriginalEffectiveFrom"] = null;
            btnSave.Text = "Save";
            lblMessage.Style["display"] = "none";
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
