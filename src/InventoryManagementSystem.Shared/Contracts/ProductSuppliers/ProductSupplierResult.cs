namespace InventoryManagementSystem.Shared.Contracts.ProductSuppliers;

public class ProductSupplierResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Website { get; set; }
}