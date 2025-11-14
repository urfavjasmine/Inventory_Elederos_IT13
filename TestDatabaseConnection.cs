using Microsoft.Data.SqlClient;
using Inventory_Elederos_IT13.Data;

namespace Inventory_Elederos_IT13
{
    public class TestDatabaseConnection
    {
        public static string TestConnection()
        {
            var connectionStrings = DatabaseConfig.GetAllConnectionStrings();
            
            foreach (var connectionString in connectionStrings)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        return $"✅ SUCCESS: Connected with: {connectionString}";
                    }
                }
                catch (Exception ex)
                {
                    // Continue to next connection string
                }
            }
            
            return "❌ FAILED: Could not connect to SQL Server with any of the configured connection strings.\n\n" +
                   "Please check:\n" +
                   "1. SQL Server is running\n" +
                   "2. SQL Server Express service is started (if using SQLEXPRESS)\n" +
                   "3. Database 'DB_Inventory_Elederos_IT13' exists\n" +
                   "4. Windows Authentication is enabled\n" +
                   "5. TCP/IP and Named Pipes are enabled in SQL Server Configuration Manager";
        }
    }
}
