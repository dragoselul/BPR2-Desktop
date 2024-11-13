using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace YourNamespace
{
    public class DatabaseHelper
    {
        private const string ConnectionString = "Host=localhost;Username=postgres;Password=123;Database=postgres";
        
        public List<string> GetAllCategories()
        {
            var categories = new List<string>();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                var query = "SELECT DISTINCT category FROM product_data ORDER BY category";
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(reader.GetString(0)); // Add each unique category
                        }
                    }
                }
            }

            return categories;
        }

        public DataTable GetProducts(int limit, int offset, string categoryFilter = null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                // Base query
                var query = "SELECT * FROM product_data";

                // Add WHERE clause if a category filter is provided
                if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
                {
                    query += " WHERE category = @Category";
                }

                // Add pagination
                query += " LIMIT @Limit OFFSET @Offset";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Limit", limit);
                    command.Parameters.AddWithValue("@Offset", offset);

                    if (!string.IsNullOrEmpty(categoryFilter) && categoryFilter != "All Categories")
                    {
                        command.Parameters.AddWithValue("@Category", categoryFilter);
                    }

                    var dataTable = new DataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

    }
}