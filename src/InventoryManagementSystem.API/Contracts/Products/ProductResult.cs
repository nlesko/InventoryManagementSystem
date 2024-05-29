using AutoMapper;

using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Contracts.ProductCategories;
using InventoryManagementSystem.API.Contracts.ProductSubCategories;
using InventoryManagementSystem.API.Contracts.ProductSuppliers;
using InventoryManagementSystem.API.Domain.Entities;

namespace InventoryManagementSystem.API.Contracts.Products;

public class ProductResult : IMapFrom<Product>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? SKU { get; set; }
    public ProductCategoryResult ProductCategory { get; set; } = null!;
    public ProductSubCategoryResult? ProductSubCategory { get; set; }
    public ProductSupplierResult ProductSupplier { get; set; } = null!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Entities.Product, ProductResult>()
            .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory))
            .ForMember(dest => dest.ProductSubCategory, opt => opt.MapFrom(src => src.ProductSubCategory))
            .ForMember(dest => dest.ProductSupplier, opt => opt.MapFrom(src => src.ProductSupplier));
    }
}