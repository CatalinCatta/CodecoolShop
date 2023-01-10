#nullable enable
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class ShoppingCartDao : IShoppingCartDao
{
    private static ShoppingCartDao? _instance;
    private readonly string _connectionString;
    private readonly IProductDao _productDao;

    private ShoppingCartDao(string connectionString)
    {
        _connectionString = connectionString;
        _productDao = ProductDaoMemory.GetInstance(connectionString);
    }

    public static ShoppingCartDao GetInstance(string connectionString)
    {
        return _instance ??= new ShoppingCartDao(connectionString);
    }

    public void Add(Product product)
    {
        const string query =
            @"IF EXISTS(select quantity from cart where user_id=@user_id AND product_id = @product_id) BEGIN
                Update cart set quantity=quantity + 1 where user_id=@user_id AND product_id = @product_id
            END ELSE BEGIN
                Insert into cart (user_id, product_id) VALUES (@user_id, @product_id)END;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@user_id", 1); // *** TO DO ***  Implement user
            cmd.Parameters.AddWithValue("@product_id", product.Id);
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
        const string query = @"DELETE FROM cart WHERE product_id=@id AND user_id=@user_id;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@user_id", 1); // *** TO DO ***  Implement user
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void ChangeNumber(int id, int number)
    {
        const string query = @"UPDATE cart SET quantity = @number WHERE product_id=@id AND user_id=@user_id;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@user_id", 1); // *** TO DO ***  Implement user
            cmd.Parameters.AddWithValue("@number", number);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void EmptyCart()
    {
        const string query = @"DELETE FROM cart WHERE user_id=@id;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@id", 1); // *** TO DO ***  Implement user
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Item> GetAllForUser()
    {
        const string cmdText = @"SELECT * FROM cart WHERE user_id=@user_id;";
        try
        {
            var results = new List<Item>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@user_id", 1); // *** TO DO ***  Implement user
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var item = new Item(_productDao.Get((int)reader["product_id"]), (int)reader["quantity"]);
                results.Add(item);
            }

            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Item> GetAllForAdmin()
    {
        const string cmdText = @"SELECT * FROM cart";
        try
        {
            var results = new List<Item>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var item = new Item(_productDao.Get((int)reader["product_id"]), (int)reader["quantity"]);
                results.Add(item);
            }

            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public Product Get(int id)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Product> GetAll()
    {
        throw new System.NotImplementedException();
    }
}