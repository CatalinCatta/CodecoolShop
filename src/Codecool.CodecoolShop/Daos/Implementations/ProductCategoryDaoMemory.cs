using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

internal class ProductCategoryDaoMemory : IProductCategoryDao
{
    private readonly List<ProductCategory> _data = new();
    private static ProductCategoryDaoMemory _instance;
    private readonly string _connectionString;

    private ProductCategoryDaoMemory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static ProductCategoryDaoMemory GetInstance(string connectionString)
    {
        return _instance ??= new ProductCategoryDaoMemory(connectionString);
    }

    public void Add(ProductCategory item)
    {
        const string query = @"INSERT INTO category (name, description)
                        VALUES (@name, @description);";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@name", item.Name);
            cmd.Parameters.AddWithValue("@description", item.Description);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void Remove(int id)
    {
        const string query = @"DELETE FROM category WHERE id=@id;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public ProductCategory Get(int id)
    {
        const string query = @"SELECT * FROM category WHERE id=@id;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }

            var category = new ProductCategory
            {
                Id = id,
                Name = reader.GetString("name"),
                Description = reader.GetString("description")
            };
            connection.Close();
            return category;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<ProductCategory> GetAll()
    {
        const string cmdText = @"SELECT * FROM category";
        try
        {
            var results = new List<ProductCategory>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
                return results;

            while (reader.Read())
            {
                var productCategory = new ProductCategory
                {
                    Id = (int)reader["Id"],
                    Name = reader["name"] as string,
                    Description = reader["description"] as string
                };
                results.Add(productCategory);
            }

            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }
}