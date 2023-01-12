using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Manager;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.CodecoolShop.Controllers;

public class AdminController :Controller
{
    private readonly IAdminDao _adminDao;
    private readonly IOrderHistoryDao _orderHistoryDao;
    
    public AdminController()
    {
        var dbManager = DbManager.GetInstance();
        _adminDao = dbManager.AdminDao;
        _orderHistoryDao = dbManager.OrderHistoryDao;
    }

    public IActionResult AdminIndex()
    {
        return View(_orderHistoryDao.GetAllForAdmin());
    }
}