using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.CodecoolShop.Controllers;

public class AdminController :Controller
{
    private readonly IAdminDao _adminDao;
    
    public AdminController()
    {
        var dbManager = DbManager.GetInstance();
        _adminDao = dbManager.AdminDao;
    }

    public IActionResult AdminIndex()
    {
        return View();
    }
}