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
    
    public string GetEmail(int? userId)
    {
        const string cmdText = @"SELECT email FROM users_log WHERE id = @userId;";
        
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return "";
            var email = reader["email"] as string;
            connection.Close();
            return email;
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
    
    public void SaveCheckoutData(CheckoutModel checkoutData, int? userId)
    {
        const string query = 
            @"IF EXISTS (SELECT * FROM users_data WHERE user_id = @userId)
            BEGIN
                UPDATE users_data 
                    SET first_name=@firstName, last_name=@lastName, address_1=@address1, address_2=@address2, phone_number=@phoneNumber, city=@city, country=@country, zip_code=@zipCode
                    WHERE user_id = @userId
            END
            ELSE
            BEGIN
                INSERT INTO users_data 
                    (user_id, first_name, last_name, address_1, address_2, phone_number, city, country, zip_code)
                    VALUES (@userId, @firstName, @lastName, @address1, @address2, @phoneNumber, @city, @country, @zipCode)
            END;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@firstName", checkoutData.FirstName);
            cmd.Parameters.AddWithValue("@lastName", checkoutData.LastName);
            cmd.Parameters.AddWithValue("@address1", checkoutData.AddressLine1);
            cmd.Parameters.AddWithValue("@address2", checkoutData.AddressLine2 ?? "");
            cmd.Parameters.AddWithValue("@phoneNumber", checkoutData.PhoneNumber);
            cmd.Parameters.AddWithValue("@city", checkoutData.City);
            cmd.Parameters.AddWithValue("@country", checkoutData.Country);
            cmd.Parameters.AddWithValue("@zipCode", checkoutData.ZipCode);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
            throw new RuntimeWrappedException(e);
        }
    }

    public void SavePaymentData(PaymentModel paymentData, int? userId)
    {
        const string query = 
            @"IF EXISTS (SELECT * FROM users_data WHERE user_id = @userId)
            BEGIN
                UPDATE users_data 
                    SET card_holder=@cardHolder, card_number=@cardNumber, expiry_month=@expiryMonth, expiry_year=@expiryYear, cvv=@cvv
                    WHERE user_id = @userId
            END
            ELSE
            BEGIN
                INSERT INTO users_data 
                    (users_id, card_holder, card_number, expiry_month, expiry_year, cvv)
                    VALUES (@userId , @cardHolder, @cardNumber, @expiryMonth, @expiryYear, @cvv)
            END;";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@cardHolder", paymentData.CardHolder);
            cmd.Parameters.AddWithValue("@cardNumber", paymentData.CardNumber);
            cmd.Parameters.AddWithValue("@expiryMonth", paymentData.ExpiryMonth);
            cmd.Parameters.AddWithValue("@expiryYear", paymentData.ExpiryYear);
            cmd.Parameters.AddWithValue("@cvv", paymentData.Cvv);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        catch (SqlException e)
        {
            Console.WriteLine(e);
        }
    }

    public CheckoutModel? GetCheckoutData(int? userId)
    {
        const string cmdText = @"SELECT * FROM users_data WHERE user_id=@userId;";
        
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            var checkoutData = new CheckoutModel{
                FirstName = reader["first_name"] as string,
                LastName = reader["last_name"] as string,
                AddressLine1 = reader["address_1"] as string,
                AddressLine2 = reader["address_2"] as string,
                PhoneNumber = reader["phone_number"] as string,
                Email = GetEmail(userId),
                City = reader["city"] as string,
                Country = reader["country"] as string,
                ZipCode = reader["zip_code"] as string,
                SaveData = true
            };
            connection.Close();
            return checkoutData;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public PaymentModel? GetPaymentData(int? userId)
    {
        const string cmdText = @"SELECT * FROM users_data WHERE user_id=@userId;";
        
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@userId", userId);
            var reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
            var paymentData = new PaymentModel{
                CardHolder = reader["card_holder"] as string,
                CardNumber = reader["card_number"] as string,
                ExpiryMonth = reader["expiry_month"] as string,
                ExpiryYear = reader["expiry_year"] as string,
                Cvv = reader["cvv"] as string,
                SaveData = true
            };
            connection.Close();
            return paymentData;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }
}