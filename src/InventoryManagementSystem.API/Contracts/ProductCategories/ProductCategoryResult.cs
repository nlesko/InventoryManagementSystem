using InventoryManagementSystem.API.Common.Mappings;

namespace InventoryManagementSystem.API.Contracts.ProductCategories;

public class ProductCategoryResult : IMapFrom<Domain.Entities.ProductCategory>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }    
}