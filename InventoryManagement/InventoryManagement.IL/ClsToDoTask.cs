using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsToDoTask
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int TASK_ID { get; set; }
        public string TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime? END_DATE { get; set; }
        public bool COMPLETED { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public DataTable GetAllTasks()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT task_id, title, description, end_date, completed, created_at ");
                sqlQueryBuilder.Append("FROM todo_tasks ORDER BY created_at DESC");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetActiveTasks()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT task_id, title, description, end_date, completed, created_at ");
                sqlQueryBuilder.Append("FROM todo_tasks WHERE completed = 0 ORDER BY ");
                sqlQueryBuilder.Append("CASE WHEN end_date IS NULL THEN 1 ELSE 0 END, end_date ASC, created_at DESC");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetTaskById(int taskId)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT task_id, title, description, end_date, completed, created_at ");
                sqlQueryBuilder.Append("FROM todo_tasks WHERE task_id = @task_id");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@task_id", taskId);
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateTask(ClsToDoTask objTask)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO todo_tasks (title, description, end_date, completed) ");
                sqlQueryBuilder.Append("VALUES (@title, @description, @end_date, @completed)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@title", objTask.TITLE ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", objTask.DESCRIPTION ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@end_date", objTask.END_DATE.HasValue ? (object)objTask.END_DATE.Value : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@completed", objTask.COMPLETED);

                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }

        public int UpdateTask(ClsToDoTask objTask)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE todo_tasks SET title = @title, description = @description, ");
                sqlQueryBuilder.Append("end_date = @end_date, completed = @completed WHERE task_id = @task_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@task_id", objTask.TASK_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@title", objTask.TITLE ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", objTask.DESCRIPTION ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@end_date", objTask.END_DATE.HasValue ? (object)objTask.END_DATE.Value : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@completed", objTask.COMPLETED);

                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }

        public int ToggleTaskCompletion(int taskId)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE todo_tasks SET completed = NOT completed WHERE task_id = @task_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@task_id", taskId);

                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }

        public int DeleteTask(ClsToDoTask objTask)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM todo_tasks WHERE task_id = @task_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@task_id", objTask.TASK_ID);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (Exception)
            {
                objUtility.RollbackTransaction();
                throw;
            }
            return rowsAffected;
        }
    }
}
