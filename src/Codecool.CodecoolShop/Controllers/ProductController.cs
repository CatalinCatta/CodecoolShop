using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Codecool.CodecoolShop.Controllers;

public class ProductController : Controller
{
    private ILogger<ProductController> Logger1 { get; }
    private ProductService ProductService { get; }
    private IProductDao ProductDao { get; }
    private IShoppingCartDao ShoppingCart { get; }

    public ProductController(ILogger<ProductController> logger)
    {
        var dbManager = DbManager.GetInstance();
        Logger1 = logger;
        ProductService = new ProductService(
            dbManager.ProductDao,
            dbManager.ProductCategoryDao,
            dbManager.SupplierDao);
        ShoppingCart = dbManager.ShoppingCartDao;
        ProductDao = dbManager.ProductDao;
    }

    public IActionResult Index(int category = 0, int supplier = 0)
    {
        IEnumerable<Product> products;
        if (category == 0 && supplier == 0)
        {
            products = ProductService.GetAllProducts();
        }
        else if (category != 0)
        {
            products = ProductService.GetProductsForCategory(category);
        }
        else
        {
            products = ProductService.GetProductsForSupplier(supplier);
        }

        return View(products.ToList());
    }

    public void AddToCart(int id)
    {
        var product = ProductService.GetProduct(id);
        ShoppingCart.Add(product, HttpContext.Session.GetInt32("id"));
    }
    
    public decimal RemoveFromCart(int id)
    {
        ShoppingCart.Remove(id, HttpContext.Session.GetInt32("id"));
        return ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).Sum(i => i.Product.DefaultPrice * i.Number);
    }

    public string AdjustCartItemNumber(int id, int number)
    {
        ShoppingCart.ChangeNumber(id, number, HttpContext.Session.GetInt32("id"));
        return JsonConvert.SerializeObject(new ChangeCartNumber
        {
            ProductPrice = ProductDao.Get(id).DefaultPrice * number,
            CartNewPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id"))
                .Sum(i => i.Product.DefaultPrice * i.Number),
            CartItemsNumber = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id"))
                .Sum(i => i.Number)
        });
    }

    public IActionResult Cart()
    {
        return View(ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private class ChangeCartNumber
    {
        public decimal ProductPrice { get; set; }
        public decimal CartItemsNumber { get; set; }
        public decimal CartNewPrice { get; set; }
    }
}