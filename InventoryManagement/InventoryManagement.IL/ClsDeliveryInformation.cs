using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace InventoryManagement.IL
{
    public class ClsDeliveryInformation
    {
        public ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public DataSet GetMasters()
        {
            DataSet ds;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_GET_MASTERS;");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());

                ds = objUtilitiy.GetDataSet(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        public DataSet GetDeliveryDetails(string DeliveryDate, string EmployeeId)
        {
            DataSet ds;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_GET_DELIVERY_INFORMATION (@DeliveryDate, @EmployeeId);");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
                objMySqlCommand.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                ds = objUtilitiy.GetDataSet(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        public DataTable GetDeliveryInfoPaymentsReport(string FromDate, string ToDate)
        {
            DataTable dt;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_DELIVERY_INFO_PAYMENTS_REPORT (@FromDate, @ToDate);");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@FromDate", FromDate);
                objMySqlCommand.Parameters.AddWithValue("@ToDate", ToDate);

                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public DataTable GetDeliveryInfoProductsReport(string FromDate, string ToDate)
        {
            DataTable dt;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("CALL DEL_INF_DELIVERY_INFO_PRODUCTS_REPORT (@FromDate, @ToDate);");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@FromDate", FromDate);
                objMySqlCommand.Parameters.AddWithValue("@ToDate", ToDate);

                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public int InsertTransaction(ArrayList arrListDeliveryInfo, ArrayList arrListDeliveryDetail, ArrayList arrListPaymentDetail)
        {
            int rowsAffected = 0;

            try
            {
                int Delivery_ID = 0;

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT (MAX(IFNULL(DELIVERY_ID,0)) + 1) AS DELIVERY_ID FROM DELIVERY_INF;");
                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());

                DataTable dt = objUtilitiy.GetDataTable(objMySqlCommand);
                if (dt != null && dt.Rows.Count > 0)
                    Delivery_ID = Convert.ToInt32(dt.Rows[0]["DELIVERY_ID"]);
                else
                    Delivery_ID = 1; // Start from 1 if there are no records

                objUtilitiy.BeginTransaction();

                for (int i = 0; i < arrListDeliveryInfo.Count; i++)
                {
                    DeliveryMaster objDeliveryMaster = (DeliveryMaster)arrListDeliveryInfo[i];

                    // Master Table Query
                    sqlQueryBuilder = new StringBuilder();
                    sqlQueryBuilder.Append("INSERT INTO delivery_inf (Delivery_ID, DeliveryDate, EmployeeID, Total_Amount, ");
                    sqlQueryBuilder.Append("Total_Quantity, Remarks, CreatedBy, created_at) VALUES (@Delivery_ID, @DeliveryDate, ");
                    sqlQueryBuilder.Append("@EmployeeID, @Total_Amount, @Total_Quantity, @Remarks, @CreatedBy, @created_at);");

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryMaster.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryMaster.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Total_Amount", objDeliveryMaster.TotalAmount);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Total_Quantity", objDeliveryMaster.TotalQuantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Remarks", objDeliveryMaster.Remarks);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@CreatedBy", objDeliveryMaster.CreatedBy);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryMaster.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                // Detail Table Query
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO delivery_item_details (Delivery_ID, DeliveryDate, EmployeeID, ");
                sqlQueryBuilder.Append("ProductID, ActualDelivered, OnlineQuantity, Price, Remarks, created_at) VALUES (@Delivery_ID, ");
                sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @ProductID, @ActualDelivered, @OnlineQuantity, @Price, @Remarks, @created_at);");

                for (int i = 0; i < arrListDeliveryDetail.Count; i++)
                {
                    DeliveryDetails objDeliveryDetails = (DeliveryDetails)arrListDeliveryDetail[i];

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryDetails.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryDetails.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@ProductID", objDeliveryDetails.ProductId);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@ActualDelivered", objDeliveryDetails.Quantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@OnlineQuantity", objDeliveryDetails.OnlineQuantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Price", objDeliveryDetails.Price);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Remarks", objDeliveryDetails.Remarks);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryDetails.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                // Payment Table Query
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO delivery_item_payment (Delivery_ID, DeliveryDate, EmployeeID, ");
                sqlQueryBuilder.Append("PaymentMode, Amount, created_at) VALUES (@Delivery_ID, ");
                sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @PaymentMode, @Amount, @created_at);");

                for (int i = 0; i < arrListPaymentDetail.Count; i++)
                {
                    PaymentDetails objPaymentDetails = (PaymentDetails)arrListPaymentDetail[i];

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objPaymentDetails.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objPaymentDetails.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@PaymentMode", objPaymentDetails.PaymentMode);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Amount", objPaymentDetails.Amount);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objPaymentDetails.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
                rowsAffected = 0;
            }

            return rowsAffected;
        }

        public int UpdateTransaction(ArrayList arrListDeliveryInfo, ArrayList arrListDeliveryDetail, ArrayList arrListPaymentDetail)
        {
            int rowsAffected = 0;

            try
            {
                string Delivery_ID = "";

                objUtilitiy.BeginTransaction();

                for (int i = 0; i < arrListDeliveryInfo.Count; i++)
                {
                    DeliveryMaster objDeliveryMaster = (DeliveryMaster)arrListDeliveryInfo[i];
                    Delivery_ID = objDeliveryMaster.DeliveryId;

                    // Master Table Query
                    sqlQueryBuilder = new StringBuilder();
                    sqlQueryBuilder.Append("UPDATE delivery_inf SET DeliveryDate = @DeliveryDate, EmployeeID = @EmployeeID, Total_Amount = @Total_Amount, ");
                    sqlQueryBuilder.Append("Total_Quantity = @Total_Quantity, Remarks = @Remarks, CreatedBy = @CreatedBy, created_at = @created_at ");
                    sqlQueryBuilder.Append("WHERE Delivery_ID = @Delivery_ID;");

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryMaster.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryMaster.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Total_Amount", objDeliveryMaster.TotalAmount);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Total_Quantity", objDeliveryMaster.TotalQuantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Remarks", objDeliveryMaster.Remarks);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@CreatedBy", objDeliveryMaster.CreatedBy);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryMaster.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM delivery_item_details WHERE Delivery_ID = @Delivery_ID;");
                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();

                // Detail Table Query
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO delivery_item_details (Delivery_ID, DeliveryDate, EmployeeID, ");
                sqlQueryBuilder.Append("ProductID, ActualDelivered, OnlineQuantity, Price, Remarks, created_at) VALUES (@Delivery_ID, ");
                sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @ProductID, @ActualDelivered, @OnlineQuantity, @Price, @Remarks, @created_at);");

                for (int i = 0; i < arrListDeliveryDetail.Count; i++)
                {
                    DeliveryDetails objDeliveryDetails = (DeliveryDetails)arrListDeliveryDetail[i];

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", objDeliveryDetails.DeliveryId);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objDeliveryDetails.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objDeliveryDetails.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@ProductID", objDeliveryDetails.ProductId);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@ActualDelivered", objDeliveryDetails.Quantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@OnlineQuantity", objDeliveryDetails.OnlineQuantity);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Price", objDeliveryDetails.Price);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Remarks", objDeliveryDetails.Remarks);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objDeliveryDetails.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM delivery_item_payment WHERE Delivery_ID = @Delivery_ID;");
                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();

                // Payment Detail Query
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO delivery_item_payment (Delivery_ID, DeliveryDate, EmployeeID, ");
                sqlQueryBuilder.Append("PaymentMode, Amount, created_at) VALUES (@Delivery_ID, ");
                sqlQueryBuilder.Append("@DeliveryDate, @EmployeeID, @PaymentMode, @Amount, @created_at);");

                for (int i = 0; i < arrListPaymentDetail.Count; i++)
                {
                    PaymentDetails objPaymentDetails = (PaymentDetails)arrListPaymentDetail[i];

                    objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", objPaymentDetails.DeliveryId);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@DeliveryDate", objPaymentDetails.DeliveryDate);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@EmployeeID", objPaymentDetails.EmployeeID);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@PaymentMode", objPaymentDetails.PaymentMode);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@Amount", objPaymentDetails.Amount);
                    objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objPaymentDetails.created_at);
                    rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                }

                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
                rowsAffected = 0;
            }

            return rowsAffected;
        }

        public int DeleteTransaction(string Delivery_ID)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM delivery_inf WHERE Delivery_ID = @Delivery_ID;");
                sqlQueryBuilder.Append("DELETE FROM delivery_item_details WHERE Delivery_ID = @Delivery_ID;");
                sqlQueryBuilder.Append("DELETE FROM delivery_item_payment WHERE Delivery_ID = @Delivery_ID;");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@Delivery_ID", Delivery_ID);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();

                /*
                sqlQueryBuilder.Append("DELETE FROM delivery_item_details WHERE Delivery_ID = @Delivery_ID;\n");
                sqlQueryBuilder.Replace("@Delivery_ID", Delivery_ID);

                sqlQueryBuilder.Append("DELETE FROM delivery_item_payment WHERE Delivery_ID = @Delivery_ID;\n");
                sqlQueryBuilder.Replace("@Delivery_ID", Delivery_ID);
                */

                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
                rowsAffected = 0;
            }

            return rowsAffected;
        }
    }

    public class DeliveryMaster
    {
        public string DeliveryId { get; set; }
        public string DeliveryDate { get; set; }
        public string EmployeeID { get; set; }
        public string TotalQuantity { get; set; }
        public string Remarks { get; set; }
        public string TotalAmount { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string TransactionType { get; set; }
    }

    public class DeliveryDetails
    {
        public string DeliveryId { get; set; }
        public string DeliveryDate { get; set; }
        public string EmployeeID { get; set; }
        public string ProductId { get; set; }
        public string Quantity { get; set; }
        public string OnlineQuantity { get; set; }
        public string Price { get; set; }
        public string TotalAmount { get; set; }
        public string Remarks { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string TransactionType { get; set; }
    }

    public class PaymentDetails
    {
        public string DeliveryId { get; set; }
        public string DeliveryDate { get; set; }
        public string EmployeeID { get; set; }
        public string PaymentMode { get; set; }
        public string Amount { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string TransactionType { get; set; }
    }
}