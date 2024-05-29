using InventoryManagementSystem.API.Common.Mappings;

namespace InventoryManagementSystem.API.Contracts.Warehouses;

public class WarehouseResult : IMapFrom<Domain.Entities.Warehouse>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public int Capacity { get; set; }
    public int ManagerId { get; set; }
    public string EmailAddress { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}