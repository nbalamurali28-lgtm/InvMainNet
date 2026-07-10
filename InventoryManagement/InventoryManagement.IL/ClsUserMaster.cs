using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsUserMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public string USER_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string ROLE_ID { get; set; }
        public string EMAIL { get; set; }
        public string EMPLOYEE_ID { get; set; }
        public bool IS_ACTIVE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public string ROLE_NAME { get; set; }

        public DataTable GetUserMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT u.user_id, u.username, u.role_id, r.role_name, u.email, u.employee_id, u.is_active ");
                sqlQueryBuilder.Append("FROM user_master u ");
                sqlQueryBuilder.Append("LEFT JOIN role_master r ON u.role_id = r.role_id ");
                sqlQueryBuilder.Append("ORDER BY u.username");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetRoles()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT role_id, role_name FROM role_master WHERE is_active = 1 ORDER BY role_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetEmployees()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT emp_code, CONCAT(`emp_code`, ' - ',`emp_name`) AS emp_name FROM employee_master WHERE status = 'ACTIVE' ORDER BY emp_code");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public int CreateUser(ClsUserMaster objUserMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO user_master (user_id, username, password, role_id, email, employee_id, is_active /*, created_by, created_at*/) ");
                sqlQueryBuilder.Append("VALUES (@user_id, @username, @password, @role_id, @email, @employee_id, @is_active /*, @created_by, @created_at*/)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@user_id", objUserMaster.USER_ID ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@username", objUserMaster.USERNAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@password", objUserMaster.PASSWORD ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@role_id", objUserMaster.ROLE_ID ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(objUserMaster.EMAIL) ? DBNull.Value : (object)objUserMaster.EMAIL);
                objUtility.sqlCommand.Parameters.AddWithValue("@employee_id", objUserMaster.EMPLOYEE_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objUserMaster.IS_ACTIVE);
                //objUtility.sqlCommand.Parameters.AddWithValue("@created_by", string.IsNullOrWhiteSpace(objUserMaster.CREATED_BY) ? DBNull.Value : (object)objUserMaster.CREATED_BY);
                //objUtility.sqlCommand.Parameters.AddWithValue("@created_at", objUserMaster.CREATED_AT.HasValue ? (object)objUserMaster.CREATED_AT : DBNull.Value);

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

        public int UpdateUser(ClsUserMaster objUserMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE user_master SET username = @username, role_id = @role_id, email = @email, ");
                sqlQueryBuilder.Append("employee_id = @employee_id, is_active = @is_active /*, updated_by = @updated_by, updated_at = @updated_at*/ ");
                sqlQueryBuilder.Append("WHERE user_id = @user_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@user_id", objUserMaster.USER_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@username", objUserMaster.USERNAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@role_id", objUserMaster.ROLE_ID ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(objUserMaster.EMAIL) ? DBNull.Value : (object)objUserMaster.EMAIL);
                objUtility.sqlCommand.Parameters.AddWithValue("@employee_id", objUserMaster.EMPLOYEE_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objUserMaster.IS_ACTIVE);
                //objUtility.sqlCommand.Parameters.AddWithValue("@updated_by", string.IsNullOrWhiteSpace(objUserMaster.UPDATED_BY) ? DBNull.Value : (object)objUserMaster.UPDATED_BY);
                //objUtility.sqlCommand.Parameters.AddWithValue("@updated_at", objUserMaster.UPDATED_AT.HasValue ? (object)objUserMaster.UPDATED_AT : DBNull.Value);

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

        public int DeleteUser(ClsUserMaster objUserMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM user_master WHERE user_id = @user_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@user_id", objUserMaster.USER_ID);
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
