using InventoryManagementSystem.Shared.Contracts.ProductCategories;
using InventoryManagementSystem.Shared.Contracts.ProductSubCategories;
using InventoryManagementSystem.Shared.Contracts.ProductSuppliers;

namespace InventoryManagementSystem.Shared.Contracts.Products;

public class ProductResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? SKU { get; set; }
    public ProductCategoryResult ProductCategory { get; set; } = null!;
    public ProductSubCategoryResult? ProductSubCategory { get; set; }
    public ProductSupplierResult ProductSupplier { get; set; } = null!;    
}