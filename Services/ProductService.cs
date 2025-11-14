using Inventory_Elederos_IT13.Models;
using Microsoft.Data.SqlClient;
using Inventory_Elederos_IT13.Data;

namespace Inventory_Elederos_IT13.Services
{
    public class ProductService
    {
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetAllProducts", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(MapReaderToProduct(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products: " + ex.Message, ex);
            }
            
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            Product? product = null;
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetProductByID", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", productId);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                product = MapReaderToProduct(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving product {productId}: " + ex.Message, ex);
            }
            
            return product;
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            var products = new List<Product>();
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_SearchProducts", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                products.Add(MapReaderToProduct(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching products: " + ex.Message, ex);
            }
            
            return products;
        }

        public async Task<int> InsertProductAsync(Product product)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_InsertProduct", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                        command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                        command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                        command.Parameters.AddWithValue("@Category", (object?)product.Category ?? DBNull.Value);
                        
                        var result = await command.ExecuteScalarAsync();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting product: " + ex.Message, ex);
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_UpdateProduct", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Description", (object?)product.Description ?? DBNull.Value);
                        command.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                        command.Parameters.AddWithValue("@ReorderLevel", product.ReorderLevel);
                        command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                        command.Parameters.AddWithValue("@Category", (object?)product.Category ?? DBNull.Value);
                        
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_DeleteProduct", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductID", productId);
                        
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product: " + ex.Message, ex);
            }
        }

        public async Task<DashboardStats> GetProductStatsAsync()
        {
            var stats = new DashboardStats();
            
            try
            {
                using (var connection = DatabaseConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("sp_GetDashboardStats", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            // Total Products
                            if (await reader.ReadAsync())
                            {
                                stats.TotalProducts = reader.GetInt32(0);
                            }
                            
                            // Low Stock Items
                            if (await reader.NextResultAsync() && await reader.ReadAsync())
                            {
                                stats.LowStockItems = reader.GetInt32(0);
                            }
                            
                            // Total Suppliers
                            if (await reader.NextResultAsync() && await reader.ReadAsync())
                            {
                                stats.TotalSuppliers = reader.GetInt32(0);
                            }
                            
                            // Total Value
                            if (await reader.NextResultAsync() && await reader.ReadAsync())
                            {
                                stats.TotalValue = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving dashboard stats: " + ex.Message, ex);
            }
            
            return stats;
        }

        private Product MapReaderToProduct(SqlDataReader reader)
        {
            return new Product
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                QuantityInStock = reader.GetInt32(3),
                ReorderLevel = reader.GetInt32(4),
                UnitPrice = reader.GetDecimal(5),
                Category = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                CreatedAt = reader.GetDateTime(7),
                UpdatedAt = reader.GetDateTime(8)
            };
        }
    }

    public class DashboardStats
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int TotalSuppliers { get; set; }
        public decimal TotalValue { get; set; }
    }
}
