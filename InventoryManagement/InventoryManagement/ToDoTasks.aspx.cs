using InventoryManagement.IL;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace InventoryManagement
{
    public partial class ToDoTasks : Page
    {
        readonly ClsToDoTask objToDoTask = new ClsToDoTask();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindTasks();
            }
        }

        private void BindTasks()
        {
            try
            {
                DataTable dt = objToDoTask.GetActiveTasks();
                if (dt.Rows.Count > 0)
                {
                    rptTasks.DataSource = dt;
                    rptTasks.DataBind();
                    lblNoTasks.Visible = false;
                }
                else
                {
                    rptTasks.DataSource = null;
                    rptTasks.DataBind();
                    lblNoTasks.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading tasks: " + ex.Message, "danger");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    ShowMessage("Task title is required.", "warning");
                    return;
                }

                // Parse end date if provided
                DateTime? endDate = null;
                if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    if (DateTime.TryParse(txtEndDate.Text, out DateTime parsedDate))
                    {
                        endDate = parsedDate;
                    }
                }

                // Check if editing or creating
                if (ViewState["EditTaskID"] != null && int.TryParse(ViewState["EditTaskID"].ToString(), out var editId) && editId > 0)
                {
                    // Update existing task
                    var objTask = new ClsToDoTask
                    {
                        TASK_ID = editId,
                        TITLE = txtTitle.Text.Trim(),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        END_DATE = endDate,
                        COMPLETED = false // Keep current status
                    };

                    int result = objToDoTask.UpdateTask(objTask);
                    ShowResult(result, "Task updated successfully.");
                    if (result > 0)
                    {
                        ClearControls();
                        BindTasks();
                    }
                }
                else
                {
                    // Create new task
                    var objTask = new ClsToDoTask
                    {
                        TITLE = txtTitle.Text.Trim(),
                        DESCRIPTION = txtDescription.Text.Trim(),
                        END_DATE = endDate,
                        COMPLETED = false
                    };

                    int result = objToDoTask.CreateTask(objTask);
                    ShowResult(result, "Task created successfully.");
                    if (result > 0)
                    {
                        ClearControls();
                        BindTasks();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        protected void rptTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                int taskId = Convert.ToInt32(e.CommandArgument);

                if (e.CommandName == "Toggle")
                {
                    // Toggle task completion status
                    int result = objToDoTask.ToggleTaskCompletion(taskId);
                    if (result > 0)
                    {
                        BindTasks();
                        ShowMessage("Task status updated.", "success");
                    }
                }
                else if (e.CommandName == "Edit")
                {
                    // Load task for editing
                    DataTable dt = objToDoTask.GetTaskById(taskId);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        txtTitle.Text = r["title"].ToString();
                        txtDescription.Text = r["description"].ToString();

                        // Load end date if available
                        if (r["end_date"] != DBNull.Value && DateTime.TryParse(r["end_date"].ToString(), out DateTime endDate))
                        {
                            txtEndDate.Text = endDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtEndDate.Text = "";
                        }

                        ViewState["EditTaskID"] = taskId;
                        btnSave.Text = "Update Task";
                        ShowMessage("Editing task. Click 'Update Task' to save changes.", "info");
                    }
                }
                else if (e.CommandName == "Delete")
                {
                    // Delete task
                    var objTask = new ClsToDoTask { TASK_ID = taskId };
                    int result = objToDoTask.DeleteTask(objTask);
                    ShowResult(result, "Task deleted successfully.");
                    if (result > 0)
                    {
                        BindTasks();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error: " + ex.Message, "danger");
            }
        }

        private void ClearControls()
        {
            txtTitle.Text = txtDescription.Text = txtEndDate.Text = "";
            ViewState["EditTaskID"] = null;
            btnSave.Text = "Save Task";
            lblMessage.Style["display"] = "none";
        }

        private void ShowResult(int rowsAffected, string successMessage)
        {
            ShowMessage(rowsAffected > 0 ? successMessage : "No records affected.", rowsAffected > 0 ? "success" : "warning");
        }

        private void ShowMessage(string message, string alertType = "info")
        {
            lblMessage.Text = message;
            lblMessage.CssClass = $"alert alert-{alertType} mt-3";
            lblMessage.Style["display"] = "block";
        }
    }
}
