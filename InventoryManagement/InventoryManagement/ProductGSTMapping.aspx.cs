using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class ProductGSTMapping : Page
    {
        readonly ClsProductGSTMapping objMapping = new ClsProductGSTMapping();
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
                DataTable dtProducts = objMapping.GetProductsWithGST();
                ddlProduct.DataSource = dtProducts;
                ddlProduct.DataTextField = "ProductName";
                ddlProduct.DataValueField = "ProductID";
                ddlProduct.DataBind();

                DataTable dtGST = objMapping.GetAvailableGST();
                ddlGST.DataSource = dtGST;
                ddlGST.DataTextField = "gst_display";
                ddlGST.DataValueField = "gst_id";
                ddlGST.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading data: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                DataTable dt = objMapping.GetProductGSTMappings();
                grdMapping.DataSource = dt;
                grdMapping.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ddlProduct.SelectedValue) || string.IsNullOrEmpty(ddlGST.SelectedValue))
                {
                    ShowMessage("Please select both Product and GST.", "warning");
                    return;
                }

                var objMap = new ClsProductGSTMapping
                {
                    PRODUCT_ID = int.Parse(ddlProduct.SelectedValue),
                    GST_ID = ddlGST.SelectedValue,
                    CREATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    CREATED_AT = DateTime.Now
                };

                int result = objMapping.CreateOrUpdateMapping(objMap);
                ShowResult(result);
                if (result > 0)
                {
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void grdMapping_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int productId = (int)grdMapping.DataKeys[e.RowIndex].Value;
                var objMap = new ClsProductGSTMapping { PRODUCT_ID = productId };
                int result = objMapping.DeleteMapping(objMap);
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

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
