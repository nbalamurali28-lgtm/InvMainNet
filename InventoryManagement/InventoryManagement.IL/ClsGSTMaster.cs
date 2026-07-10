using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsGSTMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public string GST_ID { get; set; }
        public decimal GST_PERCENTAGE { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime? EFFECTIVE_FROM { get; set; }
        public DateTime? EFFECTIVE_TO { get; set; }
        public bool IS_ACTIVE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }

        public DataTable GetGSTMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT gst_id, gst_percentage, description, effective_from, effective_to, is_active ");
                sqlQueryBuilder.Append("FROM gst_master ORDER BY gst_percentage");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateGST(ClsGSTMaster objGSTMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO gst_master (gst_percentage, description, effective_from, effective_to, is_active) ");
                sqlQueryBuilder.Append("VALUES (@gst_percentage, @description, @effective_from, @effective_to, @is_active)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_percentage", objGSTMaster.GST_PERCENTAGE);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(objGSTMaster.DESCRIPTION) ? DBNull.Value : (object)objGSTMaster.DESCRIPTION);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_from", objGSTMaster.EFFECTIVE_FROM.HasValue ? (object)objGSTMaster.EFFECTIVE_FROM : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_to", objGSTMaster.EFFECTIVE_TO.HasValue ? (object)objGSTMaster.EFFECTIVE_TO : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objGSTMaster.IS_ACTIVE);

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

        public int UpdateGST(ClsGSTMaster objGSTMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE gst_master SET gst_percentage = @gst_percentage, description = @description, ");
                sqlQueryBuilder.Append("effective_from = @effective_from, effective_to = @effective_to, is_active = @is_active WHERE gst_id = @gst_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objGSTMaster.GST_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_percentage", objGSTMaster.GST_PERCENTAGE);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(objGSTMaster.DESCRIPTION) ? DBNull.Value : (object)objGSTMaster.DESCRIPTION);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_from", objGSTMaster.EFFECTIVE_FROM.HasValue ? (object)objGSTMaster.EFFECTIVE_FROM : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_to", objGSTMaster.EFFECTIVE_TO.HasValue ? (object)objGSTMaster.EFFECTIVE_TO : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objGSTMaster.IS_ACTIVE);

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

        public int DeleteGST(ClsGSTMaster objGSTMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM gst_master WHERE gst_id = @gst_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objGSTMaster.GST_ID);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (MySqlException ex)
            {
                objUtility.RollbackTransaction();
                // Check for foreign key constraint violation (error code 1451)
                if (ex.Number == 1451 || ex.Message.Contains("foreign key constraint"))
                {
                    throw new Exception("Cannot delete this GST rate. This GST is referenced by products or transactions. Please remove those references first.");
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
