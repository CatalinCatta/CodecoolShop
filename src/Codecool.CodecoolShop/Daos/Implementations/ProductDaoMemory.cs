using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Daos.Implementations;

public class ProductDaoMemory : IProductDao
{
    private readonly List<Product> _data = new();
    private static ProductDaoMemory _instance;

    private ProductDaoMemory()
    {
    }

    public static ProductDaoMemory GetInstance()
    {
        return _instance ??= new ProductDaoMemory();
    }

    public void Add(Product item)
    {
        item.Id = _data.Count + 1;
        _data.Add(item);
    }

    public void Remove(int id)
    {
        _data.Remove(Get(id));
    }

    public Product Get(int id)
    {
        return _data.Find(x => x.Id == id);
    }

    public IEnumerable<Product> GetAll()
    {
        return _data;
    }

    public IEnumerable<Product> GetBy(Supplier supplier)
    {
        return _data.Where(x => x.Supplier.Id == supplier.Id);
    }

    public IEnumerable<Product> GetBy(ProductCategory productCategory)
    {
        return _data.Where(x => x.ProductCategory.Id == productCategory.Id);
    }
}