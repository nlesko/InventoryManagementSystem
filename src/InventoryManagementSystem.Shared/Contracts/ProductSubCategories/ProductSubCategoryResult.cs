namespace InventoryManagementSystem.Shared.Contracts.ProductSubCategories;

public class ProductSubCategoryResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }
}