using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace InventoryManagement.IL
{
    public class ClsUtility
    {
        public static string connectionString = ConfigurationManager.ConnectionStrings["dbConnectionName"].ToString();
        static MySqlConnection mySqlConnection;
        static MySqlTransaction mySqlTransaction;
        public MySqlCommand sqlCommand = new MySqlCommand();

        public void OpenConnection()
        {
            try
            {
                mySqlConnection = new MySqlConnection(connectionString);
                sqlCommand.Connection = mySqlConnection;

                if (mySqlConnection.State == ConnectionState.Open)
                    mySqlConnection.Close();

                mySqlConnection.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            try
            {
                mySqlConnection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetDataTable(MySqlCommand objMySqlCommand)
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection = new MySqlConnection(connectionString);
                objMySqlCommand.Connection = mySqlConnection;

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(objMySqlCommand);
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception)
            {
                throw;
            }

            return dataTable;
        }

        public DataSet GetDataSet(MySqlCommand objMySqlCommand)
        {
            DataSet dataSet = new DataSet();

            try
            {
                mySqlConnection = new MySqlConnection(connectionString);
                objMySqlCommand.Connection = mySqlConnection;

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(objMySqlCommand);
                sqlDataAdapter.Fill(dataSet);
            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;
        }

        public void ExecuteNonQuery(string sqlQuery)
        {
            try
            {
                OpenConnection();
                sqlCommand.CommandText = sqlQuery;
                sqlCommand.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExecuteNonQueryTransaction()
        {
            int rowsAffected;

            try
            {
                rowsAffected = sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
            }
            catch (Exception)
            {
                throw;
            }

            return rowsAffected;
        }

        public int ExecuteNonQueryTransaction(string sqlQuery)
        {
            int rowsAffected;

            try
            {
                //OpenConnection();
                sqlCommand.CommandText = sqlQuery;
                rowsAffected = sqlCommand.ExecuteNonQuery();
                //CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }

            return rowsAffected;
        }

        public int ExecuteNonQueryTransaction(MySqlCommand sqlCommandNew)
        {
            int rowsAffected = 0;

            try
            {
                sqlCommand.Parameters.Clear();

                /*
                foreach (MySqlParameter param in sqlCommandNew.Parameters)
                {
                    // The ICloneable interface provides a Clone() method
                    sqlCommand.Parameters.Add(((ICloneable)param).Clone());
                }
                */

                // Clone and copy the parameters to the new command
                var clonedParameters = sqlCommandNew.Parameters.Cast<ICloneable>()
                    .Select(p => p.Clone() as MySqlParameter)
                    .Where(p => p != null)
                    .ToArray();

                sqlCommand.Parameters.AddRange(clonedParameters);
                sqlCommand.CommandText = sqlCommandNew.CommandText;
                rowsAffected = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            return rowsAffected;
        }

        public void BeginTransaction()
        {
            try
            {
                OpenConnection();
                mySqlTransaction = mySqlConnection.BeginTransaction();
                sqlCommand.Connection = mySqlConnection;
                sqlCommand.Transaction = mySqlTransaction;
            }
            catch (Exception)
            {
                if (mySqlTransaction != null)
                {
                    mySqlTransaction.Rollback();
                }
                throw;
            }
        }

        public void CommitTransaction()
        {
            try
            {
                mySqlTransaction.Commit();
                CloseConnection();
            }
            catch (Exception)
            {
                if (mySqlTransaction != null)
                {
                    mySqlTransaction.Rollback();
                }
                CloseConnection();
                throw;
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                mySqlTransaction.Rollback();
                CloseConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}