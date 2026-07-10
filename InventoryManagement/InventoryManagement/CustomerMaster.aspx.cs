using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class CustomerMaster : System.Web.UI.Page
    {
        readonly ClsCustomerMaster objCustomerMaster = new ClsCustomerMaster();
        DataTable dtCustomerMaster = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    LoadAreas();
                    BindGridView();
                    InitializeNewCustomer();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InitializeNewCustomer()
        {
            lblCustomerIdValue.Text = objCustomerMaster.GetNextCustomerId();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateCustomerInput(out string validationMessage))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('" + validationMessage.Replace("'", "\\'") + "');", true);
                    return;
                }

                objCustomerMaster.CUSTOMER_ID = lblCustomerIdValue.Text.Trim();
                objCustomerMaster.CUSTOMER_NAME = txtCustomerName.Text.Trim();
                objCustomerMaster.CONTACT_PERSON = txtContactPerson.Text.Trim();
                objCustomerMaster.EMAIL = txtEmail.Text.Trim();
                objCustomerMaster.PHONE = txtPhone.Text.Trim();
                objCustomerMaster.ADDRESS = txtAddress.Text.Trim();
                objCustomerMaster.GST_NUMBER = txtGSTNumber.Text.Trim();
                objCustomerMaster.AREA_ID = ddlArea.SelectedValue;
                objCustomerMaster.IS_ACTIVE = Convert.ToInt32(ddlStatus.SelectedValue);
                objCustomerMaster.CREATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : null;
                objCustomerMaster.CREATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                HFieldTransactionType.Value = "Save";
                int rowsAffected = objCustomerMaster.CreateCustomer(objCustomerMaster);
                ShowResult(rowsAffected);

                if (rowsAffected > 0)
                {
                    ViewState["dtCustomerMaster"] = null;
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('Save error: " + ex.Message.Replace("'", "\\'") + "');", true);
            }
        }

        private bool ValidateCustomerInput(out string validationMessage)
        {
            validationMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(txtCustomerName.Text))
            {
                validationMessage = "Customer Name is required.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ddlArea.SelectedValue))
            {
                validationMessage = "Please select a valid Area.";
                return false;
            }

            return true;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["dtCustomerMaster"] = null;
                Response.Redirect("~/CustomerMaster.aspx");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void grdCustomerMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdCustomerMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdCustomerMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grdCustomerMaster.Rows[e.RowIndex];
            objCustomerMaster.CUSTOMER_ID = grdCustomerMaster.DataKeys[e.RowIndex].Value.ToString();
            objCustomerMaster.CUSTOMER_NAME = ((TextBox)row.FindControl("grdTxtCustomerName")).Text;
            objCustomerMaster.CONTACT_PERSON = ((TextBox)row.FindControl("grdTxtContactPerson")).Text;
            objCustomerMaster.EMAIL = ((TextBox)row.FindControl("grdTxtEmail")).Text;
            objCustomerMaster.PHONE = ((TextBox)row.FindControl("grdTxtPhone")).Text;
            objCustomerMaster.GST_NUMBER = ((TextBox)row.FindControl("grdTxtGSTNumber")).Text;
            objCustomerMaster.AREA_ID = ((DropDownList)row.FindControl("grdDdlArea")).SelectedValue;
            objCustomerMaster.IS_ACTIVE = Convert.ToInt32(((DropDownList)row.FindControl("grdDdlStatus")).SelectedValue);
            objCustomerMaster.UPDATED_BY = Session["UserID"] != null ? Session["UserID"].ToString() : null;
            objCustomerMaster.UPDATED_AT = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            HFieldTransactionType.Value = "Update";
            ShowResult(objCustomerMaster.UpdateCustomer(objCustomerMaster));

            grdCustomerMaster.EditIndex = -1;
            ViewState["dtCustomerMaster"] = null;
            BindGridView();
        }

        protected void grdCustomerMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdCustomerMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdCustomerMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string customerId = grdCustomerMaster.DataKeys[e.RowIndex].Value.ToString();
            objCustomerMaster.CUSTOMER_ID = customerId;

            HFieldTransactionType.Value = "Delete";
            ShowResult(objCustomerMaster.DeleteCustomer(objCustomerMaster));

            ViewState["dtCustomerMaster"] = null;
            BindGridView();
        }

        protected void grdCustomerMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (Control control in e.Row.Cells[e.Row.Cells.Count - 1].Controls)
                {
                    if (control is LinkButton && ((LinkButton)control).CommandName == "Delete")
                    {
                        ((LinkButton)control).OnClientClick = "return confirm('Are you sure you want to delete this record?');";
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow && grdCustomerMaster.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddlArea = (DropDownList)e.Row.FindControl("grdDdlArea");
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("grdDdlStatus");

                if (ddlArea != null)
                {
                    BindAreaDropDown(ddlArea);
                    string areaId = DataBinder.Eval(e.Row.DataItem, "area_id").ToString();
                    if (!string.IsNullOrEmpty(areaId))
                    {
                        ddlArea.SelectedValue = areaId;
                    }
                }

                if (ddlStatus != null)
                {
                    string status = DataBinder.Eval(e.Row.DataItem, "is_active").ToString();
                    ddlStatus.SelectedValue = Convert.ToBoolean(status) ? "1" : "0";
                }
            }
        }

        private void BindGridView()
        {
            if (ViewState["dtCustomerMaster"] == null)
            {
                dtCustomerMaster = objCustomerMaster.GetCustomerMaster();
                ViewState["dtCustomerMaster"] = dtCustomerMaster;
            }
            else
            {
                dtCustomerMaster = (DataTable)ViewState["dtCustomerMaster"];
            }

            if (dtCustomerMaster != null && dtCustomerMaster.Rows.Count > 0)
            {
                grdCustomerMaster.DataSource = dtCustomerMaster;
                ViewState["dtCustomerMaster"] = dtCustomerMaster;
            }
            else
            {
                grdCustomerMaster.DataSource = null;
                ViewState["dtCustomerMaster"] = null;
            }

            grdCustomerMaster.DataBind();
        }

        protected void grdCustomerMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomerMaster.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        private void LoadAreas()
        {
            DataTable dtAreas = objCustomerMaster.GetAreaList();
            ddlArea.DataSource = dtAreas;
            ddlArea.DataTextField = "area_name";
            ddlArea.DataValueField = "area_id";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("Select Area", ""));
        }

        private void BindAreaDropDown(DropDownList ddl)
        {
            DataTable dtAreas = objCustomerMaster.GetAreaList();
            ddl.DataSource = dtAreas;
            ddl.DataTextField = "area_name";
            ddl.DataValueField = "area_id";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("Select Area", ""));
        }

        private void ClearControls()
        {
            InitializeNewCustomer();
            txtCustomerName.Text = string.Empty;
            txtContactPerson.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtGSTNumber.Text = string.Empty;
            ddlArea.ClearSelection();
            ddlStatus.ClearSelection();
            var selectedItem = ddlStatus.Items.FindByValue("1");
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
            }
        }

        public void ShowResult(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                if (HFieldTransactionType.Value == "Save")
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('Saved successfully.');", true);
                else if (HFieldTransactionType.Value == "Update")
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('Modified successfully.');", true);
                else if (HFieldTransactionType.Value == "Delete")
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('Deleted successfully.');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Popup", "alert('Transaction failed.');", true);
            }

            HFieldTransactionType.Value = string.Empty;
        }
    }
}
