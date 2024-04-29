using InventoryManagementSystemApi.API.Domain.Common;

namespace InventoryManagementSystemApi.API.Domain.Entities;

public class Warehouse : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public int ManagerId { get; set; }
    public string EmailAddress { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;

    #region Navigation Properties
    public virtual ApplicationUser Manager { get; set; } = null!;
    #endregion
}