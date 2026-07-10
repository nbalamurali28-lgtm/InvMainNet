using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace InventoryManagement.IL
{
    public class ClsLogin
    {
        public ClsUtility objUtilitiy = new ClsUtility();
        StringBuilder sqlQueryBuilder;
        MySqlCommand objMySqlCommand;

        public DataTable GetUserDetails(string Username)
        {
            DataTable dt;
            try
            {
                sqlQueryBuilder = new StringBuilder();
                sqlQueryBuilder.Append("SELECT u.user_id, u.username, u.password, r.role_id, r.role_name, u.employee_id FROM user_master u ");
                sqlQueryBuilder.Append("JOIN role_master r ON u.role_id = r.role_id WHERE u.username = @username AND u.is_active = true;");

                objMySqlCommand = new MySqlCommand(sqlQueryBuilder.ToString());
                objMySqlCommand.Parameters.AddWithValue("@username", Username);

                dt = objUtilitiy.GetDataTable(objMySqlCommand);
            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }
    }
}