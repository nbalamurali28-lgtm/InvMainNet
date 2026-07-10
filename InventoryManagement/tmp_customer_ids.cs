using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        string conn = "Server=localhost; Database=inventory_management; Uid=root; Pwd=root;";
        using (var c = new MySqlConnection(conn))
        {
            c.Open();
            using (var cmd = c.CreateCommand())
            {
                cmd.CommandText = "SELECT customer_id FROM customer_master ORDER BY customer_id";
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        Console.WriteLine(r.GetString(0));
                    }
                }
            }
        }
    }
}
