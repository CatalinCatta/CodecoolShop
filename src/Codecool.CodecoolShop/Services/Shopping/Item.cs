using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services.Shopping;

public class Item
{
    public readonly Product Product;
    public int Number;

    public Item(Product product, int number)
    {
        Product = product;
        Number = number;
    }
}