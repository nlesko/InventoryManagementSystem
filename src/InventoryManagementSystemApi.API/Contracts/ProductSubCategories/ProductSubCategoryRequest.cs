namespace InventoryManagementSystemApi.API.Contracts.ProductSubCategories;

public class ProductSubCategoryRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }    
}