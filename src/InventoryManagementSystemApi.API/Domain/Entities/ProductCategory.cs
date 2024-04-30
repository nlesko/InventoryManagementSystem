using InventoryManagementSystemApi.API.Domain.Common;

namespace InventoryManagementSystemApi.API.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    #region Navigation Properties
    public virtual ICollection<ProductSubCategory> ProductSubCategories { get; set; } = new List<ProductSubCategory>();
    #endregion
}