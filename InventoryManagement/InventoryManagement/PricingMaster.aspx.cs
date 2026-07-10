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

                // If editing existing pricing, update; otherwise create new
                if (ViewState["EditPricingID"] != null && int.TryParse(ViewState["EditPricingID"].ToString(), out var editId) && editId > 0)
                {
                    DateTime? effectiveTo = null;
                    if (!string.IsNullOrEmpty(txtToDate.Text))
                    {
                        effectiveTo = DateTime.Parse(txtToDate.Text);
                    }

                    // Use the original effective_from date (preserved from when Edit was clicked)
                    DateTime effectiveFrom = ViewState["OriginalEffectiveFrom"] != null 
                        ? (DateTime)ViewState["OriginalEffectiveFrom"] 
                        : DateTime.Now;

                    var objPricing = new ClsPricingMaster
                    {
                        PRICING_ID = editId,
                        PRODUCT_ID = int.Parse(ddlProduct.SelectedValue),
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
                    var objPricing = new ClsPricingMaster
                    {
                        PRODUCT_ID = int.Parse(ddlProduct.SelectedValue),
                        BASE_PRICE = decimal.Parse(txtBasePrice.Text),
                        GST_ID = ddlGST.SelectedValue,
                        EFFECTIVE_FROM = DateTime.Now,
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

                    // Populate To Date if it exists
                    if (r["effective_to"] != DBNull.Value)
                    {
                        DateTime toDate = Convert.ToDateTime(r["effective_to"]);
                        txtToDate.Text = toDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        txtToDate.Text = "";
                    }

                    // Store the original effective_from date to preserve it during update
                    if (r["effective_from"] != DBNull.Value)
                    {
                        ViewState["OriginalEffectiveFrom"] = Convert.ToDateTime(r["effective_from"]);
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
