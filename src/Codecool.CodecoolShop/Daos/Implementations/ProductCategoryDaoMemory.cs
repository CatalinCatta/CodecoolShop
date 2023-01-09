using System.Collections.Generic;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations;

internal class ProductCategoryDaoMemory : IProductCategoryDao
{
    private readonly List<ProductCategory> _data = new();
    private static ProductCategoryDaoMemory _instance;

    private ProductCategoryDaoMemory()
    {
    }

    public static ProductCategoryDaoMemory GetInstance()
    {
        return _instance ??= new ProductCategoryDaoMemory();
    }

    public void Add(ProductCategory item)
    {
        item.Id = _data.Count + 1;
        _data.Add(item);
    }

    public void Remove(int id)
    {
        _data.Remove(Get(id));
    }

    public ProductCategory Get(int id)
    {
        return _data.Find(x => x.Id == id);
    }

    public IEnumerable<ProductCategory> GetAll()
    {
        return _data;
    }
}