using InventoryManagementSystem.API.Common.Mappings;

namespace InventoryManagementSystem.API.Contracts.ProductSuppliers;

public class ProductSuppliersResult : IMapFrom<Domain.Entities.ProductSupplier>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Website { get; set; }
}