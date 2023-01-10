using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class SupplierDaoMemory : ISupplierDao
{
    private static SupplierDaoMemory _instance;
    private readonly string _connectionString;

    private SupplierDaoMemory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static SupplierDaoMemory GetInstance(string connectionString)
    {
        return _instance ??= new SupplierDaoMemory(connectionString);
    }

    public void Add(Supplier item)
    {
        const string query = @"INSERT INTO suppliers (name, description)
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
        const string query = @"DELETE FROM suppliers WHERE id=@id;";
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

    public Supplier Get(int id)
    {
        const string query = @"SELECT * FROM suppliers WHERE id=@id;";
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

            var supplier = new Supplier
            {
                Id = id,
                Name = reader.GetString("name"),
                Description = reader.GetString("description")
            };
            connection.Close();
            return supplier;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Supplier> GetAll()
    {
        const string cmdText = @"SELECT * FROM suppliers";
        try
        {
            var results = new List<Supplier>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();

            if (!reader.HasRows)
                return results;

            while (reader.Read())
            {
                var supplier = new Supplier
                {
                    Id = (int)reader["Id"],
                    Name = reader["name"] as string,
                    Description = reader["description"] as string
                };
                results.Add(supplier);
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