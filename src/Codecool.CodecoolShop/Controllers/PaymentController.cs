using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Controllers;

public class PaymentController : Controller
{
    private ILogger<PaymentController> Logger1 { get; }
    private IShoppingCartDao ShoppingCart { get; }
    private IOrderHistoryDao OrderHistory { get; }

    public PaymentController(ILogger<PaymentController> logger)
    {
        Logger1 = logger;
        var dbManager = DbManager.GetInstance();
        ShoppingCart = dbManager.ShoppingCartDao;
        OrderHistory = dbManager.OrderHistoryDao;
    }

    public IActionResult Checkout()
    {
        //Logger1.LogInformation("Test1: " + ShoppingCart.GetAllForUser().ToList().Sum(x => x.Product.DefaultPrice));
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).Sum(x => x.Product.DefaultPrice * x.Number);
        return View(new CheckoutModel());
    }

    [HttpPost]
    public IActionResult CheckoutValidation(CheckoutModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return ModelState.IsValid ? RedirectToAction("Payment", "Payment") : View("Checkout", new CheckoutModel());
    }

    public IActionResult Payment()
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return View(new PaymentModel());
    }

    [HttpPost]
    public IActionResult ValidationPayment(PaymentModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return ModelState.IsValid ? RedirectToAction("Confirmation", "Payment") : View("Payment", new PaymentModel());
    }

    public IActionResult Confirmation()
    {
        var products = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList();
        ViewBag.TotalPrice = products.Sum(x => x.Product.DefaultPrice * x.Number);
        foreach (var product in products)
        {
            OrderHistory.Add(product, HttpContext.Session.GetInt32("id"));
        }
        ShoppingCart.EmptyCart(HttpContext.Session.GetInt32("id"));
        return View();
    }
}