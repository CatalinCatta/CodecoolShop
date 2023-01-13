#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class OrderHistoryDao: IOrderHistoryDao
{
    private static OrderHistoryDao? _instance;
    private readonly string _connectionString;

    private OrderHistoryDao(string connectionString)
    {
        _connectionString = connectionString;
        ProductDaoMemory.GetInstance(connectionString);
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

    public IEnumerable<OrderHistoryModel> GetAllForUser(int? userId)
    {
        const string cmdText = @"
            SELECT oh.id, ud.first_name, ud.last_name, p.name, oh.quantity, oh.date, p.default_price
            FROM order_history AS oh
            LEFT JOIN users_data ud 
                ON oh.user_id = ud.user_id
            LEFT JOIN products p 
                ON p.id = oh.product_id
            WHERE oh.user_id = @userId
            ORDER BY oh.date DESC;";
        try
        {
            var results = new List<OrderHistoryModel>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var order = new OrderHistoryModel
                {
                    OrderId = (int)reader["id"],
                    UserName = reader["first_name"] + " " + reader["last_name"],
                    ProductName = reader["name"] as string,
                    Quantity = (int)reader["quantity"], 
                    Date = (DateTime)reader["date"],
                    Price = (int)reader["quantity"] * (decimal)reader["default_price"]
                };
                results.Add(order);
            }
            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            return new List<OrderHistoryModel>();
        }
    }

    public IEnumerable<OrderHistoryModel> GetAllForAdmin()
    {
        const string cmdText = @"
            SELECT oh.id, ud.first_name, ud.last_name, p.name, oh.quantity, oh.date, p.default_price
            FROM order_history AS oh
            LEFT JOIN users_data ud 
                ON oh.user_id = ud.user_id
            LEFT JOIN products p 
                ON p.id = oh.product_id
            ORDER BY oh.date DESC;";
        try
        {
            var results = new List<OrderHistoryModel>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var order = new OrderHistoryModel
                {
                    OrderId = (int)reader["id"],
                    UserName = reader["first_name"] + " " + reader["last_name"],
                    ProductName = reader["name"] as string,
                    Quantity = (int)reader["quantity"], 
                    Date = (DateTime)reader["date"],
                    Price = (int)reader["quantity"] * (decimal)reader["default_price"]
                };
                results.Add(order);
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