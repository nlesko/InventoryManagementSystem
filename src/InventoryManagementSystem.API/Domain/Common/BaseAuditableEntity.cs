namespace InventoryManagementSystem.API.Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    public DateTime DateCreatedUtc { get; set; }

    public int CreatedById { get; set; }

    public DateTime? DateLastModifiedUtc { get; set; }

    public int LastModifiedById { get; set; }
}