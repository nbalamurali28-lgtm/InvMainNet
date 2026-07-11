using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                    USERNAME = txtUsername.Text.Trim(),
                    EMPLOYEE_ID = DdlEmployeeId.SelectedValue.Trim(),
                    EMAIL = txtEmail.Text.Trim(),
                    ROLE_ID = ddlRole.SelectedValue,
                    IS_ACTIVE = chkIsActive.Checked
                };

                int result = 0;
                if (string.IsNullOrEmpty(hfUserId.Value))
                {
                    // Create new user
                    if (string.IsNullOrEmpty(txtPassword.Text))
                    {
                        ShowMessage("Password is required.", "warning");
                        return;
                    }
                    objUser.USER_ID = "U" + DateTime.Now.Ticks.ToString().Substring(DateTime.Now.Ticks.ToString().Length - 8);
                    objUser.PASSWORD = txtPassword.Text;
                    result = objUserMaster.CreateUser(objUser);
                }
                else
                {
                    // Update existing user
                    objUser.USER_ID = hfUserId.Value;
                    if (!string.IsNullOrEmpty(txtPassword.Text))
                    {
                        objUser.PASSWORD = txtPassword.Text;
                    }
                    result = objUserMaster.UpdateUser(objUser);
                }

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

        protected void grdUserMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                string userId = e.CommandArgument.ToString();
                LoadUserForEdit(userId);
            }
        }

        private void LoadUserForEdit(string userId)
        {
            try
            {
                DataTable dt = objUserMaster.GetUserMaster();
                DataRow[] rows = dt.Select($"user_id = '{userId}'");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    hfUserId.Value = row["user_id"].ToString();
                    txtUsername.Text = row["username"].ToString();
                    txtPassword.Text = ""; // Don't populate password for security

                    // Handle employee_id - set to first item (-- Select --) if blank
                    string employeeId = row["employee_id"].ToString();
                    if (!string.IsNullOrEmpty(employeeId) && DdlEmployeeId.Items.FindByValue(employeeId) != null)
                    {
                        DdlEmployeeId.SelectedValue = employeeId;
                    }
                    else
                    {
                        DdlEmployeeId.SelectedIndex = 0; // Select "-- Select --"
                    }

                    // Handle role_id
                    string roleId = row["role_id"].ToString();
                    if (!string.IsNullOrEmpty(roleId) && ddlRole.Items.FindByValue(roleId) != null)
                    {
                        ddlRole.SelectedValue = roleId;
                    }
                    else
                    {
                        ddlRole.SelectedIndex = 0; // Select "-- Select --"
                    }

                    txtEmail.Text = row["email"].ToString();
                    chkIsActive.Checked = Convert.ToBoolean(row["is_active"]);

                    ShowMessage("Edit mode: Modify the user details and click Save to update.", "info");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading user: " + ex.Message, "danger");
            }
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
            hfUserId.Value = "";
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
                    // Set the checkbox state based on is_active value
                    var chkIsActive = e.Row.FindControl("GrdChkBoxIsActive") as System.Web.UI.HtmlControls.HtmlInputCheckBox;
                    if (chkIsActive != null)
                    {
                        DataRowView drv = (DataRowView)e.Row.DataItem;
                        chkIsActive.Checked = Convert.ToBoolean(drv["is_active"]);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
