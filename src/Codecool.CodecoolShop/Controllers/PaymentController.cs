using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services.Shopping;

namespace Codecool.CodecoolShop.Controllers;

public class PaymentController : Controller
{
    private ILogger<ProductController> Logger1 { get; }
    private ShoppingCart ShoppingCart { get; }

    public PaymentController(ILogger<ProductController> logger)
    {
        Logger1 = logger;
        ShoppingCart = ShoppingCart.GetInstance();
    }

    public IActionResult Checkout()
    {
        ViewBag.TotalPrice = ShoppingCart.Items.Sum(x => x.Product.DefaultPrice);
        return View(new CheckoutModel());
    }

    [HttpPost]
    public IActionResult CheckoutValidation(CheckoutModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.Items.Sum(x => x.Product.DefaultPrice);
        return ModelState.IsValid ? RedirectToAction("Payment", "Payment") : View("Checkout", new CheckoutModel());
    }

    public IActionResult Payment()
    {
        ViewBag.TotalPrice = ShoppingCart.Items.Sum(x => x.Product.DefaultPrice);
        return View(new PaymentModel());
    }

    [HttpPost]
    public IActionResult ValidationPayment(PaymentModel model)
    {
        ViewBag.TotalPrice = ShoppingCart.Items.Sum(x => x.Product.DefaultPrice);
        return ModelState.IsValid ? RedirectToAction("Confirmation", "Payment") : View("Payment", new PaymentModel());
    }

    public IActionResult Confirmation()
    {
        var products = ShoppingCart.Items;
        ViewBag.TotalPrice = products.Sum(x => x.Product.DefaultPrice);
        ShoppingCart.EmptyCart();
        return View();
    }
}