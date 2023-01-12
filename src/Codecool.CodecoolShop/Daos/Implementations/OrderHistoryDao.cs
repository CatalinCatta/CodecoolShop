#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models.Shopping;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class OrderHistoryDao: IOrderHistoryDao
{
    private static OrderHistoryDao? _instance;
    private readonly string _connectionString;
    private readonly IProductDao _productDao;

    private OrderHistoryDao(string connectionString)
    {
        _connectionString = connectionString;
        _productDao = ProductDaoMemory.GetInstance(connectionString);
    }

    public static OrderHistoryDao GetInstance(string connectionString)
    {
        return _instance ??= new OrderHistoryDao(connectionString);
    }

    public void Add(Item product, int? id)
    {
        const string query =
            @"INSERT INTO order_history(user_id, product_id, quantity) VALUES (@user_id, @product_id, @quantity);";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            
            cmd.Parameters.AddWithValue("@user_id", id); 
            cmd.Parameters.AddWithValue("@product_id", product.Product.Id);
            cmd.Parameters.AddWithValue("@quantity", product.Number);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<ItemHistory> GetAllForUser(int userId)
    {
        const string cmdText = @"SELECT * FROM order_history WHERE user_id=@user_id;";
        try
        {
            var results = new List<ItemHistory>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@user_id", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var item = new ItemHistory(_productDao.Get((int)reader["product_id"]), (int)reader["quantity"], (DateTime)reader["date"]);
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

    public IEnumerable<ItemHistory> GetAllForAdmin()
    {
        const string cmdText = @"SELECT * FROM order_history";
        try
        {
            var results = new List<ItemHistory>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var item = new ItemHistory(_productDao.Get((int)reader["product_id"]), (int)reader["quantity"], (DateTime)reader["date"]);
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

    public void Add(Item item) => throw new NotImplementedException();
    public void Remove(int id) => throw new NotImplementedException();
    public Item Get(int id) => throw new NotImplementedException();
    public IEnumerable<Item> GetAll() => throw new NotImplementedException();
}