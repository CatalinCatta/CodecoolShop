using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;

namespace Codecool.CodecoolShop.Daos;

public interface IShoppingCartDao : IDao<Product>
{
    public void ChangeNumber(int id, int number);
    public void EmptyCart();
    public IEnumerable<Item> GetAllForAdmin();
    public IEnumerable<Item> GetAllForUser();
    public new void Add(Product item);
    public new void Remove(int id);
}