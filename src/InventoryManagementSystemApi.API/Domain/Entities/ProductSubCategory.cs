using InventoryManagementSystemApi.API.Domain.Common;

namespace InventoryManagementSystemApi.API.Domain.Entities;

public class ProductSubCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int ProductCategoryId { get; set; }

    #region Navigation Properties
    public virtual ProductCategory ProductCategory { get; set; } = null!;
    #endregion
}

public class BaseAuditablesEntity
{
}