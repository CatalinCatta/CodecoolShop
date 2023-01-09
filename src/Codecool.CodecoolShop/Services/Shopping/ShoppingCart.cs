#nullable enable
using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services.Shopping;

public class ShoppingCart
{
    public List<Item> Items { get; private set; }

    private static ShoppingCart? _instance;

    private ShoppingCart()
    {
        Items = new List<Item>();
    }

    public static ShoppingCart GetInstance()
    {
        return _instance ??= new ShoppingCart();
    }

    public void Add(Product? product)
    {
        if (product == null)
        {
            return;
        }

        var found = false;
        foreach (var item in Items.Where(item => item.Product == product))
        {
            item.Number++;
            found = true;
        }

        if (!found)
        {
            Items.Add(new Item(product, 1));
        }
    }

    public void Remove(int id)
    {
        Items.Remove(Find(id));
    }

    public void ChangeNumber(int id, int number)
    {
        var item = Find(id);
        item.Number = number;
    }

    public void EmptyCart()
    {
        Items = new List<Item>();
    }

    private Item Find(int id) => Items.Find(x => x.Product.Id == id)!;
}