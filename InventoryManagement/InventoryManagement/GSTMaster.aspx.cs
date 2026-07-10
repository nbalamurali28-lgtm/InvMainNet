using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class GSTMaster : Page
    {
        readonly ClsGSTMaster objGSTMaster = new ClsGSTMaster();
        DataTable dtGSTMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGridView();
                ClearControls();
            }
        }

        private void BindGridView()
        {
            try
            {
                dtGSTMaster = objGSTMaster.GetGSTMaster();
                grdGSTMaster.DataSource = dtGSTMaster;
                grdGSTMaster.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Error binding grid: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                // If editing existing GST, update; otherwise create new
                if (ViewState["EditGSTID"] != null && !string.IsNullOrEmpty(ViewState["EditGSTID"].ToString()))
                {
                    var objGST = new ClsGSTMaster
                    {
                        GST_ID = ViewState["EditGSTID"].ToString(),
                        GST_PERCENTAGE = decimal.Parse(txtGSTPercentage.Text),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        EFFECTIVE_FROM = string.IsNullOrEmpty(dtEffectiveFrom.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveFrom.Text),
                        EFFECTIVE_TO = string.IsNullOrEmpty(dtEffectiveTo.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveTo.Text),
                        IS_ACTIVE = chkIsActive.Checked,
                        UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                        UPDATED_AT = DateTime.Now
                    };

                    int rowsAffected = objGSTMaster.UpdateGST(objGST);
                    ShowResult(rowsAffected);
                    if (rowsAffected > 0)
                    {
                        ClearControls();
                        BindGridView();
                    }
                }
                else
                {
                    var objGST = new ClsGSTMaster
                    {
                        GST_PERCENTAGE = decimal.Parse(txtGSTPercentage.Text),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        EFFECTIVE_FROM = string.IsNullOrEmpty(dtEffectiveFrom.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveFrom.Text),
                        EFFECTIVE_TO = string.IsNullOrEmpty(dtEffectiveTo.Text) ? (DateTime?)null : DateTime.Parse(dtEffectiveTo.Text),
                        IS_ACTIVE = chkIsActive.Checked,
                        CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                        CREATED_AT = DateTime.Now
                    };

                    int rowsAffected = objGSTMaster.CreateGST(objGST);
                    ShowResult(rowsAffected);
                    if (rowsAffected > 0)
                    {
                        ClearControls();
                        BindGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving GST: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearControls();
        }

        protected void grdGSTMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // Prevent inline GridView edit; populate main form for editing
                e.Cancel = true;
                string gstId = grdGSTMaster.DataKeys[e.NewEditIndex].Value.ToString();
                DataTable dt = objGSTMaster.GetGSTMaster();
                DataRow[] rows = dt.Select("gst_id = '" + gstId + "'");
                if (rows.Length > 0)
                {
                    var r = rows[0];
                    txtGSTPercentage.Text = r["gst_percentage"] != DBNull.Value ? Convert.ToDecimal(r["gst_percentage"]).ToString("0.00") : "";
                    txtDescription.Text = r["description"] != DBNull.Value ? r["description"].ToString() : string.Empty;
                    dtEffectiveFrom.Text = r["effective_from"] != DBNull.Value ? Convert.ToDateTime(r["effective_from"]).ToString("yyyy-MM-dd") : string.Empty;
                    dtEffectiveTo.Text = r["effective_to"] != DBNull.Value ? Convert.ToDateTime(r["effective_to"]).ToString("yyyy-MM-dd") : string.Empty;
                    chkIsActive.Checked = r["is_active"] != DBNull.Value && Convert.ToBoolean(r["is_active"]);

                    ViewState["EditGSTID"] = gstId;
                    btnSave.Text = "Update";
                    lblMessage.Style["display"] = "none";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error preparing GST for edit: " + ex.Message, "danger");
            }
        }

        protected void grdGSTMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string gstId = grdGSTMaster.DataKeys[e.RowIndex].Value.ToString();
                GridViewRow row = grdGSTMaster.Rows[e.RowIndex];

                var objGST = new ClsGSTMaster
                {
                    GST_ID = gstId,
                    GST_PERCENTAGE = decimal.Parse(((TextBox)row.Cells[1].Controls[0]).Text),
                    UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int rowsAffected = objGSTMaster.UpdateGST(objGST);
                ShowResult(rowsAffected);

                grdGSTMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating GST: " + ex.Message, "danger");
            }
        }

        protected void grdGSTMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdGSTMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdGSTMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string gstId = grdGSTMaster.DataKeys[e.RowIndex].Value.ToString();
                var objGST = new ClsGSTMaster { GST_ID = gstId };

                int rowsAffected = objGSTMaster.DeleteGST(objGST);
                ShowResult(rowsAffected);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting GST: " + ex.Message, "danger");
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtGSTPercentage.Text))
            {
                ShowMessage("GST Percentage is required.", "warning");
                return false;
            }

            if (!decimal.TryParse(txtGSTPercentage.Text, out _))
            {
                ShowMessage("Valid GST Percentage is required.", "warning");
                return false;
            }

            return true;
        }

        private void ClearControls()
        {
            txtGSTPercentage.Text = "";
            txtDescription.Text = "";
            dtEffectiveFrom.Text = "";
            dtEffectiveTo.Text = "";
            chkIsActive.Checked = true;
            lblMessage.Style["display"] = "none";
            ViewState["EditGSTID"] = null;
            btnSave.Text = "Save";
        }

        private void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                ShowMessage("Operation completed successfully.", "success");
            }
            else
            {
                ShowMessage("No records affected.", "warning");
            }
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
