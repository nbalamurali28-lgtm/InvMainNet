<%@ Page Title="To-Do Tasks" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ToDoTasks.aspx.cs" Inherits="InventoryManagement.ToDoTasks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Load jQuery first -->
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <!-- Load Popper.js (required for Bootstrap dropdowns) -->
    <script src="Scripts/popper.min.js"></script>
    <!-- Load Bootstrap JavaScript -->
    <script src="Scripts/bootstrap.bundle.min.js"></script>
    <style type="text/css">
        .pageBody {
            width: 98%;
            margin: 1% 0% 0% 1%;
        }
        .task-card {
            border: 1px solid #dee2e6;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background-color: #fff;
            transition: box-shadow 0.3s ease;
        }
        .task-card:hover {
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        .task-card.completed {
            background-color: #f8f9fa;
            opacity: 0.8;
        }
        .task-card.completed .task-title {
            text-decoration: line-through;
            color: #6c757d;
        }
        .task-title {
            font-size: 1.1rem;
            font-weight: 600;
            margin-bottom: 8px;
            color: #2c3e50;
        }
        .task-description {
            color: #6c757d;
            margin-bottom: 10px;
            font-size: 0.9rem;
        }
        .task-date {
            font-size: 0.8rem;
            color: #95a5a6;
        }
        .btn-complete {
            background-color: #28a745;
            border-color: #28a745;
        }
        .btn-complete:hover {
            background-color: #218838;
            border-color: #1e7e34;
        }
        .btn-incomplete {
            background-color: #6c757d;
            border-color: #6c757d;
        }
        .btn-incomplete:hover {
            background-color: #5a6268;
            border-color: #545b62;
        }
    </style>

    <main class="shadow p-3 mb-5 bg-white rounded pageBody">
        <h4 id="title"><%: Title %></h4>

        <div class="container-fluid mt-4">
            <!-- Add New Task Section -->
            <div class="row mb-4">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">Add New Task</h5>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Title *</label>
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Task title"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Description</label>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Task description"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>End Date</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary form-control" Text="Save Task" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                            </div>
                            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info mt-3" Style="display: none;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Task List Section -->
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-header bg-secondary text-white">
                            <h5 class="mb-0">My Active Tasks</h5>
                        </div>
                        <div class="card-body">
                            <asp:Repeater ID="rptTasks" runat="server" OnItemCommand="rptTasks_ItemCommand">
                                <HeaderTemplate>
                                    <div class="row">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="col-md-6 col-lg-4">
                                        <div class='task-card <%# Convert.ToBoolean(Eval("completed")) ? "completed" : "" %>'>
                                            <div class="task-title">
                                                <%# Eval("title") %>
                                            </div>
                                            <div class="task-description">
                                                <%# Eval("description") %>
                                            </div>
                                            <div class="task-date">
                                                <%# Eval("end_date") != DBNull.Value ? 
                                                    "Due: " + Convert.ToDateTime(Eval("end_date")).ToString("MMM dd, yyyy") : 
                                                    "No due date" %>
                                            </div>
                                            <div class="task-date">
                                                Created: <%# Eval("created_at", "{0:MMM dd, yyyy HH:mm}") %>
                                            </div>
                                            <div class="mt-3">
                                                <asp:LinkButton ID="btnToggle" runat="server" 
                                                    CssClass='<%# Convert.ToBoolean(Eval("completed")) ? "btn btn-sm btn-incomplete" : "btn btn-sm btn-complete" %>'
                                                    CommandName="Toggle" 
                                                    CommandArgument='<%# Eval("task_id") %>'
                                                    Text='<%# Convert.ToBoolean(Eval("completed")) ? "Mark Incomplete" : "Mark Complete" %>'>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEdit" runat="server" 
                                                    CssClass="btn btn-sm btn-info text-white ms-1" 
                                                    CommandName="Edit" 
                                                    CommandArgument='<%# Eval("task_id") %>'
                                                    Text="Edit">
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" runat="server" 
                                                    CssClass="btn btn-sm btn-danger ms-1" 
                                                    CommandName="Delete" 
                                                    CommandArgument='<%# Eval("task_id") %>'
                                                    OnClientClick="return confirm('Are you sure you want to delete this task?');"
                                                    Text="Delete">
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblNoTasks" runat="server" CssClass="text-muted" Text="No active tasks found. All tasks are completed! Create a new task above or mark completed tasks as incomplete." Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
</asp:Content>
