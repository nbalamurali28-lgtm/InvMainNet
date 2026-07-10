using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Css.Ast.Selectors;

namespace InventoryManagement
{
    public partial class UserMaster : Page
    {
        readonly ClsUserMaster objUserMaster = new ClsUserMaster();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadRoles();
                GetEmployees();
                BindGridView();
                ClearControls();
            }
        }

        private void LoadRoles()
        {
            try
            {
                DataTable dt = objUserMaster.GetRoles();
                ddlRole.DataSource = dt;
                ddlRole.DataTextField = "role_name";
                ddlRole.DataValueField = "role_id";
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("-- Select --", "0"));
                ViewState["dtRoles"] = dt; // Store roles in ViewState for later use
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading roles: " + ex.Message, "danger");
            }
        }

        private void BindGridView()
        {
            try
            {
                DataTable dt = objUserMaster.GetUserMaster();
                grdUserMaster.DataSource = dt;
                grdUserMaster.DataBind();
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
                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    ShowMessage("Username is required.", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    ShowMessage("Password are required.", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(ddlRole.SelectedValue))
                {
                    ShowMessage("Role is required.", "warning");
                    return;
                }

                if (string.IsNullOrEmpty(DdlEmployeeId.SelectedValue))
                {
                    ShowMessage("Employee Id is required.", "warning");
                    return;
                }

                var objUser = new ClsUserMaster
                {
                    USER_ID = "U" + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8),
                    USERNAME = txtUsername.Text.Trim(),
                    PASSWORD = txtPassword.Text,
                    EMPLOYEE_ID = DdlEmployeeId.SelectedValue.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    ROLE_ID = ddlRole.SelectedValue,
                    IS_ACTIVE = chkIsActive.Checked,
                    //CREATED_BY = Session["UserName"]?.ToString() ?? "SYSTEM",
                    //CREATED_AT = DateTime.Now
                };

                int result = objUserMaster.CreateUser(objUser);
                ShowResult(result);
                if (result > 0)
                {
                    ClearControls();
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e) => ClearControls();

        protected void grdUserMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdUserMaster.EditIndex = e.NewEditIndex;
            BindGridView();
        }

        protected void grdUserMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string userId = grdUserMaster.DataKeys[e.RowIndex].Value.ToString();
                var objUser = new ClsUserMaster
                {
                    USER_ID = userId,
                    EMPLOYEE_ID = ((DropDownList)grdUserMaster.Rows[e.RowIndex].Cells[2].FindControl["GrdDdlEmployeeId"]).SelectedValue.Trim(),
                    ROLE_ID = ((DropDownList)grdUserMaster.Rows[e.RowIndex].Cells[3].Controls[0]).SelectedValue.Trim(),
                    EMAIL = ((TextBox)grdUserMaster.Rows[e.RowIndex].Cells[4].Controls[0]).Text.Trim(),
                    IS_ACTIVE = ((CheckBox)grdUserMaster.Rows[e.RowIndex].Cells[4].Controls[0]).Checked,
                    UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int result = objUserMaster.UpdateUser(objUser);
                ShowResult(result);
                grdUserMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void grdUserMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdUserMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdUserMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string userId = grdUserMaster.DataKeys[e.RowIndex].Value.ToString();
                var objUser = new ClsUserMaster { USER_ID = userId };
                int result = objUserMaster.DeleteUser(objUser);
                ShowResult(result);
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ClearControls()
        {
            txtUsername.Text = txtPassword.Text = txtEmail.Text = "";
            DdlEmployeeId.SelectedIndex = 0;
            ddlRole.SelectedIndex = 0;
            chkIsActive.Checked = true;
            lblMessage.Style["display"] = "none";
        }

        private void ShowResult(int rowsAffected)
        {
            ShowMessage(rowsAffected > 0 ? "Success" : "No records affected.", rowsAffected > 0 ? "success" : "warning");
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }

        private void GetEmployees()
        {
            try
            {
                DataTable dt = objUserMaster.GetEmployees(); ;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DdlEmployeeId.DataSource = dt;
                    DdlEmployeeId.DataTextField = "emp_name";
                    DdlEmployeeId.DataValueField = "emp_code";
                    DdlEmployeeId.DataBind();
                    DdlEmployeeId.Items.Insert(0, new ListItem("-- Select --", "0"));
                    ViewState["dtEmployees"] = dt; // Store employees in ViewState for later use
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void grdUserMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
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

                if (e.Row.RowType == DataControlRowType.DataRow && grdUserMaster.EditIndex == e.Row.RowIndex)
                {
                    DropDownList GrdDdlEmployeeId = (DropDownList)e.Row.Cells[2].FindControl("GrdDdlEmployeeId");
                    GrdDdlEmployeeId.DataSource = (DataTable)ViewState["dtEmployees"]; // Method to fetch your data
                    GrdDdlEmployeeId.DataTextField = "emp_name";
                    GrdDdlEmployeeId.DataValueField = "emp_code";
                    GrdDdlEmployeeId.DataBind();
                    GrdDdlEmployeeId.Items.Insert(0, new ListItem("-- Select --", ""));

                    DropDownList GrdDdlRole = (DropDownList)e.Row.FindControl("GrdDdlRole");
                    GrdDdlRole.DataSource = (DataTable)ViewState["dtRoles"]; // Method to fetch your data
                    GrdDdlRole.DataTextField = "role_name";
                    GrdDdlRole.DataValueField = "role_id";
                    GrdDdlRole.DataBind();
                    GrdDdlRole.Items.Insert(0, new ListItem("-- Select --", ""));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
