using Microsoft.Data.SqlClient;

namespace Inventory_Elederos_IT13.Data
{
    public class DatabaseConfig
    {
        public static string GetConnectionString()
        {
            // Try multiple common SQL Server connection string formats
            var connectionStrings = new[]
            {
                "Server=localhost\\SQLEXPRESS;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=localhost;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=.;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=(local)\\SQLEXPRESS;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=(local);Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;"
            };

            return connectionStrings[0]; // Default to SQLEXPRESS
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        public static string[] GetAllConnectionStrings()
        {
            return new[]
            {
                "Server=localhost\\SQLEXPRESS;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=localhost;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=.;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=(local)\\SQLEXPRESS;Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;",
                "Server=(local);Database=DB_Inventory_Elederos_IT13;Trusted_Connection=True;TrustServerCertificate=True;"
            };
        }
    }
}
