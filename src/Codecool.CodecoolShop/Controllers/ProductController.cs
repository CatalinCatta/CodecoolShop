using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Codecool.CodecoolShop.Services.Shopping;

namespace Codecool.CodecoolShop.Controllers;

public class ProductController : Controller
{
    private ILogger<ProductController> Logger1 { get; }
    private ProductService ProductService { get; }
    private ShoppingCart ShoppingCart { get; }

    public ProductController(ILogger<ProductController> logger)
    {
        Logger1 = logger;
        ProductService = new ProductService(
            ProductDaoMemory.GetInstance(),
            ProductCategoryDaoMemory.GetInstance(),
            SupplierDaoMemory.GetInstance());
        ShoppingCart = ShoppingCart.GetInstance();
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

    public IActionResult AddToCart(int id)
    {
        var product = ProductService.GetProduct(id);
        ShoppingCart.Add(product);
        return RedirectToAction(actionName: "Index", controllerName: "Product");
    }

    public IActionResult RemoveFromCart(int id)
    {
        ShoppingCart.Remove(id);
        return RedirectToAction(actionName: "Cart", controllerName: "Product");
    }

    public IActionResult AdjustCartItemNumber(int id, int number)
    {
        ShoppingCart.ChangeNumber(id, number);
        return RedirectToAction(actionName: "Cart", controllerName: "Product");
    }

    public IActionResult Cart()
    {
        return View(ShoppingCart.Items);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}