using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Codecool.CodecoolShop.Models;
using Microsoft.Data.SqlClient;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class ProductDaoMemory : IProductDao
{
    private static ProductDaoMemory _instance;
    private readonly string _connectionString;
    private readonly IProductCategoryDao _productCategoryDao;
    private readonly ISupplierDao _supplierDao;

    private ProductDaoMemory(string connectionString)
    {
        _connectionString = connectionString;
        _productCategoryDao = ProductCategoryDaoMemory.GetInstance(connectionString);
        _supplierDao = SupplierDaoMemory.GetInstance(connectionString);
    }

    public static ProductDaoMemory GetInstance(string connectionString)
    {
        return _instance ??= new ProductDaoMemory(connectionString);
    }

    public void Add(Product item)
    {
        const string query = @"INSERT INTO products (name, default_price, currency, description, product_category_id, product_supplier_id)
                        VALUES (@name, @default_price, @currency, @description, @product_category_id, @product_supplier_id);";
        try
        {
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@name", item.Name);
            cmd.Parameters.AddWithValue("@default_price", item.DefaultPrice);
            cmd.Parameters.AddWithValue("@currency", item.Currency);
            cmd.Parameters.AddWithValue("@description", item.Description);
            cmd.Parameters.AddWithValue("@product_category_id", item.ProductCategory.Id);
            cmd.Parameters.AddWithValue("@product_supplier_id", item.Supplier.Id);
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
        const string query = @"DELETE FROM products WHERE id=@id;";
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

    public Product Get(int id)
    {
        const string query = @"SELECT * FROM products WHERE id=@id;";
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
            var product = new Product
            {
                Id = id,
                Name = reader.GetString("name"),
                DefaultPrice = reader.GetDecimal("default_price"),
                Currency = reader.GetString("currency"),
                Description = reader.GetString("description"),
                ProductCategory = _productCategoryDao.Get(reader.GetInt32("product_category_id")),
                Supplier = _supplierDao.Get(reader.GetInt32("product_supplier_id"))
            };
            connection.Close();
            return product;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Product> GetAll()
    {
        const string cmdText = @"SELECT * FROM products";
        try
        {
            var results = new List<Product>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = (int)reader["Id"],
                    Name = reader["name"] as string,
                    DefaultPrice = (decimal)reader["default_price"],
                    Currency = reader["currency"] as string,
                    Description = reader["description"] as string,
                    ProductCategory = _productCategoryDao.Get((int)reader["product_category_id"]),
                    Supplier = _supplierDao.Get((int)reader["product_supplier_id"])
                };
                results.Add(product);
            }
            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Product> GetBy(Supplier supplier)
    {
        const string cmdText = @"SELECT * FROM products WHERE product_supplier_id=@supplier_id";
        try
        {
            var results = new List<Product>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@supplier_id", supplier.Id);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = (int)reader["Id"],
                    Name = reader["name"] as string,
                    DefaultPrice = (decimal)reader["default_price"],
                    Currency = reader["currency"] as string,
                    Description = reader["description"] as string,
                    ProductCategory = _productCategoryDao.Get((int)reader["product_category_id"]),
                    Supplier = _supplierDao.Get((int)reader["product_supplier_id"])
                };
                results.Add(product);
            }
            connection.Close();
            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public IEnumerable<Product> GetBy(ProductCategory productCategory)
    {        
        const string cmdText = @"SELECT * FROM products WHERE product_category_id=@category_id";
        try
        {
            var results = new List<Product>();
            using var connection = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(cmdText, connection);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            cmd.Parameters.AddWithValue("@category_id", productCategory.Id);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                return results;
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = (int)reader["Id"],
                    Name = reader["name"] as string,
                    DefaultPrice = (decimal)reader["default_price"],
                    Currency = reader["currency"] as string,
                    Description = reader["description"] as string,
                    ProductCategory = _productCategoryDao.Get((int)reader["product_category_id"]),
                    Supplier = _supplierDao.Get((int)reader["product_supplier_id"])
                };
                results.Add(product);
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