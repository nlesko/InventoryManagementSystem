namespace InventoryManagementSystem.Shared.Contracts.InventoryItems;

public class InventoryItemRequest
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
}