using InventoryManagementSystem.API.Common.Mappings;

namespace InventoryManagementSystem.API.Contracts.ProductSubCategories;

public class ProductSubCategoryResult : IMapFrom<Domain.Entities.ProductSubCategory>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }
}