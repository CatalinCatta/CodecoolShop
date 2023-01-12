using System.Collections.Generic;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Models.Shopping;

namespace Codecool.CodecoolShop.Daos;

public interface IOrderHistoryDao : IDao<Item>
{
    public IEnumerable<OrderHistoryModel> GetAllForAdmin();
    public IEnumerable<OrderHistoryModel> GetAllForUser(int? userId);
    public void Add(Item product, int? id);
}