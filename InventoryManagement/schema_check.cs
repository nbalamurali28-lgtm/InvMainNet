using System;
using MySql.Data.MySqlClient;

class SchemaCheck
{
    static void Main()
    {
        string conn = "Server=localhost; Database=inventory_management; Uid=root; Pwd=root;";
        using (var c = new MySqlConnection(conn))
        {
            c.Open();
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = "SELECT COLUMN_NAME, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'inventory_management' AND TABLE_NAME = 'customer_master' ORDER BY ORDINAL_POSITION";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Console.WriteLine(string.Format("{0},{1}", r.GetString(0), r.GetString(1)));
                    }
                }
            }
        }
    }
}
