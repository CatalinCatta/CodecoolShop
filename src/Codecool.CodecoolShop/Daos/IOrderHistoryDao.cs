using System.Collections.Generic;
using Codecool.CodecoolShop.Models.Shopping;

namespace Codecool.CodecoolShop.Daos;

public interface IOrderHistoryDao : IDao<Item>
{
    public IEnumerable<ItemHistory> GetAllForAdmin();
    public IEnumerable<ItemHistory> GetAllForUser(int userId);
    public void Add(Item product, int? id);
}