namespace Codecool.CodecoolShop.Models;

public class Product : BaseModel
{
    public string Currency { get; set; }
    public decimal DefaultPrice { get; init; }
    public ProductCategory ProductCategory { get; set; }
    public Supplier Supplier { get; set; }

    public void SetProductCategory(ProductCategory productCategory)
    {
        ProductCategory = productCategory;
        ProductCategory.Products.Add(this);
    }

    public void SetSupplier(Supplier supplier)
    {
        Supplier = supplier;
        Supplier.Products.Add(this);
    }
}