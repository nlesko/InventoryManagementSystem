using AutoMapper;

using InventoryManagementSystemApi.API.Common.Mappings;
using InventoryManagementSystemApi.API.Domain.Entities;

namespace InventoryManagementSystemApi.API.Contracts.Products;

public class ProductsResult : IMapFrom<Product>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? SKU { get; set; }
    public string ProductCategory { get; set; } = null!;
    public string? ProductSubCategory { get; set; }
    public string ProductSupplier { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Product, ProductsResult>()
            .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory.Name))
            .ForMember(dest => dest.ProductSubCategory, opt => opt.MapFrom(src => src.ProductSubCategory != null ? src.ProductSubCategory.Name : null))
            .ForMember(dest => dest.ProductSupplier, opt => opt.MapFrom(src => src.ProductSupplier.Name));
    }
}