using InventoryManagementSystem.API.Common.Mappings;

namespace InventoryManagementSystem.API.Contracts.InventoryItems;

public class InventoryItemsResult : IMapFrom<Domain.Entities.InventoryItem>
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

    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<Domain.Entities.InventoryItem, InventoryItemsResult>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
            .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.Warehouse.Location))
            .ForMember(dest => dest.WarehouseManager, opt => opt.MapFrom(src => $"{src.Warehouse.Manager.FirstName} {src.Warehouse.Manager.LastName}"))
            .ForMember(dest => dest.WarehouseEmailAddress, opt => opt.MapFrom(src => src.Warehouse.EmailAddress))
            .ForMember(dest => dest.WarehousePhoneNumber, opt => opt.MapFrom(src => src.Warehouse.PhoneNumber));
    }
}