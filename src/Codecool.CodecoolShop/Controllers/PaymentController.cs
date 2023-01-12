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
    private IUserDao UserDao { get; }

    public PaymentController(ILogger<PaymentController> logger)
    {
        Logger1 = logger;
        var dbManager = DbManager.GetInstance();
        ShoppingCart = dbManager.ShoppingCartDao;
        OrderHistory = dbManager.OrderHistoryDao;
        UserDao = dbManager.UserDao;
    }

    public IActionResult Checkout()
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id"))
            .Sum(x => x.Product.DefaultPrice * x.Number);
        var model = UserDao.GetCheckoutData(HttpContext.Session.GetInt32("id"))?? new CheckoutModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult CheckoutValidation(CheckoutModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList()
            .Sum(x => x.Product.DefaultPrice * x.Number);
        if (!ModelState.IsValid) return View("Checkout", new CheckoutModel());

        if (model.SaveData) UserDao.SaveCheckoutData(model, HttpContext.Session.GetInt32("id"));
        return RedirectToAction("Payment", "Payment");
    }

    public IActionResult Payment()
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList()
            .Sum(x => x.Product.DefaultPrice * x.Number);
        var model = UserDao.GetPaymentData(HttpContext.Session.GetInt32("id")) ?? new PaymentModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult ValidationPayment(PaymentModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.GetAllForUser(HttpContext.Session.GetInt32("id")).ToList()
            .Sum(x => x.Product.DefaultPrice * x.Number);
        if (!ModelState.IsValid) return View("Payment", new PaymentModel());

        if (model.SaveData) UserDao.SavePaymentData(model, HttpContext.Session.GetInt32("id"));
        return RedirectToAction("Confirmation", "Payment");
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