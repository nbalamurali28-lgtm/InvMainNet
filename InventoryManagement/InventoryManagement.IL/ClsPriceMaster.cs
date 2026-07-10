using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace InventoryManagement.IL
{
    public class ClsAreaMaster
    {
        readonly ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public string AREA_ID { get; set; }

        public string AREA_NAME { get; set; }

        public string CREATED_AT { get; set; }

        public string UPDATED_AT { get; set; }

        public string CREATED_BY { get; set; }

        public string UPDATED_BY { get; set; }

        public string ZIP_CODE { get; set; }

        public DataTable GetAreaMaster()
        {
            DataTable dt = new DataTable();
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT area_id, area_name, ZIPCODE FROM area_master ORDER BY area_name");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        public int CreateArea(ClsAreaMaster objAreaMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("INSERT INTO area_master (area_id, area_name, created_at, ZIPCODE) VALUES ");
                sqlQueryBuilder.Append("(@area_id, @area_name, @created_at, @ZIPCODE);");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_name", objAreaMaster.AREA_NAME);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@created_at", objAreaMaster.CREATED_AT);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@ZIPCODE", objAreaMaster.ZIP_CODE);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
            }

            return rowsAffected;
        }

        public int UpdateArea(ClsAreaMaster objAreaMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("UPDATE area_master SET area_id = @area_id, area_name = @area_name, updated_at = @updated_at, ");
                sqlQueryBuilder.Append("ZIPCODE = @ZIPCODE WHERE area_id = @area_id");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_name", objAreaMaster.AREA_NAME);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@updated_at", objAreaMaster.UPDATED_AT);
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@ZIPCODE", objAreaMaster.ZIP_CODE);
                rowsAffected += objUtilitiy.ExecuteNonQueryTransaction();
                objUtilitiy.CommitTransaction();
            }
            catch (Exception)
            {
                objUtilitiy.RollbackTransaction();
            }

            return rowsAffected;
        }

        public int DeleteArea(ClsAreaMaster objAreaMaster)
        {
            int rowsAffected = 0;

            try
            {
                objUtilitiy.BeginTransaction();

                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("DELETE FROM area_master WHERE area_id = @area_id");

                objUtilitiy.sqlCommand.CommandText = sqlQueryBuilder.ToString();
                objUtilitiy.sqlCommand.Parameters.AddWithValue("@area_id", objAreaMaster.AREA_ID);
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