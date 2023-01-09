using System.Collections.Generic;
using System.Linq;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Models;

namespace Codecool.CodecoolShop.Services;

public class ProductService
{
    private readonly IProductDao _productDao;
    private readonly IProductCategoryDao _productCategoryDao;
    private readonly ISupplierDao _productSupplierDao;

    public ProductService(IProductDao productDao, IProductCategoryDao productCategoryDao,
        ISupplierDao productSupplierDao)
    {
        _productDao = productDao;
        _productCategoryDao = productCategoryDao;
        _productSupplierDao = productSupplierDao;
    }

    public ProductCategory GetProductCategory(int categoryId) => _productCategoryDao.Get(categoryId);

    public Supplier GetProductSuppliers(int supplierId) => _productSupplierDao.Get(supplierId);

    public IEnumerable<Product> GetProductsForCategory(int categoryId) =>
        _productDao.GetBy(_productCategoryDao.Get(categoryId));

    public IEnumerable<Product> GetProductsForSupplier(int supplierId) =>
        _productDao.GetBy(_productSupplierDao.Get(supplierId));

    public IEnumerable<Product> GetAllProducts() => _productDao.GetAll();

    public Product GetProduct(int id) => _productDao.GetAll().FirstOrDefault(product => product.Id == id);
}