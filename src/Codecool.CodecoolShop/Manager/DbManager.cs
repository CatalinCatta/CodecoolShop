#nullable enable
using System;
using System.Collections.Generic;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.IO;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Codecool.CodecoolShop.Manager;

public class DbManager
{
    public readonly IProductDao ProductDao;
    public readonly ISupplierDao SupplierDao;
    public readonly IShoppingCartDao ShoppingCartDao;
    public readonly IProductCategoryDao ProductCategoryDao;
    public readonly IOrderHistoryDao OrderHistoryDao;
    private static DbManager? _instance;

    private DbManager()
    {
        EnsureConnectionSuccessful();
        ProductDao = ProductDaoMemory.GetInstance(ConnectionString());
        SupplierDao = SupplierDaoMemory.GetInstance(ConnectionString());
        ShoppingCartDao = Daos.Implementations.ShoppingCartDao.GetInstance(ConnectionString());
        ProductCategoryDao = ProductCategoryDaoMemory.GetInstance(ConnectionString());
        OrderHistoryDao = Daos.Implementations.OrderHistoryDao.GetInstance(ConnectionString());
    }

    public static DbManager GetInstance()
    {
        return _instance ??= new DbManager();
    }
    
    private static void EnsureConnectionSuccessful()
    {
        if (!TestConnection())
        {
            Environment.Exit(1);
        }
    }

    private static string ConnectionString()
    {
        using var r = new StreamReader("appsettings.json");
        var jObj = JObject.Parse(r.ReadToEnd());
        var connectionStrings = jObj["ConnectionStrings"]!.ToString();
        return JsonConvert.DeserializeObject<ConectionString>(connectionStrings)!.DefaultConnection;
    }

    private static bool TestConnection()
    {
        using var connection = new SqlConnection(ConnectionString());
        try
        {
            connection.Open();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

}