using System;

namespace Codecool.CodecoolShop.Models.Shopping;

public class ItemHistory:Item
{
    public readonly DateTime OrderDate;

    public ItemHistory(Product product, int number, DateTime date) : base(product, number)
    {
        OrderDate = date;
    }
}