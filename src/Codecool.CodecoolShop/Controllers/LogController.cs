using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Codecool.CodecoolShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Codecool.CodecoolShop.Controllers;

public class LogController : Controller
{
    private readonly IUserDao _userDao;
    private readonly IAdminDao _adminDao;
    private ILogger<LogController> Logger1 { get; }

    public LogController(ILogger<LogController> logger)
    {
        Logger1 = logger;
        var dbManager = DbManager.GetInstance();
        _userDao = dbManager.UserDao;
        _adminDao = dbManager.AdminDao;
    }
    
    public IActionResult Login(bool wrongInput=false)
    {
        ViewBag.Username = "HttpContext.Session.GetString(\"username\")";
        ViewBag.wrongInput = wrongInput;
        return View(new LoginModel());
    }

    [HttpPost]
    public IActionResult LoginValidation(LoginModel model)
    {
        ViewBag.Username = "HttpContext.Session.GetString(\"username\")";
        if (!ModelState.IsValid) return RedirectToAction("Login", "Log", new { wrongInput = true });

        var pass = PasswordManager.GenerateSaltedHash(model.Password);
        Logger1.LogInformation("hash: " + pass.Hash + " :end\nsalt: " + pass.Salt + " :end");
        Logger1.LogInformation(" " +PasswordManager.VerifyPassword("test", "", pass.Salt));
        
        
        var userExpected = _userDao.GetPassword(model.UserName);
        if (userExpected != null && PasswordManager.VerifyPassword(model.Password, userExpected.Hash, userExpected.Salt))
        {
            HttpContext.Session.SetInt32("id", _userDao.GetUserIdAndName(model.UserName)!.Item1!);
            HttpContext.Session.SetString("username", _userDao.GetUserIdAndName(model.UserName)!.Item2!);
            return RedirectToAction("Index", "Product");
        }

        var adminExpected = _adminDao.GetPassword(model.UserName);
        if (adminExpected == null || !PasswordManager.VerifyPassword(model.Password, adminExpected.Hash, adminExpected.Salt))
            return RedirectToAction("Login", "Log", new { wrongInput = true });
        HttpContext.Session.SetInt32("id", _adminDao.GetAdminIdAndName(model.UserName)!.Item1!);
        HttpContext.Session.SetString("username", _adminDao.GetAdminIdAndName(model.UserName)!.Item2!);
        return RedirectToAction("AdminIndex", "Admin");
    }

    public IActionResult SignUp(bool taken=false, bool empty = false)
    {
        ViewBag.Username = "HttpContext.Session.GetString(\"username\")";
        ViewBag.taken = taken;
        ViewBag.empty = empty;
        return View(new SignUpModel());
    }

    [HttpPost]
    public IActionResult SignupValidation(SignUpModel model)
    {
        ViewBag.Username = "HttpContext.Session.GetString(\"username\")";
        if (!ModelState.IsValid) return RedirectToAction("Signup", "Log", new { empty = true });
        
        var usernames = _userDao.GetAllNames().Concat(_adminDao.GetAllNames()).ToList();
        var emails = _userDao.GetAllEmails().Concat(_adminDao.GetAllEmails());
        if (usernames.Contains(model.UserName) || emails.Contains(model.Email.ToLower()))
            return RedirectToAction("Signup", "Log", new { taken = true });
            
        var user = new User(model.UserName, model.Email.ToLower(), PasswordManager.GenerateSaltedHash(model.Password));
        _userDao.Add(user);
        HttpContext.Session.SetInt32("id", _userDao.GetUserIdAndName(model.UserName).Item1);
        HttpContext.Session.SetString("username", model.UserName);
        return RedirectToAction("Index", "Product");
    }

    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Product");
    }
}