using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class RoleMaster : Page
    {
        readonly ClsRoleMaster objRoleMaster = new ClsRoleMaster();

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
                DataTable dt = objRoleMaster.GetRoleMaster();
                grdRoleMaster.DataSource = dt;
                grdRoleMaster.DataBind();
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
                if (string.IsNullOrWhiteSpace(txtRoleId.Text) || string.IsNullOrWhiteSpace(txtRoleName.Text))
                {
                    ShowMessage("Role ID and Role Name are required.", "warning");
                    return;
                }

                // If editing existing role, update; otherwise create new
                if (ViewState["EditRoleID"] != null && !string.IsNullOrEmpty(ViewState["EditRoleID"].ToString()))
                {
                    var objRole = new ClsRoleMaster
                    {
                        ROLE_ID = ViewState["EditRoleID"].ToString(),
                        ROLE_NAME = txtRoleName.Text.Trim(),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        IS_ACTIVE = chkIsActive.Checked,
                        UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                        UPDATED_AT = DateTime.Now
                    };

                    int result = objRoleMaster.UpdateRole(objRole);
                    ShowResult(result);
                    if (result > 0)
                    {
                        ClearControls();
                        BindGridView();
                    }
                }
                else
                {
                    var objRole = new ClsRoleMaster
                    {
                        ROLE_ID = txtRoleId.Text.Trim(),
                        ROLE_NAME = txtRoleName.Text.Trim(),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        IS_ACTIVE = chkIsActive.Checked,
                        CREATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                        CREATED_AT = DateTime.Now
                    };

                    int result = objRoleMaster.CreateRole(objRole);
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

        protected void btnReset_Click(object sender, EventArgs e) => ClearControls();

        protected void grdRoleMaster_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // Prevent inline GridView edit; populate main form for editing
                e.Cancel = true;
                string roleId = grdRoleMaster.DataKeys[e.NewEditIndex].Value.ToString();
                DataTable dt = objRoleMaster.GetRoleMaster();
                DataRow[] rows = dt.Select("role_id = '" + roleId.Replace("'", "''") + "'");
                if (rows.Length > 0)
                {
                    var r = rows[0];
                    txtRoleId.Text = r["role_id"] != DBNull.Value ? r["role_id"].ToString() : "";
                    txtRoleName.Text = r["role_name"] != DBNull.Value ? r["role_name"].ToString() : "";
                    txtRoleId.ReadOnly = true; // Role ID should not be editable during update

                    ViewState["EditRoleID"] = roleId;
                    btnSave.Text = "Update";
                    lblMessage.Style["display"] = "none";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error preparing role for edit: " + ex.Message, "danger");
            }
        }

        protected void grdRoleMaster_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string roleId = grdRoleMaster.DataKeys[e.RowIndex].Value.ToString();
                var objRole = new ClsRoleMaster
                {
                    ROLE_ID = roleId,
                    ROLE_NAME = ((TextBox)grdRoleMaster.Rows[e.RowIndex].Cells[1].Controls[0]).Text.Trim(),
                    UPDATED_BY = Session["UserID"]?.ToString() ?? "SYSTEM",
                    UPDATED_AT = DateTime.Now
                };

                int result = objRoleMaster.UpdateRole(objRole);
                ShowResult(result);
                grdRoleMaster.EditIndex = -1;
                BindGridView();
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void grdRoleMaster_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdRoleMaster.EditIndex = -1;
            BindGridView();
        }

        protected void grdRoleMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string roleId = grdRoleMaster.DataKeys[e.RowIndex].Value.ToString();
                var objRole = new ClsRoleMaster { ROLE_ID = roleId };
                int result = objRoleMaster.DeleteRole(objRole);
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
            txtRoleId.Text = txtRoleName.Text = txtDescription.Text = "";
            chkIsActive.Checked = true;
            txtRoleId.ReadOnly = false;
            ViewState["EditRoleID"] = null;
            btnSave.Text = "Save";
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
    }
}
