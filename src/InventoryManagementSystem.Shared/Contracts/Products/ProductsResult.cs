namespace InventoryManagementSystem.Shared.Contracts.Products;

public class ProductsResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? SKU { get; set; }
    public string ProductCategory { get; set; } = null!;
    public string? ProductSubCategory { get; set; }
    public string ProductSupplier { get; set; } = null!;
}