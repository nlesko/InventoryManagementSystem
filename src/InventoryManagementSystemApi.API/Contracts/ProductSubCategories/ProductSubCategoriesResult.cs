using InventoryManagementSystemApi.API.Common.Mappings;

namespace InventoryManagementSystemApi.API.Contracts.ProductSubCategories;

public class ProductSubCategoriesResult : IMapFrom<Domain.Entities.ProductSubCategory>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }    
}