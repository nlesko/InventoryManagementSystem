using InventoryManagementSystem.API.Domain.Common;

namespace InventoryManagementSystem.API.Domain.Entities;

public class ProductSupplier : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Website { get; set; }

    #region Navigation Properties
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    #endregion
}