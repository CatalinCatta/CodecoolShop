using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;

namespace Codecool.CodecoolShop.Daos;

public interface IShoppingCartDao : IDao<Product>
{
    public void ChangeNumber(int id, int number, int? userId);
    public void EmptyCart(int? userId);
    public IEnumerable<Item> GetAllForAdmin();
    public IEnumerable<Item> GetAllForUser(int? userId);
    public void Add(Product item, int? userId);
    public void Remove(int id, int? userId);
}