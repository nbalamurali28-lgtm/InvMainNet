using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsProductGSTMapping
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int PRODUCT_ID { get; set; }
        public string GST_ID { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public string PRODUCT_NAME { get; set; }
        public decimal? GST_PERCENTAGE { get; set; }

        public DataTable GetProductGSTMappings()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT pgm.ProductID, pgm.gst_id, p.ProductName, gm.gst_percentage ");
                sqlQueryBuilder.Append("FROM Product_GST_Mapping pgm ");
                sqlQueryBuilder.Append("LEFT JOIN Product p ON pgm.ProductID = p.ProductID ");
                sqlQueryBuilder.Append("LEFT JOIN gst_master gm ON pgm.gst_id = gm.gst_id ");
                sqlQueryBuilder.Append("ORDER BY p.ProductName");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetProductsWithGST()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT DISTINCT p.ProductID, p.ProductName FROM Product p ");
                sqlQueryBuilder.Append("LEFT JOIN Product_GST_Mapping pgm ON p.ProductID = pgm.ProductID ");
                sqlQueryBuilder.Append("ORDER BY p.ProductName");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetAvailableGST()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT gst_id, gst_percentage, CONCAT(gst_percentage, '%') as gst_display FROM gst_master ");
                sqlQueryBuilder.Append("WHERE is_active = true ORDER BY gst_percentage");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateOrUpdateMapping(ClsProductGSTMapping objMapping)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                // Check if mapping exists
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT COUNT(*) FROM Product_GST_Mapping WHERE ProductID = @product_id");
                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objMapping.PRODUCT_ID);

                object scalarResult = objUtility.sqlCommand.ExecuteScalar();
                int existingCount = scalarResult != null ? Convert.ToInt32(scalarResult) : 0;

                if (existingCount > 0)
                {
                    // Update existing mapping
                    sqlQueryBuilder = new StringBuilder();
                    sqlQueryBuilder.Append("UPDATE Product_GST_Mapping SET gst_id = @gst_id ");
                    sqlQueryBuilder.Append("WHERE ProductID = @product_id");

                    objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtility.sqlCommand.Parameters.Clear();
                    objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objMapping.PRODUCT_ID);
                    objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objMapping.GST_ID);
                }
                else
                {
                    // Insert new mapping
                    sqlQueryBuilder = new StringBuilder();
                    sqlQueryBuilder.Append("INSERT INTO Product_GST_Mapping (ProductID, gst_id) ");
                    sqlQueryBuilder.Append("VALUES (@product_id, @gst_id)");

                    objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtility.sqlCommand.Parameters.Clear();
                    objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objMapping.PRODUCT_ID);
                    objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objMapping.GST_ID);
                }

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

        public int DeleteMapping(ClsProductGSTMapping objMapping)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM Product_GST_Mapping WHERE ProductID = @product_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objMapping.PRODUCT_ID);
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

        public int BulkUpdateMappings(DataTable dtMappings)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                foreach (DataRow row in dtMappings.Rows)
                {
                    int productId = Convert.ToInt32(row["ProductID"]);
                    string gstId = row["GST_ID"] != DBNull.Value ? row["GST_ID"].ToString() : null;

                    if (!string.IsNullOrEmpty(gstId))
                    {
                        var mapping = new ClsProductGSTMapping
                        {
                            PRODUCT_ID = productId,
                            GST_ID = gstId,
                            UPDATED_BY = row["UpdatedBy"]?.ToString(),
                            UPDATED_AT = DateTime.Now
                        };
                        CreateOrUpdateMapping(mapping);
                    }
                    else
                    {
                        var mapping = new ClsProductGSTMapping { PRODUCT_ID = productId };
                        DeleteMapping(mapping);
                    }
                }

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
