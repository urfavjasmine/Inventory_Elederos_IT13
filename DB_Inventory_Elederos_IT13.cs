using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Inventory_Elederos_IT13
{
    internal class LegacyDatabaseConfig
    {
        private static readonly string connectionString = "Data Source=DESKTOP-12345\\SQLEXPRESS;Initial Catalog=DB_Inventory_Elederos_IT13;Integrated Security=True;";
        
        public static string ConnectionString => connectionString;
    }
}
