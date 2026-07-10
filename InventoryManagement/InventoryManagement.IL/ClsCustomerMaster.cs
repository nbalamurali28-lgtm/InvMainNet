using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsCustomerMaster
    {
        readonly ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public string CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string ADDRESS { get; set; }
        public string GST_NUMBER { get; set; }
        public string AREA_ID { get; set; }
        public int IS_ACTIVE { get; set; }
        public string CREATED_AT { get; set; }
        public string UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public DataTable GetCustomerMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT c.customer_id, c.customer_name, c.contact_person, c.email, c.phone, c.address, ");
                sqlQueryBuilder.Append("c.gst_number, c.area_id, a.area_name, c.is_active ");
                sqlQueryBuilder.Append("FROM customer_master c LEFT JOIN area_master a ON c.area_id = a.area_id ");
                sqlQueryBuilder.Append("ORDER BY c.customer_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public DataTable GetAreaList()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT area_id, area_name FROM area_master ORDER BY area_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public string GetNextCustomerId()
        {
            string nextId = "CUST000001";
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT customer_id FROM customer_master");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                DataTable dt = objUtilitiy.GetDataTable(objMySqlCommand);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int maxIdNumber = 0;
                    string prefix = "CUST";

                    foreach (DataRow row in dt.Rows)
                    {
                        string customerId = row["customer_id"]?.ToString();
                        if (string.IsNullOrWhiteSpace(customerId))
                        {
                            continue;
                        }

                        int index = customerId.Length;
                        while (index > 0 && char.IsDigit(customerId[index - 1]))
                        {
                            index--;
                        }

                        string currentPrefix = customerId.Substring(0, index);
                        string numericPart = customerId.Substring(index);

                        if (!string.IsNullOrEmpty(numericPart) && int.TryParse(numericPart, out int currentNumber))
                        {
                            if (currentNumber > maxIdNumber)
                            {
                                maxIdNumber = currentNumber;
                                prefix = currentPrefix;
                            }
                        }
                    }

                    if (maxIdNumber > 0)
                    {
                        nextId = prefix + (maxIdNumber + 1).ToString("D6");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return nextId;
        }

        private bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @tableName AND LOWER(COLUMN_NAME) = LOWER(@columnName)");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@tableName", tableName);
                objMySqlCommand.Parameters.AddWithValue("@columnName", columnName);

                DataTable dt = objUtilitiy.GetDataTable(objMySqlCommand);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0]) > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return false;
        }

        private string GetExistingColumnName(string tableName, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (ColumnExists(tableName, columnName))
                {
                    return columnName;
                }
            }

            return null;
        }

        public int CreateCustomer(ClsCustomerMaster objCustomerMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();
                objUtilitiy.sqlCommand.Parameters.Clear();

                string createdByColumn = GetExistingColumnName("customer_master", "created_by", "CreatedBy");
                bool hasCreatedBy = !string.IsNullOrEmpty(createdByColumn);

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO customer_master (customer_id, customer_name, contact_person, email, phone, address, gst_number, area_id, is_active, created_at");
                if (hasCreatedBy)
                {
                    sqlQueryBuilder.Append(", " + createdByColumn);
                }
                sqlQueryBuilder.Append(") VALUES (@customer_id, @customer_name, @contact_person, @email, @phone, @address, @gst_number, @area_id, @is_active, @created_at");
                if (hasCreatedBy)
                {
                    sqlQueryBuilder.Append(", @created_by");
                }
                sqlQueryBuilder.Append(");");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@customer_id", objCustomerMaster.CUSTOMER_ID ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@customer_name", objCustomerMaster.CUSTOMER_NAME ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@contact_person", string.IsNullOrWhiteSpace(objCustomerMaster.CONTACT_PERSON) ? DBNull.Value : (object)objCustomerMaster.CONTACT_PERSON);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(objCustomerMaster.EMAIL) ? DBNull.Value : (object)objCustomerMaster.EMAIL);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(objCustomerMaster.PHONE) ? DBNull.Value : (object)objCustomerMaster.PHONE);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(objCustomerMaster.ADDRESS) ? DBNull.Value : (object)objCustomerMaster.ADDRESS);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@gst_number", string.IsNullOrWhiteSpace(objCustomerMaster.GST_NUMBER) ? DBNull.Value : (object)objCustomerMaster.GST_NUMBER);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_id", string.IsNullOrWhiteSpace(objCustomerMaster.AREA_ID) ? DBNull.Value : (object)objCustomerMaster.AREA_ID);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@is_active", objCustomerMaster.IS_ACTIVE);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objCustomerMaster.CREATED_AT ?? (object)DBNull.Value);
                if (hasCreatedBy)
                {
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_by", string.IsNullOrWhiteSpace(objCustomerMaster.CREATED_BY) ? DBNull.Value : (object)objCustomerMaster.CREATED_BY);
                }

                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
                throw;
            }

            return rowsAffected;
        }

        public int UpdateCustomer(ClsCustomerMaster objCustomerMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();
                objUtilitiy.sqlCommand.Parameters.Clear();

                string updatedByColumn = GetExistingColumnName("customer_master", "updated_by", "UpdatedBy");
                bool hasUpdatedBy = !string.IsNullOrEmpty(updatedByColumn);

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE customer_master SET customer_name = @customer_name, contact_person = @contact_person, email = @email, phone = @phone, ");
                sqlQueryBuilder.Append("address = @address, gst_number = @gst_number, area_id = @area_id, is_active = @is_active, updated_at = @updated_at");
                if (hasUpdatedBy)
                {
                    sqlQueryBuilder.Append(", " + updatedByColumn + " = @updated_by");
                }
                sqlQueryBuilder.Append(" WHERE customer_id = @customer_id");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@customer_id", objCustomerMaster.CUSTOMER_ID ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@customer_name", objCustomerMaster.CUSTOMER_NAME ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@contact_person", objCustomerMaster.CONTACT_PERSON ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@email", objCustomerMaster.EMAIL ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@phone", objCustomerMaster.PHONE ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@address", objCustomerMaster.ADDRESS ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@gst_number", objCustomerMaster.GST_NUMBER ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_id", objCustomerMaster.AREA_ID ?? string.Empty);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@is_active", objCustomerMaster.IS_ACTIVE);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@updated_at", objCustomerMaster.UPDATED_AT ?? (object)DBNull.Value);
                if (hasUpdatedBy)
                {
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@updated_by", string.IsNullOrWhiteSpace(objCustomerMaster.UPDATED_BY) ? DBNull.Value : (object)objCustomerMaster.UPDATED_BY);
                }

                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
                throw;
            }

            return rowsAffected;
        }

        public int DeleteCustomer(ClsCustomerMaster objCustomerMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM customer_master WHERE customer_id = @customer_id");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@customer_id", objCustomerMaster.CUSTOMER_ID ?? string.Empty);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
            }

            return rowsAffected;
        }
    }
}
