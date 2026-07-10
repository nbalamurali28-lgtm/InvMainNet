using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace InventoryManagement
{
    public partial class AreaMaster : System.Web.UI.Page
    {
        readonly ClsAreaMaster objAreaMaster = new ClsAreaMaster();
        DataTable dtAreaMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    BindGridView();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                objAreaMaster.AREA_ID = txtAreaId.Text;
                objAreaMaster.AREA_NAME = txtAreaName.Text;
                objAreaMaster.ZIP_CODE = txtZipCode.Text;
                objAreaMaster.CREATED_BY = "1";
                objAreaMaster.CREATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                HFieldTransactionType.Value = "Save";
                ShowResult(objAreaMaster.CreateArea(objAreaMaster));

                ViewState["dtAreaMaster"] = null;
                ClearControls();
                BindGridView();
                //Response.Redirect("~/AreaMaster.aspx");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtAreaMaster"] = null;
                Response.Redirect("~/AreaMaster.aspx");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void grdAreaMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdAreaMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdAreaMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Find the control in the EditItemTemplate
            GridViewRow row = grdAreaMaster.Rows[e.RowIndex];
            //string AreaId = ((TextBox)row.FindControl("grdTxtAreaId")).Text;
            //string AreaName = ((TextBox)row.FindControl("grdTxtAreaName")).Text;
            //string ZipCode = ((TextBox)row.FindControl("grdTxtZipCode")).Text;

            // Code to update data source using e.RowIndex or DataKeys[e.RowIndex]
            // 2. Perform the actual update in your database
            objAreaMaster.AREA_ID = ((TextBox)row.FindControl("grdTxtAreaId")).Text;
            objAreaMaster.AREA_NAME = ((TextBox)row.FindControl("grdTxtAreaName")).Text;
            objAreaMaster.ZIP_CODE = ((TextBox)row.FindControl("grdTxtZipCode")).Text;
            objAreaMaster.UPDATED_BY = "1";
            objAreaMaster.UPDATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            HFieldTransactionType.Value = "Update";
            ShowResult(objAreaMaster.UpdateArea(objAreaMaster));

            grdAreaMaster.EditIndex = -1; // Exit edit mode
            ViewState["dtAreaMaster"] = null;
            BindGridView();
        }

        protected void grdAreaMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdAreaMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdAreaMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 1. Get the ID of the record to delete from DataKeys
            GridViewRow row = grdAreaMaster.Rows[e.RowIndex];
            string AreaId = grdAreaMaster.DataKeys[e.RowIndex].Value.ToString();

            // 2. Perform the actual deletion in your database
            objAreaMaster.AREA_ID = AreaId; // ((TextBox)row.FindControl("grdTxtAreaId")).Text;

            HFieldTransactionType.Value = "Delete";
            ShowResult(objAreaMaster.DeleteArea(objAreaMaster));

            // 3. Refresh the GridView to show updated data
            ViewState["dtAreaMaster"] = null;
            BindGridView();
        }

        protected void grdAreaMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Find the Delete button (it is usually the first control in the CommandField cell)
                foreach (Control control in e.Row.Cells[e.Row.Cells.Count - 1].Controls)
                {
                    if (control is LinkButton && ((LinkButton)control).CommandName == "Delete")
                    {
                        ((LinkButton)control).OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                    }
                }
            }
        }

        private void BindGridView()
        {
            if (ViewState["dtAreaMaster"] == null)
            {
                dtAreaMaster = objAreaMaster.GetAreaMaster();
                ViewState["dtAreaMaster"] = dtAreaMaster;
            }
            else
            {
                dtAreaMaster = (DataTable)ViewState["dtAreaMaster"];
            }

            if (dtAreaMaster != null && dtAreaMaster.Rows.Count > 0)
            {
                grdAreaMaster.DataSource = dtAreaMaster;
                ViewState["dtAreaMaster"] = dtAreaMaster;
            }
            else
            {
                grdAreaMaster.DataSource = null;
                ViewState["dtAreaMaster"] = null;
            }
            grdAreaMaster.DataBind();
        }

        private void ClearControls()
        {
            try
            {
                txtAreaId.Text = "";
                txtAreaName.Text = "";
                txtZipCode.Text = "";
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
    }
}