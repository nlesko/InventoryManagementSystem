using InventoryManagementSystem.API.Domain.Common;

namespace InventoryManagementSystem.API.Domain.Entities;

public class Product : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public int ProductSupplierId { get; set; }
    public int ProductCategoryId { get; set; }
    public int? ProductSubCategoryId { get; set; }
    public string? SKU { get; set; }

    #region Navigation Properties
    public virtual ProductSupplier ProductSupplier { get; set; } = null!;
    public virtual ProductCategory ProductCategory { get; set; } = null!;
    public virtual ProductSubCategory? ProductSubCategory { get; set; }
    #endregion
}