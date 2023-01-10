using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Controllers;

public class PaymentController : Controller
{
    private ILogger<ProductController> Logger1 { get; }
    private IShoppingCartDao ShoppingCart { get; }
    private IOrderHistoryDao OrderHistory { get; }

    public PaymentController(ILogger<ProductController> logger)
    {
        Logger1 = logger;
        ShoppingCart = DbManager.GetInstance().ShoppingCartDao;
        OrderHistory = DbManager.GetInstance().OrderHistoryDao;
    }

    public IActionResult Checkout()
    {
        //Logger1.LogInformation("Test1: " + ShoppingCart.GetAllForUser().ToList().Sum(x => x.Product.DefaultPrice));
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser().Sum(x => x.Product.DefaultPrice * x.Number);
        return View(new CheckoutModel());
    }

    [HttpPost]
    public IActionResult CheckoutValidation(CheckoutModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser().ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return ModelState.IsValid ? RedirectToAction("Payment", "Payment") : View("Checkout", new CheckoutModel());
    }

    public IActionResult Payment()
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser().ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return View(new PaymentModel());
    }

    [HttpPost]
    public IActionResult ValidationPayment(PaymentModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser().ToList().Sum(x => x.Product.DefaultPrice * x.Number);
        return ModelState.IsValid ? RedirectToAction("Confirmation", "Payment") : View("Payment", new PaymentModel());
    }

    public IActionResult Confirmation()
    {
        var products = ShoppingCart.GetAllForUser().ToList();
        ViewBag.TotalPrice = products.Sum(x => x.Product.DefaultPrice * x.Number);
        foreach (var product in products)
        {
            OrderHistory.Add(product);
        }
        ShoppingCart.EmptyCart();
        return View();
    }
}