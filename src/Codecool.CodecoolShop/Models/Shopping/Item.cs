namespace Codecool.CodecoolShop.Models.Shopping;

public class Item
{
    public readonly Product Product;
    public readonly int Number;

    public Item(Product product, int number)
    {
        Product = product;
        Number = number;
    }
}