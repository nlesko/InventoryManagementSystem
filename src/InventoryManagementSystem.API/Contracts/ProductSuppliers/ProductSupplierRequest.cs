namespace InventoryManagementSystem.API.Contracts.ProductSuppliers;

public class ProductSupplierRequest
{
    public string Name { get; set; } = null!;
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ContactPerson { get; set; }
    public string? Website { get; set; }    
}