using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsPricingMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int PRICING_ID { get; set; }
        public int PRODUCT_ID { get; set; }
        public decimal BASE_PRICE { get; set; }
        public string GST_ID { get; set; }
        public DateTime? EFFECTIVE_FROM { get; set; }
        public DateTime? EFFECTIVE_TO { get; set; }
        public string EFFECTIVE_STATUS { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public string PRODUCT_NAME { get; set; }
        public decimal? GST_PERCENTAGE { get; set; }

        public DataTable GetPricing()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT pm.pricing_id, pm.ProductID, p.ProductName, pm.base_price, pm.gst_id, ");
                sqlQueryBuilder.Append("gm.gst_percentage, pm.effective_from, pm.effective_to, pm.effectiveStatus ");
                sqlQueryBuilder.Append("FROM pricing_master pm ");
                sqlQueryBuilder.Append("LEFT JOIN Product p ON pm.ProductID = p.ProductID ");
                sqlQueryBuilder.Append("LEFT JOIN gst_master gm ON pm.gst_id = gm.gst_id ");
                sqlQueryBuilder.Append("ORDER BY pm.effective_from DESC");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public DataTable GetPricingForDelivery(int deliveryId)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT pm.pricing_id, pm.ProductID, p.ProductName, pm.base_price, pm.gst_id, ");
                sqlQueryBuilder.Append("gm.gst_percentage FROM pricing_master pm ");
                sqlQueryBuilder.Append("LEFT JOIN Product p ON pm.ProductID = p.ProductID ");
                sqlQueryBuilder.Append("LEFT JOIN gst_master gm ON pm.gst_id = gm.gst_id ");
                sqlQueryBuilder.Append("WHERE pm.effective_from <= NOW() AND pm.effective_to >= NOW()");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreatePricing(ClsPricingMaster objPricingMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO pricing_master (ProductID, base_price, gst_id, effective_from, effective_to, effectiveStatus) ");
                sqlQueryBuilder.Append("VALUES (@product_id, @base_price, @gst_id, @effective_from, @effective_to, @effective_status)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objPricingMaster.PRODUCT_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@base_price", objPricingMaster.BASE_PRICE);
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objPricingMaster.GST_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_from", objPricingMaster.EFFECTIVE_FROM.HasValue ? (object)objPricingMaster.EFFECTIVE_FROM : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_to", objPricingMaster.EFFECTIVE_TO.HasValue ? (object)objPricingMaster.EFFECTIVE_TO : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_status", objPricingMaster.EFFECTIVE_STATUS ?? "ACTIVE");

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

        public int UpdatePricing(ClsPricingMaster objPricingMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE pricing_master SET ProductID = @product_id, base_price = @base_price, ");
                sqlQueryBuilder.Append("gst_id = @gst_id, effective_from = @effective_from, effective_to = @effective_to, ");
                sqlQueryBuilder.Append("effectiveStatus = @effective_status WHERE pricing_id = @pricing_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@pricing_id", objPricingMaster.PRICING_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objPricingMaster.PRODUCT_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@base_price", objPricingMaster.BASE_PRICE);
                objUtility.sqlCommand.Parameters.AddWithValue("@gst_id", objPricingMaster.GST_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_from", objPricingMaster.EFFECTIVE_FROM.HasValue ? (object)objPricingMaster.EFFECTIVE_FROM : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_to", objPricingMaster.EFFECTIVE_TO.HasValue ? (object)objPricingMaster.EFFECTIVE_TO : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@effective_status", objPricingMaster.EFFECTIVE_STATUS ?? "ACTIVE");

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

        public int DeletePricing(ClsPricingMaster objPricingMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM pricing_master WHERE pricing_id = @pricing_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@pricing_id", objPricingMaster.PRICING_ID);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (MySqlException ex)
            {
                objUtility.RollbackTransaction();
                // Check for foreign key constraint violation (error code 1451)
                if (ex.Number == 1451 || ex.Message.Contains("foreign key constraint"))
                {
                    throw new Exception("Cannot delete this pricing. This pricing is referenced in other records (e.g., orders, deliveries). Please remove those references first.");
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
