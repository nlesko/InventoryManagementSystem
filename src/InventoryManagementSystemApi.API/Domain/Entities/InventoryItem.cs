using InventoryManagementSystemApi.API.Domain.Common;

namespace InventoryManagementSystemApi.API.Domain.Entities;

public class InventoryItem : BaseAuditableEntity
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }

    #region Navigation Properties
    public virtual Product Product { get; set; } = null!;
    public virtual Warehouse Warehouse { get; set; } = null!;
    #endregion
}