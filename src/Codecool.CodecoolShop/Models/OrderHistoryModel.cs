using System;

namespace Codecool.CodecoolShop.Models;

public class OrderHistoryModel
{
    public int OrderId;
    public string UserName;
    public string ProductName;
    public int Quantity;
    public DateTime Date;
    public decimal Price;
}