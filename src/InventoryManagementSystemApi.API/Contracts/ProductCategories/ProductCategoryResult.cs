using InventoryManagementSystemApi.API.Common.Mappings;

namespace InventoryManagementSystemApi.API.Contracts.ProductCategories;

public class ProductCategoryResult : IMapFrom<Domain.Entities.ProductCategory>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }    
}