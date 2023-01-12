#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class UserDao :IUserDao
{
    private static UserDao? _instance;
    private readonly string _connectionString;

    private UserDao(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static UserDao GetInstance(string connectionString)
    {
        return _instance ??= new UserDao(connectionString);
    }

    public HashSalt? GetPassword(string username)
    {
        const string cmdText = @"
            IF EXISTS(SELECT password, salt FROM users_log WHERE username = @username) BEGIN
                SELECT password, salt FROM users_log WHERE username = @username
            END ELSE BEGIN
                SELECT password, salt FROM users_log WHERE email = @username
            END;";
        
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            var hashSalt = new HashSalt
            {
                Hash = reader["password"] as string,
                Salt = reader["salt"] as string
            };
            connection.Close();
            return hashSalt;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }


    public Tuple<int, string> GetUserIdAndName(string username)
    {
        const string cmdText = @"
            IF EXISTS(SELECT id, username FROM users_log WHERE username = @username) BEGIN
                SELECT id, username FROM users_log WHERE username = @username
            END ELSE BEGIN
                SELECT id, username FROM users_log WHERE email = @username
            END;";
        
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            var idAndPassword = new Tuple<int, string>(
                (int)reader["id"],
                reader["username"] as string
            );
            connection.Close();
            return idAndPassword;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }
    
    public IEnumerable<string> GetAllNames()
    {
        const string query = @"SELECT username FROM users_log;";
        try
        {
            var results = new List<string>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                throw new NullReferenceException();
            }
            while (reader.Read())
            {
                var username = reader.GetString("username");
                results.Add(username);
            }
            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }
    
    public IEnumerable<string> GetAllEmails()
    {
        const string query = @"SELECT email FROM users_log;";
        try
        {
            var results = new List<string>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                throw new NullReferenceException();
            }
            while (reader.Read())
            {
                var email = reader.GetString("email");
                results.Add(email);
            }
            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void Add(User user)
    {
        
        const string query = @"INSERT INTO users_log (username, email, password, salt)
                            VALUES (@username, @email, @password, @salt);";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@username", user.UserName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password.Hash);
            cmd.Parameters.AddWithValue("@salt", user.Password.Salt);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }
    }
}