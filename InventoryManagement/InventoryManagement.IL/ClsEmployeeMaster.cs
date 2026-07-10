using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsEmployeeMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string DESIGNATION { get; set; }
        public string DEPARTMENT { get; set; }
        public string MOBILE_NO { get; set; }
        public string EMAIL_ID { get; set; }
        public string ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string PINCODE { get; set; }
        public DateTime? JOIN_DATE { get; set; }
        public decimal? SALARY { get; set; }
        public string STATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public DataTable GetEmployeeMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT emp_code, emp_name, designation, department, mobile_no, email_id, ");
                sqlQueryBuilder.Append("address, city, state, pincode, join_date, salary, status ");
                sqlQueryBuilder.Append("FROM employee_master ORDER BY emp_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateEmployee(ClsEmployeeMaster objEmployeeMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO employee_master (emp_name, designation, department, mobile_no, email_id, ");
                sqlQueryBuilder.Append("address, city, state, pincode, join_date, salary, status, created_by, created_at) ");
                sqlQueryBuilder.Append("VALUES (@emp_name, @designation, @department, @mobile_no, @email_id, @address, ");
                sqlQueryBuilder.Append("@city, @state, @pincode, @join_date, @salary, @status, @created_by, @created_at)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@emp_name", objEmployeeMaster.EMP_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@designation", string.IsNullOrWhiteSpace(objEmployeeMaster.DESIGNATION) ? DBNull.Value : (object)objEmployeeMaster.DESIGNATION);
                objUtility.sqlCommand.Parameters.AddWithValue("@department", string.IsNullOrWhiteSpace(objEmployeeMaster.DEPARTMENT) ? DBNull.Value : (object)objEmployeeMaster.DEPARTMENT);
                objUtility.sqlCommand.Parameters.AddWithValue("@mobile_no", string.IsNullOrWhiteSpace(objEmployeeMaster.MOBILE_NO) ? DBNull.Value : (object)objEmployeeMaster.MOBILE_NO);
                objUtility.sqlCommand.Parameters.AddWithValue("@email_id", string.IsNullOrWhiteSpace(objEmployeeMaster.EMAIL_ID) ? DBNull.Value : (object)objEmployeeMaster.EMAIL_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(objEmployeeMaster.ADDRESS) ? DBNull.Value : (object)objEmployeeMaster.ADDRESS);
                objUtility.sqlCommand.Parameters.AddWithValue("@city", string.IsNullOrWhiteSpace(objEmployeeMaster.CITY) ? DBNull.Value : (object)objEmployeeMaster.CITY);
                objUtility.sqlCommand.Parameters.AddWithValue("@state", string.IsNullOrWhiteSpace(objEmployeeMaster.STATE) ? DBNull.Value : (object)objEmployeeMaster.STATE);
                objUtility.sqlCommand.Parameters.AddWithValue("@pincode", string.IsNullOrWhiteSpace(objEmployeeMaster.PINCODE) ? DBNull.Value : (object)objEmployeeMaster.PINCODE);
                objUtility.sqlCommand.Parameters.AddWithValue("@join_date", objEmployeeMaster.JOIN_DATE.HasValue ? (object)objEmployeeMaster.JOIN_DATE : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@salary", objEmployeeMaster.SALARY.HasValue ? (object)objEmployeeMaster.SALARY : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@status", objEmployeeMaster.STATUS ?? "ACTIVE");
                objUtility.sqlCommand.Parameters.AddWithValue("@created_by", string.IsNullOrWhiteSpace(objEmployeeMaster.CREATED_BY) ? DBNull.Value : (object)objEmployeeMaster.CREATED_BY);
                objUtility.sqlCommand.Parameters.AddWithValue("@created_at", objEmployeeMaster.CREATED_AT.HasValue ? (object)objEmployeeMaster.CREATED_AT : DBNull.Value);

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

        public int UpdateEmployee(ClsEmployeeMaster objEmployeeMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE employee_master SET emp_name = @emp_name, designation = @designation, ");
                sqlQueryBuilder.Append("department = @department, mobile_no = @mobile_no, email_id = @email_id, ");
                sqlQueryBuilder.Append("address = @address, city = @city, state = @state, pincode = @pincode, ");
                sqlQueryBuilder.Append("join_date = @join_date, salary = @salary, status = @status WHERE emp_code = @emp_code");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@emp_code", objEmployeeMaster.EMP_CODE);
                objUtility.sqlCommand.Parameters.AddWithValue("@emp_name", objEmployeeMaster.EMP_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@designation", string.IsNullOrWhiteSpace(objEmployeeMaster.DESIGNATION) ? DBNull.Value : (object)objEmployeeMaster.DESIGNATION);
                objUtility.sqlCommand.Parameters.AddWithValue("@department", string.IsNullOrWhiteSpace(objEmployeeMaster.DEPARTMENT) ? DBNull.Value : (object)objEmployeeMaster.DEPARTMENT);
                objUtility.sqlCommand.Parameters.AddWithValue("@mobile_no", string.IsNullOrWhiteSpace(objEmployeeMaster.MOBILE_NO) ? DBNull.Value : (object)objEmployeeMaster.MOBILE_NO);
                objUtility.sqlCommand.Parameters.AddWithValue("@email_id", string.IsNullOrWhiteSpace(objEmployeeMaster.EMAIL_ID) ? DBNull.Value : (object)objEmployeeMaster.EMAIL_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(objEmployeeMaster.ADDRESS) ? DBNull.Value : (object)objEmployeeMaster.ADDRESS);
                objUtility.sqlCommand.Parameters.AddWithValue("@city", string.IsNullOrWhiteSpace(objEmployeeMaster.CITY) ? DBNull.Value : (object)objEmployeeMaster.CITY);
                objUtility.sqlCommand.Parameters.AddWithValue("@state", string.IsNullOrWhiteSpace(objEmployeeMaster.STATE) ? DBNull.Value : (object)objEmployeeMaster.STATE);
                objUtility.sqlCommand.Parameters.AddWithValue("@pincode", string.IsNullOrWhiteSpace(objEmployeeMaster.PINCODE) ? DBNull.Value : (object)objEmployeeMaster.PINCODE);
                objUtility.sqlCommand.Parameters.AddWithValue("@join_date", objEmployeeMaster.JOIN_DATE.HasValue ? (object)objEmployeeMaster.JOIN_DATE : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@salary", objEmployeeMaster.SALARY.HasValue ? (object)objEmployeeMaster.SALARY : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@status", objEmployeeMaster.STATUS ?? "ACTIVE");

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

        public int DeleteEmployee(ClsEmployeeMaster objEmployeeMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM employee_master WHERE emp_code = @emp_code");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@emp_code", objEmployeeMaster.EMP_CODE);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (MySqlException ex)
            {
                objUtility.RollbackTransaction();
                // Check for foreign key constraint violation (error code 1451)
                if (ex.Number == 1451 || ex.Message.Contains("foreign key constraint"))
                {
                    throw new Exception("Cannot delete this employee. This employee is referenced in other records (e.g., deliveries, orders). Please remove those references first.");
                }
                throw;
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
