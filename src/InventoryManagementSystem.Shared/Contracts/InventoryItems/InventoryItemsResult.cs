namespace InventoryManagementSystem.Shared.Contracts.InventoryItems;

public class InventoryItemsResult
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; } = null!;
    public string WarehouseName { get; set; } = null!;
    public string WarehouseLocation { get; set; } = null!;
    public string WarehouseManager { get; set; } = null!;
    public string WarehouseEmailAddress { get; set; } = null!;
    public string WarehousePhoneNumber { get; set; } = null!;
}