namespace InventoryManagementSystemApi.API.Domain.Common;

public class BaseAuditableEntity : BaseEntity
{
    public DateTime DateCreated { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? DateLastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}