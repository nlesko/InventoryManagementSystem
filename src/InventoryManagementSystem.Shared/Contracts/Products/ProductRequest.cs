namespace InventoryManagementSystem.Shared.Contracts.Products;

public class ProductRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }
    public int? ProductSubCategoryId { get; set; }
    public int ProductSupplierId { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string? SKU { get; set; }
    public string? ImageUrl { get; set; }
}