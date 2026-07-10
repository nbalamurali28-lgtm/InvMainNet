using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsProductMaster
    {
        readonly ClsUtility objUtility = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public int PRODUCT_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public int CATEGORY_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public int STOCK_QUANTITY { get; set; }
        public int REORDER_LEVEL { get; set; }
        public decimal? WEIGHT { get; set; }
        public string GAS_TYPE { get; set; }
        public bool IS_ACTIVE { get; set; }
        public string AVAILABLE_OUT_DELIVERY { get; set; }
        public string FLEXI_PRICE { get; set; }
        public DateTime? CREATED_AT { get; set; }
        public DateTime? UPDATED_AT { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_BY { get; set; }
        public string CATEGORY_NAME { get; set; }

        public DataTable GetProductMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT p.ProductID, p.ProductName, p.category_id, pc.category_name, ");
                sqlQueryBuilder.Append("p.Description, p.UnitPrice, p.StockQuantity, p.ReorderLevel, p.Weight, ");
                sqlQueryBuilder.Append("p.GasType, p.IsActive, p.available_out_delivery, p.FlexiPrice ");
                sqlQueryBuilder.Append("FROM Product p ");
                sqlQueryBuilder.Append("LEFT JOIN product_category pc ON p.category_id = pc.category_id ");
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

        public DataTable GetProductCategories()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT category_id, category_name FROM product_category ORDER BY category_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtility.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public int CreateProduct(ClsProductMaster objProductMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO Product (ProductName, category_id, Description, UnitPrice, ");
                sqlQueryBuilder.Append("StockQuantity, ReorderLevel, Weight, GasType, IsActive, available_out_delivery, FlexiPrice) ");
                sqlQueryBuilder.Append("VALUES (@product_name, @category_id, @description, @unit_price, @stock_quantity, ");
                sqlQueryBuilder.Append("@reorder_level, @weight, @gas_type, @is_active, @available_out_delivery, @flexi_price)");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_name", objProductMaster.PRODUCT_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@category_id", objProductMaster.CATEGORY_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(objProductMaster.DESCRIPTION) ? DBNull.Value : (object)objProductMaster.DESCRIPTION);
                objUtility.sqlCommand.Parameters.AddWithValue("@unit_price", objProductMaster.UNIT_PRICE);
                objUtility.sqlCommand.Parameters.AddWithValue("@stock_quantity", objProductMaster.STOCK_QUANTITY);
                objUtility.sqlCommand.Parameters.AddWithValue("@reorder_level", objProductMaster.REORDER_LEVEL);
                objUtility.sqlCommand.Parameters.AddWithValue("@weight", objProductMaster.WEIGHT.HasValue ? (object)objProductMaster.WEIGHT : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@gas_type", string.IsNullOrWhiteSpace(objProductMaster.GAS_TYPE) ? DBNull.Value : (object)objProductMaster.GAS_TYPE);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objProductMaster.IS_ACTIVE);
                objUtility.sqlCommand.Parameters.AddWithValue("@available_out_delivery", objProductMaster.AVAILABLE_OUT_DELIVERY ?? "NO");
                objUtility.sqlCommand.Parameters.AddWithValue("@flexi_price", objProductMaster.FLEXI_PRICE ?? "NO");

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

        public int UpdateProduct(ClsProductMaster objProductMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE Product SET ProductName = @product_name, category_id = @category_id, ");
                sqlQueryBuilder.Append("Description = @description, UnitPrice = @unit_price, StockQuantity = @stock_quantity, ");
                sqlQueryBuilder.Append("ReorderLevel = @reorder_level, Weight = @weight, GasType = @gas_type, IsActive = @is_active, ");
                sqlQueryBuilder.Append("available_out_delivery = @available_out_delivery, FlexiPrice = @flexi_price ");
                sqlQueryBuilder.Append("WHERE ProductID = @product_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objProductMaster.PRODUCT_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@product_name", objProductMaster.PRODUCT_NAME ?? string.Empty);
                objUtility.sqlCommand.Parameters.AddWithValue("@category_id", objProductMaster.CATEGORY_ID);
                objUtility.sqlCommand.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(objProductMaster.DESCRIPTION) ? DBNull.Value : (object)objProductMaster.DESCRIPTION);
                objUtility.sqlCommand.Parameters.AddWithValue("@unit_price", objProductMaster.UNIT_PRICE);
                objUtility.sqlCommand.Parameters.AddWithValue("@stock_quantity", objProductMaster.STOCK_QUANTITY);
                objUtility.sqlCommand.Parameters.AddWithValue("@reorder_level", objProductMaster.REORDER_LEVEL);
                objUtility.sqlCommand.Parameters.AddWithValue("@weight", objProductMaster.WEIGHT.HasValue ? (object)objProductMaster.WEIGHT : DBNull.Value);
                objUtility.sqlCommand.Parameters.AddWithValue("@gas_type", string.IsNullOrWhiteSpace(objProductMaster.GAS_TYPE) ? DBNull.Value : (object)objProductMaster.GAS_TYPE);
                objUtility.sqlCommand.Parameters.AddWithValue("@is_active", objProductMaster.IS_ACTIVE);
                objUtility.sqlCommand.Parameters.AddWithValue("@available_out_delivery", objProductMaster.AVAILABLE_OUT_DELIVERY ?? "NO");
                objUtility.sqlCommand.Parameters.AddWithValue("@flexi_price", objProductMaster.FLEXI_PRICE ?? "NO");

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

        public int DeleteProduct(ClsProductMaster objProductMaster)
        {
            int rowsAffected = 0;
            try
            {
                objUtility.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM Product WHERE ProductID = @product_id");

                objUtility.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtility.sqlCommand.Parameters.AddWithValue("@product_id", objProductMaster.PRODUCT_ID);
                rowsAffected += objUtility.ExecuteNonQueryTransaction();
                objUtility.CommitTransaction();
            }
            catch (MySqlException ex)
            {
                objUtility.RollbackTransaction();
                // Check for foreign key constraint violation (error code 1451)
                if (ex.Number == 1451 || ex.Message.Contains("foreign key constraint"))
                {
                    throw new Exception("Cannot delete this product. This product is referenced in other records (e.g., orders, deliveries, inventory). Please remove those references first.");
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
