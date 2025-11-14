using Inventory_Elederos_IT13.Models;
using Microsoft.Data.SqlClient;
using Inventory_Elederos_IT13.Data;

namespace Inventory_Elederos_IT13.Services
{
    public class SupplierService
    {
        public async Task<List<Supplier>> GetAllSuppliersAsync()
        {
            var suppliers = new List<Supplier>();
            var connectionStrings = DatabaseConfig.GetAllConnectionStrings();
            Exception lastException = null;

            foreach (var connectionString in connectionStrings)
            {
                try
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();
                        using (var command = new SqlCommand("sp_GetAllSuppliers", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    suppliers.Add(new Supplier
                                    {
                                        SupplierID = reader.GetInt32(0),
                                        SupplierName = reader.GetString(1),
                                        ContactPerson = reader.GetString(2),
                                        PhoneNumber = reader.GetString(3),
                                        Email = reader.GetString(4),
                                        Address = reader.GetString(5),
                                        ProductSupplied = reader.GetString(6)
                                    });
                                }
                            }
                            return suppliers; // Success, return the data
                        }
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    continue; // Try next connection string
                }
            }

            // All connection strings failed
            throw new Exception($"Unable to connect to SQL Server. Please ensure SQL Server is running and the database 'DB_Inventory_Elederos_IT13' exists. Tried {connectionStrings.Length} connection options. Last error: {lastException?.Message}");
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int supplierId)
        {
            Supplier? supplier = null;
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetSupplierByID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", supplierId);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                supplier = MapReaderToSupplier(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving supplier {supplierId}: " + ex.Message, ex);
            }
            
            return supplier;
        }

        public async Task<List<Supplier>> SearchSuppliersAsync(string searchTerm)
        {
            var suppliers = new List<Supplier>();
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_SearchSuppliers", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                suppliers.Add(MapReaderToSupplier(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching suppliers: " + ex.Message, ex);
            }
            
            return suppliers;
        }

        public async Task<int> InsertSupplierAsync(Supplier supplier)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_InsertSupplier", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                        command.Parameters.AddWithValue("@ContactPerson", (object?)supplier.ContactPerson ?? DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", (object?)supplier.PhoneNumber ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ProductSupplied", (object?)supplier.ProductSupplied ?? DBNull.Value);
                        
                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting supplier: " + ex.Message, ex);
            }
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateSupplier", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@SupplierID", supplier.SupplierID);
                        command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                        command.Parameters.AddWithValue("@ContactPerson", (object?)supplier.ContactPerson ?? DBNull.Value);
                        command.Parameters.AddWithValue("@PhoneNumber", (object?)supplier.PhoneNumber ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object?)supplier.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Address", (object?)supplier.Address ?? DBNull.Value);
                        command.Parameters.AddWithValue("@ProductSupplied", (object?)supplier.ProductSupplied ?? DBNull.Value);
                        
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating supplier: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteSupplierAsync(int supplierId)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteSupplier", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SupplierID", supplierId);
                        
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting supplier: " + ex.Message, ex);
            }
        }

        private Supplier MapReaderToSupplier(SqlDataReader reader)
        {
            return new Supplier
            {
                SupplierID = reader.GetInt32(0),
                SupplierName = reader.GetString(1),
                ContactPerson = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                PhoneNumber = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Email = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Address = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                ProductSupplied = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                CreatedAt = reader.GetDateTime(7),
                UpdatedAt = reader.GetDateTime(8)
            };
        }
    }
}
